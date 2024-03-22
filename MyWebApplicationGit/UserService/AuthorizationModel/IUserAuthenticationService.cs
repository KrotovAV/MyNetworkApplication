namespace UserService.AuthorizationModel
{
    public interface IUserAuthenticationService
    {
        UserDto Authenticate(LoginModel model);
    }
}