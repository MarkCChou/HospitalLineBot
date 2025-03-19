using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class ImageMessageDto : BaseMessageDtos
    {
        public ImageMessageDto()
        {
            Type = MessageTypeEnum.Image;
        }
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}
