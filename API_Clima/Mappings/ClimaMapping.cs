using API_Clima.DTOs;
using AutoMapper;

namespace API_Clima.Mappings
{
    public class ClimaMapping : Profile
    {
        public ClimaMapping()
        {
            CreateMap(typeof(ServiceResponse<>), typeof(ServiceResponse<>));

        }
    }
}
