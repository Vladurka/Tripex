using BuildingBlocks.Exceptions;

namespace Profiles.Domain.ValueObjects;

public record ProfileId
{
    public Guid Value { get; }
    private ProfileId(Guid value) => Value = value;
    public static ProfileId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        if (value == Guid.Empty)
            throw new DomainException("OrderId cannot be empty.");

        return new ProfileId(value);
    }
}