﻿using System.Collections;
using System.Linq;
using Tripex.Application.DTOs.Comments;
using Tripex.Application.DTOs.Followers;
using Tripex.Application.DTOs.Posts;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Users
{
    public class UserGet : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public IEnumerable<PostGetMin> Posts { get; set; } = new List<PostGetMin>();
        public IEnumerable<UserGetMin> Followers { get; set; } = new List<UserGetMin>();
        public IEnumerable<UserGetMin> Following { get; set; } = new List<UserGetMin>();

        public int FollowersCount {  get; set; }
        public int FollowingCount { get; set; }
        public UserGet(User user)
        {
            Id = user.Id;
            CreatedAt = user.CreatedAt;
            UserName = user.UserName;
            Avatar = user.AvatarUrl;

            Posts = user.Posts
                .Select(post => new PostGetMin(post));
            Following = user.Followers
                .Select(follower => new UserGetMin(follower.FollowingEntity));
            Followers = user.Following
                .Select(following => new UserGetMin(following.FollowerEntity));

            FollowersCount = Followers.Count();
            FollowingCount = Following.Count();
        }
    }
}
