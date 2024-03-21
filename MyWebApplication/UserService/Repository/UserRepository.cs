using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService;
using UserService.BD;
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
                //var users = context.Users.Where(x => x.RoleId == RoleId.User || x.RoleId == RoleId.Adminhelper);
                //List<UserDtoUs> usersDtoUs = _mapper.Map<UserDtoUs>(users).ToList();
                //return usersDtoUs;

                //var users = context.Users.Select(_mapper.Map<UserDtoUs>).ToList();
                //return users;

                var users = context.Users.Select(_mapper.Map<UserDtoUs>)
                    .Where(x => x.RoleId == RoleId.User || x.RoleId == RoleId.Adminhelper)
                    .ToList();
                return users;

            }
            //return context.Users.ToList();

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

        public void DeleteUser(string name)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == name);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (user.Name == "admin")
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