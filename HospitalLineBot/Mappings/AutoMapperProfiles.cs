using AutoMapper;
using HospitalLineBot.Models.Domain;
using HospitalLineBot.Models.DTOs;

namespace HospitalLineBot.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
