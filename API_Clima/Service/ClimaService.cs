using API_Clima.DTOs;
using API_Clima.Interfaces;
using API_Clima.Models;
using AutoMapper;

namespace API_Clima.Service.ClimaService
{
    public class ClimaService : IClimaService
    {
        private readonly IMapper _mapper;
        private readonly IBrasilApi _brasilApi;

        public ClimaService(IMapper mapper, IBrasilApi brasilApi)
        {
            _mapper = mapper; 
            _brasilApi = brasilApi;
        }


        public async Task<ServiceResponse<LocalModel>> GetClimaLocal(int cityCode)
        {
            var climaLocal = await _brasilApi.GetClimaLocal(cityCode);

            return _mapper.Map<ServiceResponse<LocalModel>>(climaLocal);
        }


        public async Task<ServiceResponse<AeroportoModel>> GetClimaAeroporto(string icaoCode)
        {
            var climaAeroporto = await _brasilApi.GetClimaAeroporto(icaoCode);

            return _mapper.Map<ServiceResponse<AeroportoModel>>(climaAeroporto);
        }
    }
}
