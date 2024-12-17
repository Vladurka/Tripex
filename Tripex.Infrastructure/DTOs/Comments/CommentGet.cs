﻿using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Comments
{
    public class CommentGet : BaseEntity
    {
        public Guid PostId { get; set; }
        public UserGetMin User { get; set; }
        public string Content { get; set; } = string.Empty;

        public CommentGet(Comment comment) 
        {
            Id = comment.Id;
            User = new UserGetMin(comment.User);
            PostId = comment.PostId;
            Content = comment.Content;
            CreatedAt = comment.CreatedAt;
        }
    }
}