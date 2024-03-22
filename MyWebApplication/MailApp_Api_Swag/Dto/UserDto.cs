namespace MainMailApiMultiSwagger.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        //public string RoleId { get; set; }
        public RoleId RoleId { get; set; }

    }
}