using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User GetByUsername(string username);
        void Create(User user);
        void Update(User user);
        bool ValidateRefreshToken(string username, string refreshToken);
    }
}
