using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Repositories
{
    public interface ICrudRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T entity);
        public Task<IReadOnlyList<T>> ListAllAsync();
        public Task<T?> GetByIdAsync(Guid id);
        public Task<ResponseOptions> UpdateAsync(T entity);
        public Task<ResponseOptions> RemoveAsync(Guid id);
    }
}
