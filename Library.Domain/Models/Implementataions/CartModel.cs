using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Implementataions
{
    public class CartModel : ICart
    {
        private Guid _id;
        public CartModel()
        {

        }

        public Guid Id
        {
            get
            {
                if (Guid.Empty == _id || _id == null)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set { _id = value; }
        }
        public Guid UserId { get; set; }
        public List<ICartBook> Books { get; set; } = new();
        public double CountIntermediatePrice()
        {
            double price = 0;
            foreach (var cartBook in Books)
            {
                price += cartBook.Book.Price * cartBook.Count;
            }
            return price;
        }
        public double CountDiscount()
        {
            double discount = 0;
            foreach (var cartBook in Books)
            {
                discount += cartBook.Book.Discount * cartBook.Count;
            }
            return discount;
        }
        public double CountTotal()
        {   
            return this.CountIntermediatePrice() - this.CountDiscount();
        }
    }

    public class CartBookModel : ICartBook
    {
        public IBook Book { get; set; }
        public int Count { get; set; }
    }
}
