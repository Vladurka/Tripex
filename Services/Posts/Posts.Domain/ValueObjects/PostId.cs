using BuildingBlocks.Exceptions;

namespace Posts.Domain.ValueObjects;

public class PostId
{
    public Guid Value { get; }
    private PostId(Guid value) => Value = value;
    public static PostId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("PostId cannot be empty.");

        return new PostId(value);
    }
}