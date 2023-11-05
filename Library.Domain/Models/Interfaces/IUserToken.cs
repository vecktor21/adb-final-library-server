using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface IUserToken
    {
        public Guid UserID { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
