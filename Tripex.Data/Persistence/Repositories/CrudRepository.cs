﻿using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;

namespace Tripex.Infrastructure.Persistence.Repositories
{
    public class CrudRepository<T>(AppDbContext context) : ICrudRepository<T> where T : BaseEntity
    {
        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync() =>
            await context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) =>
              await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ResponseOptions> UpdateAsync(T entity)
        {
            if (await GetByIdAsync(entity.Id) == null)
                return ResponseOptions.NotFound;

            context.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();

            return ResponseOptions.Ok;
        }
        public async Task<ResponseOptions> RemoveAsync(Guid id)
        {
            var product = await GetByIdAsync(id);

            if(product == null)
                return ResponseOptions.NotFound;

            context.Set<T>().Remove(product);
            await SaveChangesAsync();

            return ResponseOptions.Ok;
        }

        private async Task SaveChangesAsync()
        {
            if (await context.SaveChangesAsync() <= 0)
                throw new InvalidOperationException("Could not save changes");
        }
    }
}