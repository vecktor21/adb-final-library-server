using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Models
{
    public class CartEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<CartBookEntity> Books { get; set; } = new List<CartBookEntity>();
    }

    public class CartBookEntity
    {
        public Guid BookId { get; set; }
        public int Count { get; set; }
    }
}
