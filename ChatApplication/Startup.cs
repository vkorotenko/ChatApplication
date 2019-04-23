#region License
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
using ChatApplication.Poco;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ChatApplication
{
    /// <summary>
    /// Конфигурация сервера
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Конструктор с конфигурацией сервера. Вызывается инфраструктурой при старте
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            ConfigureMapper();
            // получаем строку подключения из файла конфигурации
            string connection = Configuration.GetConnectionString("DefaultConnection");
            // добавляем контекст DbContext для всего приложения
            var dbcon = new DbContext(connection);
            services.AddSingleton<DbContext>(dbcon);
            
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
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Internal messages API",
                    Description = "ASP.NET Core internal message API",
                    Contact = new Contact() { Name = "Vladimir Korotenko", Email = "koroten@ya.ru", Url = "www.vkorotenko.ru" }
                });                
            });            
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*",
                                "http://www.contoso.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });            
        }

        private void ConfigureMapper()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<DbUser, ApplicationUser>();
            });
        }

        

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private string GetXmlCommentsPath()
        {
            var app = System.AppContext.BaseDirectory;
            return System.IO.Path.Combine(app, "ChatApplication.xml");
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseSwagger();            
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Internal messages V1"); });
            app.UseCors();
            //app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
