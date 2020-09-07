using System.Threading.Tasks;

namespace SignalRRedisHubContext.Common.Contracts
{
    public interface IMessageEngine
    {
        Task<string> SendSignalRMessage(string userId);
    }
}
