using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Models
{
    internal class BookEntity : IBook
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PiblishCity { get; set; }
        public string Publisher { get; set; }
        public int Pages { get; set; }
        public string CoverType { get; set; }
        public double Price { get; set; }
        public int Year { get; set; }
        public double Discount { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public IUser Author { get; set; }
        public List<Guid> Likes { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
