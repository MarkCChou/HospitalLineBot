using AutoMapper;
using HospitalLineBot.Models.DTOs;
using HospitalLineBot.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalLineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentRepository appointmentRepository, IMapper mapper
        )
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        //GET ALL APPOINTMENT
        //http://localhost:7217api/appointments

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointmentsDomain = await _appointmentRepository.GetAllAsync();

            var appointmentDto = _mapper.Map<List<AppointmentDto>>(appointmentsDomain);

            return Ok(appointmentDto);
        }

        [HttpGet]
        [Route("{id}")]

        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var appointmentsDomain = await _appointmentRepository.GetByIdAsync(id);

            if (appointmentsDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<AppointmentDto>>(appointmentsDomain));
        }
    }
}
