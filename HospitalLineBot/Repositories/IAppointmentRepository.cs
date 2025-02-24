using HospitalLineBot.Models.Domain;

namespace HospitalLineBot.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync();

        Task<List<Appointment?>> GetByIdAsync(string id);


    }
}
