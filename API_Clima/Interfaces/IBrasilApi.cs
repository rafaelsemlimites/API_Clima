using API_Clima.DTOs;
using API_Clima.Models;

namespace API_Clima.Interfaces
{
    public interface IBrasilApi
    {
        Task<ServiceResponse<LocalModel>> GetClimaLocal(int cityCode);
        Task<ServiceResponse<AeroportoModel>> GetClimaAeroporto(string icaoCode);

    }
}
