using HospitalLineBot.Enum;
using HospitalLineBot.Models.DTOs.Messages;
using HospitalLineBot.Models.DTOs.Messages.Request;
using HospitalLineBot.Models.DTOs.Webhook;
using HospitalLineBot.Providers;
using HospitalLineBot.Repositories;
using System.Net.Http.Headers;
using System.Text;

namespace HospitalLineBot.Services
{
    public class LineBotService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        // (將 LineBotController 裡宣告的 ChannelAccessToken & ChannelSecret 移到 LineBotService中)
        // 貼上 messaging api channel 中的 accessToken & secret
        private readonly string _channelAccessToken;

        private readonly string _channelSecret;

        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";
        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest
        private readonly JsonProvider _jsonProvider = new JsonProvider();

        public LineBotService(IAppointmentRepository appointmentRepository, IConfiguration configuration)
        {
            _appointmentRepository = appointmentRepository;
            _channelAccessToken = configuration["LineBot:ChannelAccessToken"];
            _channelSecret = configuration["LineBot:ChannelSecret"];
        }



        public async Task ReceiveWebhook(WebhookRequestBodyDto requestBody)
        {
            foreach (var eventObject in requestBody.Events)
            {
                switch (eventObject.Type)
                {
                    case WebhookEventTypeEnum.Message:
                        string userMessage = eventObject.Message.Text;
                        string replyText;

                        if (userMessage.ToLower().Contains("appointment"))
                        {
                            var replyAppointment = await _appointmentRepository.GetByIdAsync(eventObject.Source.UserId);
                            if (replyAppointment == null || !replyAppointment.Any())
                            {
                                replyText = "沒有您的門診資訊";
                            }
                            else
                            {
                                replyText = string.Join("\n",
                                    replyAppointment.Where(a => a.UserId == eventObject.Source.UserId).Select(a =>
                                        $"日期: {a.Date:yyyy-MM-dd} ({a.Session})\n醫師: {a.Doctor}\n科別: {a.Clinic}\n地點: {a.Location}\n號碼: {a.Number}\n—"
                                    ));
                            }
                        }
                        else
                        {
                            replyText = $"您好，收到您傳來\"{eventObject.Message.Text}\"!";
                        }
                        var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                        {
                            ReplyToken = eventObject.ReplyToken,
                            Messages = new List<TextMessageDto>
                            {
                                new TextMessageDto(){Text = replyText}
                            }
                        };
                        ReplyMessageHandler("text", replyMessage);
                        break;
                    case WebhookEventTypeEnum.Unsend:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}在聊天室收回訊息!");
                        break;
                    case WebhookEventTypeEnum.Follow:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}將我們新增為好友");
                        break;
                    case WebhookEventTypeEnum.Unfollow:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}封鎖了我們");
                        break;
                    case WebhookEventTypeEnum.Join:
                        Console.WriteLine("我們被邀請至聊天室!");
                        break;
                    case WebhookEventTypeEnum.Leave:
                        Console.WriteLine("我們被聊天室踢出!");
                        break;

                    case WebhookEventTypeEnum.MemberJoined:
                        string joinedMemberIds = "";
                        foreach (var member in eventObject.Joined.Members)
                        {
                            joinedMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine($"使用者{joinedMemberIds}加入了群組！");
                        break;
                    case WebhookEventTypeEnum.MemberLeft:
                        string leftMemberIds = "";
                        foreach (var member in eventObject.Left.Members)
                        {
                            leftMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine($"使用者{leftMemberIds}離開了群組！");
                        break;
                    case WebhookEventTypeEnum.Postback:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}觸發了postback事件");
                        break;
                    case WebhookEventTypeEnum.VideoPlayComplete:
                        replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                        {
                            ReplyToken = eventObject.ReplyToken,
                            Messages = new List<TextMessageDto>
                            {
                                new TextMessageDto(){Text = "使用者您好，謝謝您收看官方影片 !"}
                            }
                        };
                        await ReplyMessageHandler("text", replyMessage);
                        break;

                }
            }
        }


        /// Broadcast
        /// <summary>
        /// 接收到廣播請求時，在將請求傳至 Line 前多一層處理，依據收到的 messageType 將 messages 轉換成正確的型別，這樣 Json 轉換時才能正確轉換。
        /// </summary>
        /// <param name="messageType">Text,Sticker,Image,Video,Audio,Location</param>
        /// <param name="requestBody">RequestBody</param>
        public void BroadcastMessageHandler(string messageType, object requestBody)
        {
            string strBody = requestBody.ToString();
            dynamic messageRequest = new BroadcastMessageRequestDto<BaseMessageDtos>();
            switch (messageType)
            {
                case MessageTypeEnum.Text:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<TextMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
                case MessageTypeEnum.Sticker:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<StickerMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
                case MessageTypeEnum.Image:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<ImageMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
                case MessageTypeEnum.Video:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<VideoMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
                case MessageTypeEnum.Audio:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<AudioMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
                case MessageTypeEnum.Location:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<LocationMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;

            }

        }

        /// <summary>
        /// 將廣播訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        public async Task BroadcastMessage<T>(BroadcastMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _channelAccessToken); //帶入 channel access token
            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(broadcastMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        ///ReplyMessages
        /// <summary>
        /// 接收到回覆請求時，在將請求傳至 Line 前多一層處理(目前為預留)
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public async Task ReplyMessageHandler<T>(string messageType, ReplyMessageRequestDto<T> requestBody)
        {
            await ReplyMessage(requestBody);
        }

        /// <summary>
        /// 將回覆訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>    
        public async Task ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _channelAccessToken); //帶入 channel access token
            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
