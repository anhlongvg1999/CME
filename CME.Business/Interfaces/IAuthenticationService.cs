using CME.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CME.Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseModel> Login(string username, string password,string macaddress);
    }
}
