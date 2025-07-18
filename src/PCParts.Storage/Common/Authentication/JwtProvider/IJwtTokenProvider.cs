using PCParts.Application.Command;
using PCParts.Domain.Entities;

namespace PCParts.Application.Abstraction.Authentication
{
    public interface IJwtTokenProvider
    {
        string Create(Guid id, string email);
    }
}
