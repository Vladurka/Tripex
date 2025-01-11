using Microsoft.EntityFrameworkCore.Storage;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Enums;

namespace Tripex.Core.Domain.Interfaces.Repositories
{
    public interface ICrudRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T entity);
        public IQueryable<T> GetQueryable<T>() where T : class;
        public Task<T?> GetByIdAsync(Guid id);
        public Task<ResponseOptions> UpdateAsync(T entity);
        public Task<ResponseOptions> RemoveAsync(Guid id);
        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
