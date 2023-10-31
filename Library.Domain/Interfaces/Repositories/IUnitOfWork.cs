using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;set;}
        IBookRepository BookRepository { get; set; }
        ICartRepository CartRepository { get; set; }
    }
}
