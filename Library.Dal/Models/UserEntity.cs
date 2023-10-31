using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Models
{
    public class UserEntity : IUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public List<IBook> BooksViewHistory { get; set; } = new List<IBook>();
    }
}
