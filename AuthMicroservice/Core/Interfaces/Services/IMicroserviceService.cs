using AuthMicroservice.Core.Models.Dto;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public void Register(RegisterDto dto);
        public string Login(LoginDto dto);
        public void ChangePassword(ChangePassword dto);
        public void CloseAccount();
    }
}
