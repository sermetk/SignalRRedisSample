
namespace SignalRRedisHubContext.Common.Contracts
{
    public interface IAccessBusinessEngine
    {
        bool IsAuthenticated { get; }
        string User { get; }
    }
}
