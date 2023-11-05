using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Email { get; set; }
        string Role { get; set; }
        int Age { get; set; }
        string PhoneNumber { get; set; }
        string Password { get; set; }
        string Description { get; set; }
        DateTime RegisterDate { get; set; }
        List<IBook> BooksViewHistory { get; set; }
    }
}
