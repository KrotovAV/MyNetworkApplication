
using DataBaseUsers.BD;
namespace DataBaseUsers.Repository;


public interface IUserRepository {
    public void UserAdd(string name, string password, RoleId roleId);
    public RoleId UserCheck(string name, string password);
    public bool UserExists(string name);
    public List<User> GetAllUsers();
    public void DeleteUser(string name);
}