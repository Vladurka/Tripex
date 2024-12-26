using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;

namespace Tripex.Core.Services
{
    public class FollowersService(ICrudRepository<Follower> repo) : IFollowersService
    {
        public async Task<ResponseOptions> FollowPerson(Follower followingAdd)
        {
            var exists = await repo.GetQueryable<Follower>()
                .AnyAsync(f => f.FollowerId == followingAdd.FollowerId && f.FollowingPersonId == followingAdd.FollowingPersonId);

            if (exists)
                return ResponseOptions.Exists;

            await repo.AddAsync(followingAdd);
            return ResponseOptions.Ok;
        }

        public async Task<ResponseOptions> Unfollow(Follower follower)
        {
            var followerGet = await GetFollowerAsync(follower.FollowerId, follower.FollowingPersonId);

            if (followerGet == null)
                return ResponseOptions.NotFound;

            return await repo.RemoveAsync(followerGet.Id);
        }

        public async Task<IEnumerable<Follower>> GetFollowersAsync(Guid userId, int pageIndex ,string? userName)
        {
            return await repo.GetQueryable<Follower>()
                .Include(p => p.FollowerEntity)
                .Where(f => f.FollowingPersonId == userId &&
                    (string.IsNullOrWhiteSpace(userName) ||
                     EF.Functions.ILike(f.FollowerEntity.UserName, $"%{userName}%")))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();
        }

        public async Task<IEnumerable<Follower>> GetFollowingAsync(Guid userId, int pageIndex, string? userName)
        {
            return await repo.GetQueryable<Follower>()
                .Include(p => p.FollowingEntity)
                .Where(f => f.FollowerId == userId &&
                    (string.IsNullOrWhiteSpace(userName) ||
                     EF.Functions.ILike(f.FollowingEntity.UserName, $"%{userName}%")))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();
        }

        private async Task<Follower?> GetFollowerAsync(Guid followerId, Guid followingPersonId)
        {
            return await repo.GetQueryable<Follower>()
                .SingleOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingPersonId == followingPersonId);
        }
    }
}
