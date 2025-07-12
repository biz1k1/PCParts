using System.Data.Common;

namespace PCParts.Storage.Common.Services.DbConnectionProvider
{
    public interface IDbConnectionProvider<TConnection> 
        where TConnection : DbConnection
    {
        Task<TConnection> GetOpenConnection(bool forListenOnly);
    }
}
