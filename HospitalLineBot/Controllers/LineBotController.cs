using HospitalLineBot.Models.DTOs.Webhook;
using HospitalLineBot.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HospitalLineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
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
