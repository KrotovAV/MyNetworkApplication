namespace MainMailApiMultiSwagger.Dto
{
    public class Role
    {
        public RoleId RoleId { get; set; }
        public string Name { get; set; }
        public virtual List<UserModel> Users { get; set; }
    }
}