using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Contracts;

namespace Tripex.Core.Domain.Interfaces.Repositories
{
    public interface ICrudRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T entity);
        public IQueryable<T> GetQueryable<T>() where T : class;
        public Task<T?> GetByIdAsync(Guid id);
        public Task<ResponseOptions> UpdateAsync(T entity);
        public Task<ResponseOptions> RemoveAsync(Guid id);
        public Task<IEnumerable<T>> GetByPostIdAsync<T>(Guid postId) where T : BaseEntity, IPostForeignKey;
    }
}
