using System;
using System.Threading.Tasks;
using AuthMicroservice.Core.Models.Dto;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public Task<string> Register(RegisterDto dto);
        public string Login(LoginDto dto);
        public string RefreshToken();
        public void ChangePassword(ChangePassword dto);
        public void CloseAccount();
        public void RegisterConfirmation(Guid id);
        public string RequestPasswordReset(string username, Password dto);
        public void PasswordResetConfirmation(Guid id);
    }
}
