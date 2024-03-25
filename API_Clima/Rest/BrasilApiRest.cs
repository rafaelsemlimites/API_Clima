using API_Clima.DTOs;
using API_Clima.Interfaces;
using API_Clima.Logs;
using API_Clima.Models;
using Dapper;
using System;
using System.Data.SqlClient;
using System.Dynamic;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace API_Clima.Rest
{
    public class BrasilApiRest : IBrasilApi
    {
        private readonly string _connectionString;

        public BrasilApiRest(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ServiceResponse<LocalModel>> GetClimaLocal(int cityCode)
        {
            Logger.Log($"Iniciando busca de clima local para cityCode: {cityCode}", "INFO");

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://brasilapi.com.br/api/cptec/v1/clima/previsao/{cityCode}");
        
            var response = new ServiceResponse<LocalModel>();

            using (var client = new HttpClient())
            {
                var responseBrasilApi = await client.SendAsync(request);
                var contentResp = await responseBrasilApi.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<LocalModel>(contentResp);

                if (responseBrasilApi.IsSuccessStatusCode)
                {

                    var parameters = new
                    {
                        Cidade = objResponse.Cidade,
                        Estado = objResponse.Estado,
                        AtualizadoEm = objResponse.AtualizadoEm,
                        DataPesquisa = DateTime.Now.ToLocalTime(),

                        Data = objResponse.Clima[0].Data,
                        Condicao = objResponse.Clima[0].Condicao,
                        CondicaoDesc = objResponse.Clima[0].CondicaoDesc,
                        Min = objResponse.Clima[0].Min,
                        Max = objResponse.Clima[0].Max,
                        IndiceUv = objResponse.Clima[0].IndiceUv,
                    };

                    Logger.Log($"Dados recebidos da API Brasil para cityCode: {cityCode}", "INFO");

                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        await sqlConnection.OpenAsync();

                        // Inserção na tabela Locais
                        const string insertLocalSql = @"
                        INSERT INTO Locais (Cidade, Estado, AtualizadoEm, DataPesquisa)
                        OUTPUT INSERTED.LocalId
                        VALUES (@Cidade, @Estado, @AtualizadoEm, @DataPesquisa)";

                        // Executa a inserção e obtém o ID gerado para o local
                        var localId = await sqlConnection.ExecuteScalarAsync<int>(insertLocalSql, new
                        {
                            objResponse.Cidade,
                            objResponse.Estado,
                            objResponse.AtualizadoEm,
                            DataPesquisa = DateTime.Now.ToLocalTime()
                        });

                        // Inserção na tabela Clima
                        const string insertClimaSql = @"
                        INSERT INTO Clima (LocalId, Data, Condicao, CondicaoDesc, Min, Max, IndiceUv)
                        VALUES (@LocalId, @Data, @Condicao, @CondicaoDesc, @Min, @Max, @IndiceUv)";

                        // Para cada registro de clima, inserir na tabela Clima
                        foreach (var clima in objResponse.Clima)
                        {
                            await sqlConnection.ExecuteAsync(insertClimaSql, new
                            {
                                LocalId = localId,
                                Data = clima.Data,
                                Condicao = clima.Condicao,
                                CondicaoDesc = clima.CondicaoDesc,
                                Min = clima.Min,
                                Max = clima.Max,
                                IndiceUv = clima.IndiceUv
                            });
                        }
                    }

                    Logger.Log($"Dados climáticos inseridos com sucesso no banco para cityCode: {cityCode}", "INFO");

                    response.CodigoHttp = responseBrasilApi.StatusCode;
                    response.Dados = objResponse;
                }
                else
                {
                    Logger.Log($"Falha ao buscar dados climáticos para cityCode: {cityCode}, StatusCode: {responseBrasilApi.StatusCode}", "ERROR");

                    response.CodigoHttp = responseBrasilApi.StatusCode;
                    response.ErroRetorno = JsonSerializer.Deserialize<ExpandoObject>(contentResp);
                }
            }

            return response;
        
        
        }
        public async Task<ServiceResponse<AeroportoModel>> GetClimaAeroporto(string icaoCode)
        {
            Logger.Log($"Iniciando busca de clima para o Aeroporto icaoCode: {icaoCode}", "INFO");

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://brasilapi.com.br/api/cptec/v1/clima/aeroporto/{icaoCode}");

            var response = new ServiceResponse<AeroportoModel>();

            using (var client = new HttpClient())
            {
                var responseBrasilApi = await client.SendAsync(request);
                var contentResp = await responseBrasilApi.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<AeroportoModel>(contentResp);

                if (responseBrasilApi.IsSuccessStatusCode)
                {
                    var parameters = new
                    {
                        CodigoIcao = objResponse.CodigoIcao,
                        Umidade = objResponse.Umidade,
                        AtualizadoEm = objResponse.AtualizadoEm,
                        PressaoAtmosferica = objResponse.PressaoAtmosferica,
                        Visibilidade = objResponse.Visibilidade,
                        Vento = objResponse.Vento,
                        DirecaoVento = objResponse.DirecaoVento,
                        Condicao = objResponse.Condicao,
                        CondicaoDesc = objResponse.CondicaoDesc,
                        Temp = objResponse.Temp,
                        DataPesquisa = DateTime.Now.ToLocalTime()
                    };

                    Logger.Log($"Dados recebidos da API Brasil para o Aeroporto icaoCode: {icaoCode}", "INFO");

                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        await sqlConnection.OpenAsync(); // Garante que a conexão esteja aberta para executar a query

                        const string sql = @"
                        INSERT INTO ClimaAeroportos
                        (CodigoIcao, Umidade, AtualizadoEm, PressaoAtmosferica, Visibilidade, Vento, DirecaoVento, Condicao, CondicaoDesc, Temp, DataPesquisa)
                        OUTPUT INSERTED.Id
                        VALUES
                        (@CodigoIcao, @Umidade, @AtualizadoEm, @PressaoAtmosferica, @Visibilidade, @Vento, @DirecaoVento, @Condicao, @CondicaoDesc, @Temp, @DataPesquisa)";

                        // Executa a inserção e obtém o ID gerado
                        var insertedId = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                        Logger.Log($"Dados climáticos do Aeroporto inseridos com sucesso no banco para icaoCode: {icaoCode}", "INFO");

                        response.CodigoHttp = responseBrasilApi.StatusCode;
                        response.Dados = objResponse;
                    }
                }
                else
                {
                    Logger.Log($"Falha ao buscar dados climáticos para icaoCode: {icaoCode}, StatusCode: {responseBrasilApi.StatusCode}", "ERROR");
                    
                    response.CodigoHttp = responseBrasilApi.StatusCode;
                    response.CodigoHttp = responseBrasilApi.StatusCode;
                    response.ErroRetorno = JsonSerializer.Deserialize<ExpandoObject>(contentResp);
                }
            }

            return response;
        }

    }
}
