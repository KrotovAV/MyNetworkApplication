using DataBaseUsers.BD;
using UserService.Dto;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        public void UserAdd(string name, string password, RoleId roleId);
        public RoleId UserCheck(string name, string password);
        public string GetUserRole(string name);
        public string ChangeUserRole(string name);
        public bool UserExists(string name);
        public List<UserDtoUs> GetAllUsers();
        public void DeleteUser(string name);
    }
}