namespace PCParts.Storage.Common.Extensions
{
    public class AsyncSignal
    {
        private readonly SemaphoreSlim _signal = new(0, 1);

        public Task WaitAsync(CancellationToken ct) => _signal.WaitAsync(ct);

        public void Set()
        {
            if (_signal.CurrentCount == 0)
                _signal.Release();
        }
    }
}
