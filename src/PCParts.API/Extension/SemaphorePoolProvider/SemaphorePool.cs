using System.Collections.Concurrent;

namespace PCParts.API.Extension.SemaphorePoolProvider
{
    public class SemaphorePool
    {
        private readonly SemaphoreSlim[] _pool;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
        public SemaphorePool(int size)
        {
            if (size <= 0 || (size & size - 1) != 0)
            {
                throw new ArgumentException("Size must be a positive power of two.", nameof(size));
            }
            _pool = new SemaphoreSlim[size];
            for (var i = 0; i < size; i++)
            {
                _pool[i] = new SemaphoreSlim(1, 1);
            }
        }

        private int GetIndex(string key)
        {
            var hash = key.GetHashCode() & int.MaxValue;
            return hash % _pool.Length;
        }

        public SemaphoreSlim GetOrAddSemaphore(string key)
        {
            return _locks.GetOrAdd(key, k => _pool[GetIndex(k)]);
        }
    }
}
