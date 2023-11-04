using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface ICart
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        List<ICartBook> Books { get; set; }
        //промежуточная цена (без скидки)
        double CountIntermediatePrice();
        //скидка
        double CountDiscount();
        //Итог
        double CountTotal();
    }

    public interface ICartBook
    {
        IBook Book { get; set; }
        int Count { get; set; }
    }
}
