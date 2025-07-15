using PCParts.Domain.Entities;

namespace PCParts.Application.Abstraction.Authentication
{
    public interface IJwtTokenProvider
    {
        string Create(User user);
    }
}
