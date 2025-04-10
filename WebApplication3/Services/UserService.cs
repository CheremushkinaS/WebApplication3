using WebApplication3.Model.UserAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace WebApplication3.Service
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(
        ApplicationDbContext context)
        {
            _context = context;
           
        }

        public async Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registrationDto.Email))
            {
                return UserRegistrationResult.Error("Пользователь с таким email уже существует");
            }


            var user = new User
            {
                Name = registrationDto.Name,
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                RoleId = 3 // Роль студента
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return UserRegistrationResult.Success(user);
        }
        
        public class UserRegistrationDto
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        public class UserRegistrationResult
        {
            public bool UseSuccess { get; }
            public string UseError { get; }
            public User User { get; }

            private UserRegistrationResult(bool success, User user, string error)
            {
                UseSuccess = success;
                User = user;
                UseError = error;
            }

            public static UserRegistrationResult Success(User user)
                => new UserRegistrationResult(true, user, null);

            public static UserRegistrationResult Error(string error)
                => new UserRegistrationResult(false, null, error);
        }
    }
}
