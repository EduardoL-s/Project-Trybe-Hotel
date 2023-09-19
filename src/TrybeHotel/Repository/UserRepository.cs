using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var userToLogin = from user in _context.Users
                            where user.Email == login.Email
                            && user.Password == login.Password
                            select new UserDto {
                                UserId = user.UserId,
                                Name = user.Name,
                                Email = user.Email,
                                UserType = user.UserType
                            };
            
            if (userToLogin.Count() == 0) {
                return null!;
            }

            return userToLogin.First();
        }
        public UserDto Add(UserDtoInsert user)
        {
            var newUser = new User {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new UserDto {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var userFound = from user in _context.Users
                            where user.Email == userEmail
                            select new UserDto {
                                UserId = user.UserId,
                                Name = user.Name,
                                Email = user.Email,
                                UserType = user.UserType
                            };
            
            if (userFound.Count() == 0) {
                return null!;
            }

            return userFound.First();
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = from user in _context.Users
                        select new UserDto {
                            UserId = user.UserId,
                            Name = user.Name,
                            Email = user.Email,
                            UserType = user.UserType
                        };

            return users;
        }

    }
}