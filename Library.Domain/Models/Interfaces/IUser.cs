using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface IUser : IModel
    {
        string Name { get; set; }
        string Surname { get; set; }
        string Email { get; set; }
        int Age { get; set; }
        string PhoneNumber { get; set; }
        string Password { get; set; }
        string Description { get; set; }
        DateTime RegisterDate { get; set; }
        List<IBook> BooksViewHistory { get; set; }
    }
}
