using HospitalLineBot.Data;
using HospitalLineBot.Models.Domain;
using HospitalLineBot.Models.DTOs;
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

        public async Task<List<HospitalRespDto>> GetHospitalAddress(string hospitalName)
        {
            return await _dbContext.Hospitals
                .Where(x => x.Name.Contains(hospitalName))
                .Select(x => new HospitalRespDto
                {
                    Name = x.Name,
                    Address = x.Address
                })
                .ToListAsync();
        }
    }
}
