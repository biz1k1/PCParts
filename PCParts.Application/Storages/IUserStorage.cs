using PCParts.Domain.Entities;

namespace PCParts.Application.Storages;

public interface IUserStorage
{
    public Task<User> CreateUser(Guid id, string phone, string phoneConfirmed,
        string passwordHash, DateTimeOffset createdAt, CancellationToken cancellationToken);
}