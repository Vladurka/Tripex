using System.Text.Json.Serialization;
using Tripex.Core.Domain.Interfaces.Contracts;

namespace Tripex.Core.Domain.Entities
{
    public class Like : BaseEntity, IUserForeignKey, IPostForeignKey
    {
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }   

        public Guid PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; } 
    }
}
