using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class VideoMessageDto : BaseMessageDtos
    {
        public VideoMessageDto()
        {
            Type = MessageTypeEnum.Video;
        }
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
        public string? TrackingId { get; set; }

    }
}
