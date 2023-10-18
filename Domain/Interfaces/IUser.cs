using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUser
    {
        Task AddUser(string email, string password, int age, string telephone);

        Task<bool> ExistUser(string email, string password);

        Task<string> ReturnUserId(string email);
    }
}