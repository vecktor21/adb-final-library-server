using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Models
{
    public class UserTokenEntity : IUserToken
    {
        public Guid UserID { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
