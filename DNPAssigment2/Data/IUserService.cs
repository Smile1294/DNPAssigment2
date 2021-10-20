﻿using System.Threading.Tasks;
using DNPAssigment1.Login;

namespace DNPAssigment1.Login
{
    public interface IUserService
    {
        public Task<User> ValidateUser(string userName, string passWord);
    }
}