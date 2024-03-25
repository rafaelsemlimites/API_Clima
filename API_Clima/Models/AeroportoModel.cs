using System.ComponentModel;
using System.Text.Json.Serialization;

namespace API_Clima.Models
{
    public class AeroportoModel
    {
        [JsonPropertyName("umidade")]
        public int Umidade { get; set; }

        [JsonPropertyName("visibilidade")]
        public string Visibilidade { get; set; }

        [JsonPropertyName("codigo_icao")]
        public string CodigoIcao { get; set; }

        [JsonPropertyName("pressao_atmosferica")]
        public int PressaoAtmosferica { get; set; }

        [JsonPropertyName("vento")]
        public int Vento { get; set; }

        [JsonPropertyName("direcao_vento")]
        public int DirecaoVento { get; set; }

        [JsonPropertyName("condicao")]
        public string Condicao { get; set; }

        [JsonPropertyName("condicao_desc")]
        public string CondicaoDesc { get; set; }

        [JsonPropertyName("temp")]
        public int Temp { get; set; }

        [JsonPropertyName("atualizado_em")]
        public DateTime AtualizadoEm { get; set; }
    }
}
