using Library.Domain.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Dtos
{
    public class AuthorizationResponseDto
    {
        public UserViewModel User { get; set; }
        public string AccessToken { get; set; }
    }
    public class AccessTokenRequestData
    {
        public string AccessToken { get; set; }
    }
}
