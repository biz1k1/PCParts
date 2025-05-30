using AutoMapper;
using PCParts.Application.Storages;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Storages;

public class UserStorage : IUserStorage
{
    private readonly IMapper _mapper;
    private readonly PgContext _pgContext;

    public UserStorage(
        PgContext pgContext,
        IMapper mapper)
    {
        _pgContext = pgContext;
        _mapper = mapper;
    }

    public async Task<User?> CreateUser(Guid id, string phone, string phoneConfirmed,
        string passwordHash, DateTimeOffset createdAt, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = id,
            Phone = phone,
            PhoneConfirmed = false,
            PasswordHash = passwordHash,
            CreatedAt = createdAt
        };

        _pgContext.Users.Add(user);
        await _pgContext.SaveChangesAsync(cancellationToken);
        return await _pgContext.Users.FindAsync(user.Id);
    }
}