using HospitalLineBot.Data;
using HospitalLineBot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HospitalLineBot.Repositories
{
    public class SQLAppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalLineBotDbContext _dbcontext;

        public SQLAppointmentRepository(HospitalLineBotDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _dbcontext.Appointments.Include("User").ToListAsync();
        }
    }
}
