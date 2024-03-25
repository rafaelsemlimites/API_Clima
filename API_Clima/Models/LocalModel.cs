using System.ComponentModel;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace API_Clima.Models
{
    public class LocalModel
    {
        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("atualizado_em")]
        public DateTime AtualizadoEm { get; set; }

        [JsonPropertyName("clima")]
        public List<ClimaModel> Clima { get; set; }
    }
}
