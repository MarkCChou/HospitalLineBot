namespace HospitalLineBot.Models.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 上午/下午
        /// </summary>
        public string Session { get; set; }

        public DateTime Date { get; set; }

        public string Clinic { get; set; }

        public string Doctor { get; set; }

        public string Location { get; set; }

        public int Number { get; set; }

        public string UserId { get; set; }






        public UserDto user { get; set; }
    }
}
