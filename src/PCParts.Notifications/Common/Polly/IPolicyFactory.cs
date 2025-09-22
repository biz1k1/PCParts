using Polly.Wrap;

namespace PCParts.Notifications.Common.Polly
{
    public interface IPolicyFactory
    {
        AsyncPolicyWrap<T> GetPolicy<T>();
    }
}
