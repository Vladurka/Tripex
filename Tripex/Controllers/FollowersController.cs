using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Followers;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class FollowersController(IFollowersService service) : BaseApiController
    {

        [HttpPost]
        public async Task<ActionResult> FollowPerson(FollowerAddRemove followerAdd)
        {
            var follower = new Follower(followerAdd.FollowingPersonId, followerAdd.FollowerId);
            var result = await service.FollowPerson(follower);

            return CheckResponse(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Unfollow(FollowerAddRemove followerRemove)
        {
            var follower = new Follower(followerRemove.FollowingPersonId, followerRemove.FollowerId);
            var result = await service.Unfollow(follower);

            return CheckResponse(result);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FollowerGet>>> GetFollowers(Guid userId, [FromQuery] string? userName)
        {
            var followers = await service.GetFollowersAsync(userId, userName);
            var followersGet = followers.Select(user => new FollowerGet(user));

            return Ok(followersGet);
        }

        [HttpGet("following/{userId}")]
        public async Task<ActionResult<IEnumerable<FollowingGet>>> GetFollowing(Guid userId, [FromQuery] string? userName)
        {
            var following = await service.GetFollowingAsync(userId, userName);
            var followingGet = following.Select(user => new FollowingGet(user));

            return Ok(followingGet);
        }
    }
}
