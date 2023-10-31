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
    public class CartProxy : ICart
    {
        private Guid _id;
        private Guid _userId;
        private IUser _user;
        private List<IBook> _books;
        

        private readonly IUnitOfWork db;
        public CartProxy(IUnitOfWork db)
        {
            this.db = db;
            _user = db.UserRepository.FindUser(_userId).Result;
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
        public IUser User 
        { 
            get
            {
                return _user;
            } 
        }
        public Guid UserId { get { return _userId; } set { _userId = value; } }
        public List<IBook> Books { get; set; }
        public double IntermediatePrice
        {
            get
            {
                double price = 0;
                foreach (var book in _books)
                {
                    price += book.Price;
                }
                return price;
            }
        }
        public double Discount 
        {
            get
            {
                double discount = 0;
                foreach (var book in _books)
                {
                    discount += book.Discount;
                }
                return discount;
            }
        }
        public double Total
        {
            get
            {
                return this.IntermediatePrice - this.Discount;
            }
        }
    }
}
