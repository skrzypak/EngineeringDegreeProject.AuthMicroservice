using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Models.Dto;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public void Register(RegisterDto dto);
        public string Login(LoginDto dto);
        public string RefreshLogin();
        public string Logout();
        public void ChangePassword(ChangePassword dto);
        public void CloseAccount();
    }
}
