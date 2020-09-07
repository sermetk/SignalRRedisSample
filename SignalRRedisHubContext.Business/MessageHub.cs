using SignalRRedisHubContext.Common.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalRRedisHubContext.BusinessEngine
{
    public class MessageHub : Hub
    {
        private readonly IDistributedCache distributedCache;
        private readonly IAccessBusinessEngine accessBusinessEngine;
        private readonly ILogger<MessageHub> logger;
        public MessageHub(IDistributedCache distributedCache, IAccessBusinessEngine accessBusinessEngine, ILogger<MessageHub> logger)
        {
            this.distributedCache = distributedCache;
            this.accessBusinessEngine = accessBusinessEngine;
            this.logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            if (accessBusinessEngine.IsAuthenticated)
            { 
                await distributedCache.SetStringAsync(accessBusinessEngine.User, Context.ConnectionId);
                logger.LogInformation($"Connected: ConnectionId: {Context.ConnectionId}");
            }
            else
            {
                Context.Abort();
                logger.LogWarning($"UnauthorizedAccess {Context.ConnectionId}");
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (accessBusinessEngine.IsAuthenticated)
            { 
                await distributedCache.RemoveAsync(accessBusinessEngine.User);
                logger.LogInformation($"Connected: ConnectionId: {accessBusinessEngine.User}");
            }
            else
            {
                logger.LogWarning($"User cannot be found: {accessBusinessEngine.User}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
