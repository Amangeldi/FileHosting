using FileHosting.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileHosting.BLL.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(UserRegistrationDTO UserRegistrationDTO);
        Task<string> GetUserName(string id);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task DeleteUser(string id);
        Task<string> GetUserId(string currentUserName);
    }
}
