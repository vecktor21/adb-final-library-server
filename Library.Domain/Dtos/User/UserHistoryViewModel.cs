using Library.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Dtos.User
{
    public class UserHistoryViewModel
    {
        public Guid UserId { get; set; }
        public List<BookViewModel> Books { get; set; } = new List<BookViewModel>();
    }
}
