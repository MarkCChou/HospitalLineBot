using HospitalLineBot.Data;
using HospitalLineBot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HospitalLineBot.Repositories
{

    public class SQLAppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalLineBotDbContext _dbContext;

        public SQLAppointmentRepository(HospitalLineBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _dbContext.Appointments.Include("User").ToListAsync();
        }

        public async Task<List<Appointment?>> GetByIdAsync(string id)
        {
            return await _dbContext.Appointments.Include("User").Where(x => x.UserId == id).ToListAsync();
        }
    }
}
