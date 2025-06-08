namespace PCParts.Application.Abstraction.Storage;

public interface IUnitOfWork
{
    public Task<IUnitOfWorkTransaction> StartScope(CancellationToken cancellationToken);
}

public interface IUnitOfWorkTransaction : IAsyncDisposable
{
    TStorage GetStorage<TStorage>() where TStorage : IStorage;
    Task Commit(CancellationToken cancellation);
}

/// <summary>
///     Заглушка, позволяющая вынимать нужные зависимости из нового scope
/// </summary>
public interface IStorage
{
}