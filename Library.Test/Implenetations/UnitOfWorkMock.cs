using Library.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Test.Implenetations
{
    internal class UnitOfWorkMock : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public IBookRepository BookRepository { get; set ; }
        public ICartRepository CartRepository { get; set; }
    }
}
