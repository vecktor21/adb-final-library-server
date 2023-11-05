using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface IBook
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string PiblishCity { get; set; }
        string Publisher { get; set; }
        int Pages { get; set; }
        string CoverType { get; set; }
        double Price { get; set; }
        int Year { get; set; }
        double Discount { get; set; }
        string Genre { get; set; }
        string Description { get; set; }
        string Author { get; set; }
        public List<IFileModel> Images { get; set; }
        List<Guid> Likes { get; set; }
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
