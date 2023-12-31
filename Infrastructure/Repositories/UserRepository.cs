﻿using Domain.Interfaces;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Configuration;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUser
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public UserRepository()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task AddUser(string email, string password, int age, string telephone)
        {
            try
            {
                using (var data = new ContextBase(_OptionsBuilder))
                {
                    await data.ApplicationUser.AddAsync(
                        new ApplicationUser
                        {
                            Email = email,
                            PasswordHash = password,
                            Age = age,
                            Telephone = telephone,
                            Usertype = Usertype.Common
                        });
                    await data.SaveChangesAsync();
                }
            }
            catch(Exception)
            {
                throw new Exception("Erro ao adicionar");
            }
        }

        public async Task<bool> ExistUser(string email, string password)
        {
            try
            {
                using (var data = new ContextBase(_OptionsBuilder))
                {
                    return await data.ApplicationUser
                        .Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(password))
                        .AsNoTracking()
                        .AnyAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> ReturnUserId(string email)
        {
            try
            {
                using (var data = new ContextBase(_OptionsBuilder))
                {
                    var getUserId =  await data.ApplicationUser
                        .Where(u => u.Email.Equals(email))
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                    return getUserId.Id;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}