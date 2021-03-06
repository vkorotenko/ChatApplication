﻿#region License
// Разработано: Коротенко Владимиром Николаевичем (Vladimir N. Korotenko)
// email: koroten@ya.ru
// skype:vladimir-korotenko 
// https://vkorotenko.ru
// Создано:  13.04.2019 22:30
#endregion

using AutoMapper;
using ChatApplication.Code;
using ChatApplication.Dbl;
using ChatApplication.Dbl.Models;
using ChatApplication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Threading.Tasks;
using ChatApplication.Hubs;

namespace ChatApplication
{
    /// <summary>
    /// Конфигурация сервера
    /// </summary>
    public class Startup
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Конструктор с конфигурацией сервера. Вызывается инфраструктурой при старте
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="logger">Логгер</param>
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Конфигурирование сервисов
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            _logger.LogInformation("Start configure services");
            ConfigureMapper();            
            services.AddScoped<IDbContext, DbContext>();
            services.AddLogging();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,

                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,

                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
            services.AddSwaggerGen(c =>
            {
                var xml = GetXmlCommentsPath();
                if(!string.IsNullOrWhiteSpace(xml)) c.IncludeXmlComments(GetXmlCommentsPath());
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Internal messages API",
                    Description = "ASP.NET Core internal message API",
                    Contact = new Contact()
                    {
                        Name = "Vladimir Korotenko",
                        Email = "koroten@ya.ru",
                        Url = "https://vkorotenko.ru"
                    }
                });                
            });            
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddSignalR();
            _logger.LogInformation("End configuring services");
        }

        private void ConfigureMapper()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<DbUser, ApplicationUser>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName));
                cfg.CreateMap<DbMessage, MessageModel>();
                cfg.CreateMap<DbFile, UploadFile>()
                    .ForMember(dest => dest.Upload, opt => opt.MapFrom(src => src.Created));
                cfg.CreateMap<DbFile, AttachmentModel>();
                
                cfg.CreateMap<DbMessage, LatestMessageModel>()
                    .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
                    .ForMember(dest=> dest.Url, opt=> opt.MapFrom(src => $"/user/chat/?id={src.TopicId}") )
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Body)); 
            });
        }

        

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private string GetXmlCommentsPath()
        {
            try
            {
                var app = System.AppContext.BaseDirectory;
                return System.IO.Path.Combine(app, "ChatApplication.xml");
            }
            catch (FileNotFoundException fnfe)
            {
                _logger.LogError("ChatApplication.xml not found");
                _logger.LogError(fnfe.Message);
                return String.Empty;
            }
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Приложение</param>
        /// <param name="env">Переменные среды</param>   
        /// <param name="loggerFactory">Фабрика логгирования</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var fillPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs/logger.txt");
            try
            {
                // loggerFactory.AddFile(fillPath);
                // var logger = loggerFactory.CreateLogger("FileLogger");
            }
            catch (Exception ex)
            {                
                _logger.LogError($"Cold not add log file: {fillPath}");
                _logger.LogError(ex.Message);
            }
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            _logger.LogInformation("App start");
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseSwagger();            
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Internal messages V1"); });
            app.UseCors();
            //app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseMvc();
        }
        
    }
}
