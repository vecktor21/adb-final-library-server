using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Common.Exceptions
{
    public class ResponseResultException : Exception
    {
        public HttpStatusCode Code { get; }

        public string? Field { get; }

        public string Message { get; set; }

        public ResponseResultException(HttpStatusCode code, string? msg = null, string? field = null)
            : base(msg)
        {
            Code = code;
            Field = field;
            Message = msg;
        }
    }
}
