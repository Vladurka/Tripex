using Tripex.Core.Domain.Entities;
using Tripex.Core.Enums;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IFollowersService
    {
        public Task<ResponseOptions> FollowPerson(Follower followingAdd);
        public Task<ResponseOptions> Unfollow(Follower follower);
        public Task<IEnumerable<Follower>> GetFollowersAsync(Guid userId, int pageIndex, string? userName);
        public Task<IEnumerable<Follower>> GetFollowingAsync(Guid userId, int pageIndex, string? userName);
    }
}
