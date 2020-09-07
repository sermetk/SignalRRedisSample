using SignalRRedisHubContext.Common.Contracts;
using System.Security.Authentication;

namespace SignalRRedisHubContext.BusinessEngine
{
    public class AccessBusinessEngine : IAccessBusinessEngine
    {
        public bool IsAuthenticated
        {
            get
            {
                Load();
                return user != null;
            }
        }
        private string user;
        public string User
        {
            get
            {
                Load();
                ThrowIfUnauthenticated();
                return user;
            }
        }
        private void ThrowIfUnauthenticated()
        {
            if (!IsAuthenticated) 
                throw new AuthenticationException("UnauthorizedAccess");
        }
        private bool isLoaded;
        private readonly IRequestBusinessEngine requestBusinessEngine;
        public AccessBusinessEngine(IRequestBusinessEngine requestBusinessEngine)
        {
            this.requestBusinessEngine = requestBusinessEngine;
        }
        private void Load()
        {
            if (isLoaded) 
                return;
            isLoaded = true;
            if (!string.IsNullOrEmpty(requestBusinessEngine.Token))
                user = requestBusinessEngine.Token == "validtoken" ? "user" : null;
        }
    }
}
