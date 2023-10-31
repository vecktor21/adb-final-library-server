using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IUser?> FindUser(Guid id);
        Task<List<IUser>> GetUsers();
        Task<bool> UpdateUser(IUser user);
        Task<bool> DeleteUser(Guid id);
    }
}
