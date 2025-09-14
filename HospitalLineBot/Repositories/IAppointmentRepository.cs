using HospitalLineBot.Models.Domain;
using HospitalLineBot.Models.DTOs;

namespace HospitalLineBot.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync();

        Task<List<Appointment?>> GetByIdAsync(string id);

        Task<List<HospitalRespDto>> GetHospitalAddress(string hospitalName);
    }
}
