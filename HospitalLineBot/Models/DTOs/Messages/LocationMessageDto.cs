using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class LocationMessageDto : BaseMessageDtos
    {
        public LocationMessageDto()
        {
            Type = MessageTypeEnum.Location;
        }
        public string Title { get; set; }
        public string Address { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
