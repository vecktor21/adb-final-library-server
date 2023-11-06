using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Models
{
    public class HistoryEntity
    {
        public Guid UserId { get; set; }
        public List<Guid> Books { get; set; } = new List<Guid>();
    }
}
