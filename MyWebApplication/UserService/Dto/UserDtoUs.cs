using UserService.AuthorizationModel;
using UserService.BD;

namespace UserService.Dto
{
    public class UserDtoUs
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public RoleId RoleId { get; set; }
    }
}