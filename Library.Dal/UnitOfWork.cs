using Library.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public IBookRepository BookRepository { get; set; }
        public ICartRepository CartRepository { get; set; }
        public UnitOfWork(IBookRepository bookRepository, IUserRepository userRepository, ICartRepository cartRepository
            )
        {
            BookRepository = bookRepository;
            UserRepository = userRepository;
            CartRepository = cartRepository;
        }
    }
}
