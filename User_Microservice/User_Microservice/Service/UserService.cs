using User_Microservice.Context;
using User_Microservice.Entity;
using User_Microservice.Interface;
using User_Microservice.Model;
using User_Microservice.Hashing;

namespace User_Microservice.Service
{
    public class UserService : IUserInterface
    {
        private readonly UserContext _context;
        private readonly HashingPassword _password;

        public UserService(UserContext context, HashingPassword password)
        {
            _context = context;
            _password = password;
        }

        public bool AddNewUser(UserRegistrationModel model)
        {
            var user = _context.User_Details.FirstOrDefault(e => e.Email == model.Email);
            if (user == null)
            {
                UserEntity userEntity = new UserEntity();

                userEntity.Email = model.Email;
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Password = _password.HashPassword(model.Password);
                _context.User_Details.Add(userEntity);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
