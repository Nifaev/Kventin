using Kventin.Services.Dtos.User;

namespace Kventin.Services.Interfaces.Services
{
    public interface IUserService
    {
        public Task Register(RegisterUserDto dto);
        public Task<string> Login(LoginUserDto dto);
    }
}
