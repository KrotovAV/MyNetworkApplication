namespace MainMailApiMultiSwagger.Dto
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public UserRole Role { get; set; }
        public RoleId RoleId { get; set; }
    }
}
