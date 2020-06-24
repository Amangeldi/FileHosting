using AutoMapper;
using FileHosting.BLL.DTO;
using FileHosting.BLL.Interfaces;
using FileHosting.DAL.EF;
using FileHosting.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileHosting.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApiContext db;
        public UserService(UserManager<User> userManager, ApiContext context)
        {
            _userManager = userManager;
            db = context;
        }
        public async Task CreateUser(UserRegistrationDTO UserRegistrationDTO)
        {
            if (UserRegistrationDTO.Password == UserRegistrationDTO.ConfirmPassword)
            {
                User user = new User
                {
                    Email = UserRegistrationDTO.Email,
                    UserName = UserRegistrationDTO.Email
                };
                var result = await _userManager.CreateAsync(user, UserRegistrationDTO.Password);
                if (result.Succeeded != true)
                {
                    string errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors = errors + "<div class='alert alert-danger'>" + error.Description + "</div>";
                    }
                    throw new Exception(errors);
                }
            }
            else
            {
                throw new Exception("Пароли не совпадают");
            }
        }

        public async Task DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<string> GetUserId(string currentUserName)
        {
            User user = await _userManager.FindByNameAsync(currentUserName);
            return user.Id;
        }

        public async Task<string> GetUserName(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            return user.UserName;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            IEnumerable<User> model = await db.Users.ToListAsync();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            IEnumerable<UserDTO> DTO = mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(model);
            return DTO;
        }
    }
}
