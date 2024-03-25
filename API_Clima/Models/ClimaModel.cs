using System.ComponentModel;
using System.Text.Json.Serialization;

namespace API_Clima.Models
{
    public class ClimaModel
    {
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("condicao")]
        public string Condicao { get; set; }

        [JsonPropertyName("condicao_desc")]
        public string CondicaoDesc { get; set; }

        [JsonPropertyName("min")]
        public int Min { get; set; }

        [JsonPropertyName("max")]
        public int Max { get; set; }

        [JsonPropertyName("indice_uv")]
        public int IndiceUv { get; set; }
    }
}
