using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface ICart : IModel
    {
        Guid UserId { get; set; }
        IUser User { get; }
        List<IBook> Books { get; set; }
        double IntermediatePrice { get; }
        double Discount { get; }
        double Total { get;}
        
    }
}
