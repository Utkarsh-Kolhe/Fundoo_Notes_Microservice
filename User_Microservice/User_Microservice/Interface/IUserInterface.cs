using User_Microservice.Model;

namespace User_Microservice.Interface
{
    public interface IUserInterface
    {
        public bool AddNewUser(UserRegistrationModel model);

        public string UserLogin(UserLoginModel model);
    }
}
