using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRRedisHubContext.Common.Contracts;

namespace SignalRRedisHubContext.API.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageEngine messageEngine;
        public MessageController(IMessageEngine messageEngine)
        {
            this.messageEngine = messageEngine;
        }
        [HttpGet]
        [Route("/Message/Send")]
        public async Task<string> SendAsync(string user)
        {
            return await messageEngine.SendSignalRMessage(user);
        }
    }
}
