using API_Clima.DTOs;
using API_Clima.Interfaces;
using API_Clima.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_Clima.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClimaController : ControllerBase
    {

        public readonly IClimaService _climaService;
        

        public ClimaController(IClimaService climaService)
        {
            _climaService = climaService;
        }

        /// <summary>
        /// Obtém as condições climáticas para uma cidade específica.
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna as condições climáticas atuais para a cidade especificada pelo código da cidade.
        /// </remarks>
        /// <param name="cityCode">O código da cidade para a qual as condições climáticas são solicitadas. Deve ser um inteiro positivo.</param>
        /// <response code="200">Retorna as condições climáticas atuais para a cidade especificada.</response>
        /// <response code="400">Código da cidade inválido. Deve ser um inteiro positivo.</response>
        /// <response code="404">Não foram encontradas condições climáticas para o código da cidade fornecido.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("climaLocal/{cityCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AeroportoModel))] // Sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código cityCode inválido ou não fornecido
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Não encontrado
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Erro interno do servidor
        [Produces("application/json")]
        public async Task<IActionResult> BuscarClimaLocal([FromRoute] int cityCode)
        {
            try
            {
                var response = await _climaService.GetClimaLocal(cityCode);
                switch (response.CodigoHttp)
                {
                    case HttpStatusCode.OK: // 200
                        return Ok(response.Dados);
                    case HttpStatusCode.BadRequest: // 400
                        return BadRequest("Código da cidade inválido. Deve ser um inteiro positivo.");
                    case HttpStatusCode.NotFound: // 404
                        return NotFound("Não foram encontradas condições climáticas para o código da cidade fornecido.");
                    default: // Qualquer outro código considerado erro
                        return StatusCode((int)response.CodigoHttp, response.ErroRetorno);
                }
            }
            catch (Exception ex)
            {
                // Log da exceção...
                return StatusCode(500, "Erro interno do servidor."); // 500
            }
        }


        /// <summary>
        /// Obtém as condições climáticas para um aeroporto específico.
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna as condições climáticas atuais para o aeroporto especificado pelo código ICAO.
        /// </remarks>
        /// <param name="icaoCode">O código ICAO do aeroporto para o qual as condições climáticas são solicitadas.</param>
        /// <response code="200">Retorna as condições climáticas atuais para o aeroporto especificado.</response>
        /// <response code="400">Código ICAO inválido ou não fornecido.</response>
        /// <response code="404">Não foram encontradas condições climáticas para o código ICAO fornecido.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("climaAeroporto/{icaoCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClimaModel))] // Sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código ICAO inválido ou não fornecido
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Não encontrado
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Erro interno do servidor
        [Produces("application/json")]
        public async Task<IActionResult> BuscarClimaAeroporto([FromRoute] string icaoCode)
        {
            try
            {
                var response = await _climaService.GetClimaAeroporto(icaoCode);
                switch (response.CodigoHttp)
                {
                    case HttpStatusCode.OK:
                        return Ok(response.Dados); // 200
                    case HttpStatusCode.BadRequest: // 400
                        return BadRequest("Código ICAO inválido ou não fornecido.");
                    case HttpStatusCode.NotFound: // 404
                        return NotFound("Não foram encontradas condições climáticas para o código ICAO fornecido.");
                    default: // Para outros códigos de erro
                        return StatusCode((int)response.CodigoHttp, response.ErroRetorno);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno do servidor."); // 500
            }
        }


    }
}
