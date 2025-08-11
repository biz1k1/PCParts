using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PCParts.Application.Abstraction.Storage;

namespace PCParts.Storage.Storages;

public class UnitOfWork(IServiceScopeFactory scopeFactory) : IUnitOfWork
{
    public async Task<IUnitOfWorkTransaction> StartScope(CancellationToken cancellationToken)
    {
        var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PgContext>();
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new UnitOfWorkTransaction(scope, transaction);
    }
}

internal sealed class UnitOfWorkTransaction(
    IServiceScope scope,
    IDbContextTransaction transaction) : IUnitOfWorkTransaction
{
    public async Task Commit(CancellationToken cancellationToken)
    {
        await transaction.CommitAsync(cancellationToken);
    }

    public TStorage GetStorage<TStorage>() where TStorage : IStorage
    {
        return scope.ServiceProvider.GetRequiredService<TStorage>();
    }

    public async ValueTask DisposeAsync()
    {
        if (scope is IAsyncDisposable scopeAsyncDisposable)
        {
            await scopeAsyncDisposable.DisposeAsync();
        }
        else
        {
            scope.Dispose();
        }

        await transaction.DisposeAsync();
    }
}
