﻿namespace Tripex.Core.Domain.Entities
{
    public class Like : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }   

        public Guid PostId { get; set; }
        public Post Post { get; set; } 
    }
}
