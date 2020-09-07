using SignalRRedisHubContext.Common.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SignalRRedisHubContext.BusinessEngine
{
    public class MessageEngine : IMessageEngine
    {
        private readonly IHubContext<MessageHub> messageHubContext;
        private readonly IDistributedCache distributedCache;
        public MessageEngine(IHubContext<MessageHub> messageHubContext, IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
            this.messageHubContext = messageHubContext;
        }
        public async Task<string> SendSignalRMessage(string user)
        {
            var connectionId = await distributedCache.GetStringAsync(user);
            if (!string.IsNullOrEmpty(connectionId))
            { 
                await messageHubContext.Clients.Client(connectionId).SendAsync("SendMessage");
                return $"Message sended to:{user}";
            }
            else
            {
                return "User cannot be found.";
            }
        }
    }
}