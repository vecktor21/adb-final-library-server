using Library.Domain.Dtos.Book;
using Library.Domain.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Dtos.Cart
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<CartBookViewModel> Books { get; set; } = new List<CartBookViewModel>();
        public double IntermediatePrice { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
    }

    public class CartBookViewModel
    {
        public BookViewModel Book { get; set; }
        public int Count { get; set; }
    }
}
