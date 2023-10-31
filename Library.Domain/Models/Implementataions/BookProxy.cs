using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Implementataions
{
    public class BookProxy : IBook
    {
        private Guid _id;

        private string _title = "";
        private string _city = "";
        private string _publisher = "";
        private int _pages = 0;
        private string _coverType = "";
        private double _price = 0;
        private int _year = 0;
        private double _discount = 0;
        private string _genre = "";
        private Guid _authorId;
        private IUser _author;
        
        private readonly IUnitOfWork _db;
        public BookProxy(IUnitOfWork db)
        {
            this._db = db;
            _author = _db.UserRepository.FindUser(_authorId).Result;
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
        public string Title { get { return _title; } set { _title = value; } }
        public string PiblishCity { get { return _city; } set { _city = value; } }
        public string Publisher { get { return _publisher; } set { _publisher = value; } }
        public int Pages { get { return _pages; } set { _pages = value; } }
        public string CoverType { get { return _coverType; } set { _coverType = value; } }
        public double Price { get { return _price; } set { _price = value; } }
        public int Year { get { return _year; } set { _year = value; } }
        public double Discount { get { return _discount; } set { _discount = value; } }
        public string Genre { get { return _genre; } set { _genre = value; } }
        public Guid AuthorId { get { return _authorId; } set { _authorId = value; } }
        public IUser Author 
        { 
            get 
            {
                if(_author == null)
                {
                    _author = _db.UserRepository.FindUser(_authorId).Result;
                }
                return _author;
                
            }
        }
        public List<int> Likes { get; set; }
    }
}
