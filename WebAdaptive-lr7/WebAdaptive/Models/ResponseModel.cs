using System.Net;

namespace WebAdaptive.Models
{
    public class ResponseModel<T>
    {
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
}
