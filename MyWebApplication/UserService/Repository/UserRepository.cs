using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DataBaseUsers;
using DataBaseUsers.BD;
using Microsoft.EntityFrameworkCore;
using UserService.Dto;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private IMapper _mapper;

        public UserRepository(IMapper mapper) 
        {
            _mapper = mapper;
        }
        public void UserAdd(string name, string password, RoleId roleId)
        {
            using (var context = new UserContext())
            {
                if (roleId == RoleId.Admin)
                {
                    var c = context.Users.Count(x => x.RoleId == RoleId.Admin);

                    if (c > 0)
                    {
                        throw new Exception("Administrator already exist");
                    }
                }

                var user = new User();
                user.Name = name;
                user.RoleId = roleId;

                user.Salt = new byte[16];
                new Random().NextBytes(user.Salt);
                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                user.Password = shaM.ComputeHash(data);
                context.Add(user);
                context.SaveChanges();
            }
        }

        public RoleId UserCheck(string name, string password)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == name);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                var bpassword = shaM.ComputeHash(data);

                if (user.Password.SequenceEqual(bpassword))
                {
                    return user.RoleId;
                }
                else
                {
                    throw new Exception("Wrong password");
                }
            }
        }

        public List<UserDtoUs> GetAllUsers()
        {
            using (var context = new UserContext())
            {
                var users = context.Users.Select(_mapper.Map<UserDtoUs>)
                    .Where(x => x.RoleId == RoleId.User || x.RoleId == RoleId.Adminhelper)
                    .ToList();
                return users;

            }
        }

        public bool UserExists(string name)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == name);

                if (user != null)
                {
                    return true;
                }
                throw new Exception("User not found");
            }
        }

        public string GetUserRole(string name)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == name);

                if (user != null)
                {
                    string role = user.RoleId.ToString();
                    return role;
                }
                throw new Exception("User not found");
            }
        }
        
        public string ChangeUserRole(string name)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == name);
                if (user != null)
                {
                    string oldRole = user.RoleId.ToString();
                    if (user.RoleId == RoleId.Adminhelper || user.RoleId == RoleId.User) 
                    {
                        if (user.RoleId == RoleId.Adminhelper) 
                            user.RoleId = RoleId.User;
                        else 
                            user.RoleId = RoleId.Adminhelper;

                        context.SaveChanges();
                        return $"{user.Name} change role from {oldRole} to {user.RoleId}.";
                    }
                    return "Administrator cant change role!";
                }
                throw new Exception("User not found.");
            }
        }
        
        public void DeleteUser(string name)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == name);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (user.RoleId == RoleId.Admin)
                {
                    throw new Exception("Admin cant del himself");
                }

                try
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}