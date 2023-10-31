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

        public IUser FindUser(int id)
        {
            return users[id];
        }

        public List<IUser> GetUsers()
        {
            return users.Select(x=>x.Value).ToList();
        }

        public bool UpdateUser(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
