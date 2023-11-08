using System.Net;

namespace Library.Domain.Models
{
    public class ExceptionResultDto
    {
        public int Code { get; set; }

        public string? Field { get; set; }

        public string Message { get; set; } = "";
    }
}
