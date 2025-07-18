using AutoMapper;
using PCParts.Application.Abstraction.Authentication;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.DomainEvents;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.PendingUserService;

public class PendingUserService : IPendingUserService
{
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPendingUserStorage _pendingUserStorage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserStorage _userStorage;
    private readonly IValidationService _validationService;

    public PendingUserService(
        IMapper mapper,
        IUserStorage userStorage,
        IPasswordHasher passwordHasher,
        IValidationService validationService,
        IPendingUserStorage pendingUserStorage,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userStorage = userStorage;
        _passwordHasher = passwordHasher;
        _validationService = validationService;
        _pendingUserStorage = pendingUserStorage;
        _unitOfWork = unitOfWork;
    }

    public async Task<PendingUser> CreatePendingUser(CreatePendingUserCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var existingUser = await _userStorage
            .GetUser(x => x.Email == command.Email, cancellationToken);
        if (existingUser is not null)
        {
            throw new EntityAlreadyExistsException(command.Email);
        }

        var existingpendingUser = await _pendingUserStorage
            .GetPendingUser(x => x.Email == command.Email, cancellationToken);
        if (existingpendingUser is not null)
        {
            throw new PendingUserException();
        }

        await using (var scope = await _unitOfWork.StartScope(cancellationToken))
        {
            var createPendingUserStorage = scope.GetStorage<IPendingUserStorage>();
            var createDomainEventsStorage = scope.GetStorage<IDomainEventsStorage>();

            var hash = _passwordHasher.Hash(command.Password);

            var user = await createPendingUserStorage.CreatePendingUser(command.Email, hash, cancellationToken);
            var userDTO = _mapper.Map<PendingUser>(user);


            await createDomainEventsStorage.AddAsync(
                RegistrationPendingUserEvent.EventCreated(userDTO), cancellationToken);

            await scope.Commit(cancellationToken);

            return userDTO;
        }
    }
}