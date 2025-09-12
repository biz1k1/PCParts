using Polly.Wrap;

namespace PCParts.Storage.Common.Polly
{
    public interface IPolicyFactory
    {
        AsyncPolicyWrap<T> GetPolicy<T>();
    }
}
