using PCParts.Application.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Services.PendingUserService
{
    public interface IPendingUserService
    {
        Task<PendingUser> CreatePendingUser(CreatePendingUserCommand command, CancellationToken cancellationToken );
    }
}
