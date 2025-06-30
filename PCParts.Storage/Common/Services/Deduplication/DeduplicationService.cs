using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Storage.Common.Services.Deduplication
{
    public class DeduplicationService : IDeduplicationService
    {
        private readonly IMemoryCache _memoryCache;
        public DeduplicationService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string GetMessageId(string cacheKey)
        {
            if (_memoryCache.TryGetValue(cacheKey, out object cachedValue))
            {
                return cachedValue.ToString();
            }

            return String.Empty;
        } 
        public bool IsDuplicate(string messageId)
        {
            if (_memoryCache.TryGetValue(messageId, out _))
                return true;

            _memoryCache.Set(messageId, true, TimeSpan.FromMinutes(5));
            return false;
        }
    }
}
