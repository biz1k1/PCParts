using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Storage.Common.Services.Deduplication
{
    public interface IDeduplicationService
    {
        bool IsDuplicate(string messageId);
    }
}
