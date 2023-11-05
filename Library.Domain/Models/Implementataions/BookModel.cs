using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Implementataions
{
    public class BookModel : IBook
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
        private string _description = "";
        private DateTime _createDate = DateTime.MinValue;
        private DateTime _updateDate = DateTime.MinValue;
        public BookModel()
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
        public string Title { get { return _title; } set { _title = value; } }
        public string PiblishCity { get { return _city; } set { _city = value; } }
        public string Publisher { get { return _publisher; } set { _publisher = value; } }
        public int Pages { get { return _pages; } set { _pages = value; } }
        public string CoverType { get { return _coverType; } set { _coverType = value; } }
        public double Price { get { return _price; } set { _price = value; } }
        public int Year { get { return _year; } set { _year = value; } }
        public double Discount { get { return _discount; } set { _discount = value; } }
        public string Genre { get { return _genre; } set { _genre = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public string Author { get; set; }
        public List<Guid> Likes { get; set; } = new();
        public DateTime CreateDate 
        {
            get
            {
                if(_createDate == DateTime.MinValue)
                {
                    _createDate = DateTime.Now;
                }
                return _createDate;
            }
            set
            {
                _createDate = value;
            }
        }
        public DateTime UpdateDate
        {
            get
            {
                if (_updateDate == DateTime.MinValue)
                {
                    _updateDate = DateTime.Now;
                }
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        public List<IFileModel> Images { get; set; }
    }
}
