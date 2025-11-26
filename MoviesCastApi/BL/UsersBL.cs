using MoviesCastApi.Dal;
using MoviesCastApi.Models;

namespace MoviesCastApi.BL
{
    public class UsersBL
    {
        public static User Register(User user)
        {
            return UsersDal.Register(user);
        }

        public static User? Login(string email, string password)
        {
            return UsersDal.Login(email, password);
        }
    }
}
