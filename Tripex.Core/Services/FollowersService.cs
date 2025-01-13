using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class FollowersService(ICrudRepository<Follower> repo, ICrudRepository<User> usersCrudRepo,
        IS3FileService s3FileService) : IFollowersService
    {
        public async Task<ResponseOptions> Follow(Follower followingAdd)
        {
            if (followingAdd.FollowerId == followingAdd.FollowingPersonId)
                return ResponseOptions.BadRequest;

            var exists = await repo.GetQueryable<Follower>()
                .AnyAsync(f => f.FollowerId == followingAdd.FollowerId && f.FollowingPersonId == followingAdd.FollowingPersonId);

            if (exists)
                return ResponseOptions.Exists;

            await using var transaction = await repo.BeginTransactionAsync();
            await repo.AddAsync(followingAdd);

            var userFollowing = await usersCrudRepo.GetByIdAsync(followingAdd.FollowingPersonId);

            if(userFollowing == null)
                return ResponseOptions.NotFound;

            userFollowing.FollowersCount++;
            await usersCrudRepo.UpdateAsync(userFollowing);

            var follower = await usersCrudRepo.GetByIdAsync(followingAdd.FollowerId);

            if (follower == null)
                return ResponseOptions.NotFound;

            follower.FollowingCount++;
            await usersCrudRepo.UpdateAsync(follower);
            await transaction.CommitAsync();

            return ResponseOptions.Ok;
        }

        public async Task<ResponseOptions> Unfollow(Follower followerDelete)
        {
            var followerGet = await GetFollowerAsync(followerDelete.FollowerId, followerDelete.FollowingPersonId);

            if (followerGet == null)
                return ResponseOptions.NotFound;

            var userFollowing = await usersCrudRepo.GetByIdAsync(followerDelete.FollowingPersonId);

            if (userFollowing == null)
                return ResponseOptions.NotFound;

            await using var transaction = await repo.BeginTransactionAsync();
            userFollowing.FollowersCount--;
            await usersCrudRepo.UpdateAsync(userFollowing);

            var follower = await usersCrudRepo.GetByIdAsync(followerDelete.FollowerId);

            if (follower == null)
                return ResponseOptions.NotFound;

            follower.FollowingCount--;
            await usersCrudRepo.UpdateAsync(follower);

            var result = await repo.RemoveAsync(followerGet.Id);
            await transaction.CommitAsync();

            return result;
        }

        public async Task<IEnumerable<Follower>> GetFollowersAsync(Guid userId, int pageIndex ,string? userName)
        {
            var followers =  await repo.GetQueryable<Follower>()
                .AsNoTracking()
                .Include(p => p.FollowerEntity)
                .Where(f => f.FollowingPersonId == userId &&
                    (string.IsNullOrWhiteSpace(userName) ||
                     EF.Functions.ILike(f.FollowerEntity.UserName, $"%{userName}%")))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();

            var tasks = followers.Select(follower => follower.FollowerEntity.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo));
            await Task.WhenAll(tasks);

            return followers;
        }

        public async Task<IEnumerable<Follower>> GetFollowingAsync(Guid userId, int pageIndex, string? userName)
        {
            var following =  await repo.GetQueryable<Follower>()
                .AsNoTracking()
                .Include(p => p.FollowingEntity)
                .Where(f => f.FollowerId == userId &&
                    (string.IsNullOrWhiteSpace(userName) ||
                     EF.Functions.ILike(f.FollowingEntity.UserName, $"%{userName}%")))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();

            var tasks = following.Select(f => f.FollowingEntity.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo));
            await Task.WhenAll(tasks);

            return following;
        }

        private async Task<Follower?> GetFollowerAsync(Guid followerId, Guid followingPersonId)
        {
            var follower =  await repo.GetQueryable<Follower>()
                .SingleOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingPersonId == followingPersonId);

            await follower.FollowerEntity.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo);

            return follower;
        }
    }
}
