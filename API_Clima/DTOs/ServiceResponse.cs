using System.Dynamic;
using System.Net;

namespace API_Clima.DTOs
{
    public class ServiceResponse<T>
    {
        public HttpStatusCode CodigoHttp { get; set; }
        public T? Dados { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public ExpandoObject? ErroRetorno { get; set; }
    }
}
