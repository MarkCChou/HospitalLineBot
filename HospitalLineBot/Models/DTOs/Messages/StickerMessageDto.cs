using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class StickerMessageDto : BaseMessageDtos
    {
        public StickerMessageDto()
        {
            Type = MessageTypeEnum.Sticker;
        }

        public string PackageId { get; set; }
        public string StickerId { get; set; }
    }
}
