using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class ImageMessageDto : BaseMessageDtos
    {

        //must be png or jpeg
        public ImageMessageDto()
        {
            Type = MessageTypeEnum.Image;
        }
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}
