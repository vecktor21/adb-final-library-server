using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Test.Implenetations
{
    internal class UserRepositoryMock : IUserRepository
    {
        Dictionary<int,IUser> users;
        public UserRepositoryMock()
        {
            users = new Dictionary<int, IUser>();
        }
        public bool DeleteUser(int id)
        {
            users.Remove(id);
            return true;
        }

        public Task<bool> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public IUser FindUser(int id)
        {
            return users[id];
        }

        public Task<IUser?> FindUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<IUser> GetUsers()
        {
            return users.Select(x=>x.Value).ToList();
        }

        public bool UpdateUser(IUser user)
        {
            throw new NotImplementedException();
        }

        Task<List<IUser>> IUserRepository.GetUsers()
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserRepository.UpdateUser(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
