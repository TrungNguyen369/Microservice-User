using UserService.Data;
using UserService.Models;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            return _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.SingleOrDefault(x => x.Username == username);
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public bool ValidateRefreshToken(string username, string refreshToken)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username && x.RefreshToken == refreshToken);
            return user != null && user.RefreshTokenExpiryTime > DateTime.Now;
        }
    }
}
