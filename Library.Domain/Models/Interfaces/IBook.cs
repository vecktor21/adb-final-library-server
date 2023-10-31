using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface IBook : IModel
    {
        string Title { get; set; }
        string PiblishCity { get; set; }
        string Publisher { get; set; }
        int Pages { get; set; }
        string CoverType { get; set; }
        double Price { get; set; }
        int Year { get; set; }
        double Discount { get; set; }
        string Genre { get; set; }
        Guid AuthorId { get; set; }
        IUser Author { get; }
        List<int> Likes { get; set; }
    }
}
