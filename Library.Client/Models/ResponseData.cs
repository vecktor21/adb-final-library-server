using Library.Common.Exceptions;
using Library.Domain.Models;

namespace Library.Client.Models
{
    public class ResponseData<TData>
    {
        public TData Data { get; set; }
        public ExceptionResultDto Excetption { get; set; }
    }
}
