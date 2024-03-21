using DataBaseUsers.BD;
using System.Security.Cryptography;
using System.Text;
using UserService.AuthorizationModel;
using DataBaseUsers;
namespace UserService.AuthorizationModel
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        public UserDto Authenticate(LoginModel model)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == model.Name);
                if (user == null)
                {
                    return null;
                }
                var data = Encoding.ASCII.GetBytes(model.Password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                var bpassword = shaM.ComputeHash(data);

                if (user.Password.SequenceEqual(bpassword))
                {
                    UserDto userModel = new UserDto(){ Password = model.Password, Username = model.Name, Role = UserRole.Adminstrator };
                    if (user.RoleId == (RoleId)1) userModel.Role = UserRole.User;

                    return userModel;
                }
                else
                {
                    throw new Exception("Wrong password");
                }


                //if (model.Name == "admin" && model.Password == "password")
                //{
                //    return new UserModel { Password = model.Password, Username = model.Name, Role = UserRole.Adminstrator };
                //}

                //if (model.Name == "user" && model.Password == "super")
                //{
                //    return new UserModel { Password = model.Password, Username = model.Name, Role = UserRole.User };
                //}

                //return null;
            }
        }
    }
    //public RoleId UserCheck(string name, string password)
    //{
    //    using (var context = new UserContext())
    //    {
    //        var user = context.Users.FirstOrDefault(x => x.Name == name);

    //        if (user == null)
    //        {
    //            throw new Exception("User not found");
    //        }

    //        var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
    //        SHA512 shaM = new SHA512Managed();
    //        var bpassword = shaM.ComputeHash(data);

    //        if (user.Password.SequenceEqual(bpassword))
    //        {
    //            return user.RoleId;
    //        }
    //        else
    //        {
    //            throw new Exception("Wrong password");
    //        }
    //    }
    //}
}