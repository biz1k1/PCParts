namespace PCParts.Storage.Common.Extensions;

public class AsyncSignal: IDisposable
{
    private readonly SemaphoreSlim _signal = new(0, 1);
    private bool _disposed;

    public Task WaitAsync(CancellationToken ct) => _signal.WaitAsync(ct);

    public void Set()
    {
        if (_signal.CurrentCount == 0)
            _signal.Release();
    }
    public void Dispose()
    {
        if (!_disposed)
        {
            _signal.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
