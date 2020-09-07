using SignalRRedisHubContext.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System.Linq;


namespace SignalRRedisHubContext.API.Accessor
{
    public class RequestBusinessEngine : IRequestBusinessEngine
    {
        public string Token
        {
            get
            {
                if (string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["Token"]))
                    return string.Empty;
                return httpContextAccessor.HttpContext.Request.Headers["Token"].First();
            }
        }

        private readonly IHttpContextAccessor httpContextAccessor;
        public RequestBusinessEngine(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}
