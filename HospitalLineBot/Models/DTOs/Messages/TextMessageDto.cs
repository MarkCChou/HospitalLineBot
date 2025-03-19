using HospitalLineBot.Enum;

namespace HospitalLineBot.Models.DTOs.Messages
{
    public class TextMessageDto : BaseMessageDtos
    {
        public TextMessageDto()
        {
            Type = MessageTypeEnum.Text;
        }
        public string Text { get; set; }

        public List<TextMassageEmojiDto>? Emojis { get; set; }

        //新增emoji class
        public class TextMassageEmojiDto
        {
            public int Index { get; set; }
            public string ProductId { get; set; }
            public string EmojiId { get; set; }


        }
    }
}
