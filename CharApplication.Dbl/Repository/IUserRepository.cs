﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApplication.Dbl.Models;

namespace ChatApplication.Dbl.Repository
{
    public interface IUserRepository
    {
        Task<DbUser> Create(DbUser user);
        Task Delete(int id);
        Task<DbUser> Get(int id);
        Task <List<DbUser>> GetUsers();
        Task Update(DbUser user);
    }
}