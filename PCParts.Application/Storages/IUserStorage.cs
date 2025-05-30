using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCParts.Domain.Entities;


namespace PCParts.Application.Storages
{
    public interface IUserStorage
    {
        public Task<User> CreateUser(Guid id, string phone, string phoneConfirmed,
            string passwordHash, DateTimeOffset createdAt, CancellationToken cancellationToken);
    }
}
