using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class AudioMessageDto : BaseMessageDtos
    {
        public AudioMessageDto()
        {
            Type = MessageTypeEnum.Audio;
        }

        public string OriginalContentUrl { get; set; }
        public int Duration { get; set; }
    }
}
