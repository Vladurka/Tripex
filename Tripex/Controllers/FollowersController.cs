using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Followers;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    [Authorize]
    public class FollowersController(IFollowersService service, ITokenService tokenService) : BaseApiController
    {

        [HttpPost("{followingPersonId:guid}")]
        public async Task<ActionResult> FollowPerson(Guid followingPersonId)
        {
            var id = tokenService.GetUserIdByToken();
            var follower = new Follower(followingPersonId, id);
            var result = await service.Follow(follower);

            return CheckResponse(result);
        }

        [HttpDelete("{followingPersonId:guid}")]
        public async Task<ActionResult> Unfollow(Guid followingPersonId)
        {
            var id = tokenService.GetUserIdByToken();
            var follower = new Follower(followingPersonId, id);
            var result = await service.Unfollow(follower);

            return CheckResponse(result);
        }

        [HttpGet("{userId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<FollowerGet>>> GetFollowers(Guid userId, int pageIndex, [FromQuery] string? userName)
        {
            var followers = await service.GetFollowersAsync(userId, pageIndex,userName);
            var followersGet = followers.Select(user => new FollowerGet(user))
                .ToList();

            return Ok(followersGet);
        }

        [HttpGet("following/{userId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<FollowingGet>>> GetFollowing(Guid userId, int pageIndex, [FromQuery] string? userName)
        {
            var following = await service.GetFollowingAsync(userId, pageIndex, userName);
            var followingGet = following.Select(user => new FollowingGet(user))
                .ToList();

            return Ok(followingGet);
        }
    }
}
