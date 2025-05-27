using HospitalLineBot.Models.Domain;
using HospitalLineBot.Models.DTOs.Webhook;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HospitalLineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        // Messaging api channel accessToken & secret

        private readonly string channelAccessToken =
            "uuWiZ/6D9pCpTPNDjxOicp2Z6KVTblplyc/W59vMjhPkozoNRWxajWvkpwYBlr680JAuTkzMNXFiqr2FBKBWkX8uoLGbB9/oim1H5pDrdVb17ZPlhpQktQxdg1mcYAVF/zcScGNq3We9n76TNTLlFgdB04t89/1O/w1cDnyilFU=";

        private readonly string channelsecret = "0841fbb9777c8b6c6c55b8341ce95f13";

        // 宣告 service
        private readonly LineBotService _lineBotService;

        public LineBotController(LineBotService lineBotService)
        {
            _lineBotService = lineBotService;

        }


        [HttpPost("webhook")]

        public async Task<IActionResult> Webhook(WebhookRequestBodyDto body)
        {
            await _lineBotService.ReceiveWebhook(body);
            return Ok();

        }

        [HttpPost("SendMessage/Broadcast")]
        public IActionResult Broadcast([Required] string messageType, object body)
        {
            _lineBotService.BroadcastMessageHandler(messageType, body);

            return Ok();
        }
    }
}
