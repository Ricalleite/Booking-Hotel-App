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
            var userLogin = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
           if (userLogin == null) 
           {
            return null;
           }
           return new UserDto
           {
                UserId = userLogin.UserId,
                Name = userLogin.Name,
                Email = userLogin.Email,
                UserType = userLogin.UserType 
           };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var newUser = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = user.Email.Contains("admin") ? "admin" : "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var userAdd = new UserDto
                          {
                            UserId = newUser.UserId,
                            Name = newUser.Name,
                            Email = newUser.Email,
                            UserType = newUser.UserType
                          };
            return userAdd;
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var userByEmail = _context.Users.FirstOrDefault(userByEmail => userByEmail.Email == userEmail);

            if (userByEmail != null)
            {
                return new UserDto
                    {
                        UserId = userByEmail.UserId,
                        Name = userByEmail.Name,
                        Email = userByEmail.Email,
                        UserType = userByEmail.UserType,
                    };
            } return null;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users;

           var getAllUsers = from user in users
           select new UserDto
           {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            UserType = user.UserType,
           };

           if (getAllUsers.Count() < 0 ) 
           {
            return null;
           }
           return getAllUsers;
        }
    }
}