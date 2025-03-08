namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){}
        public NotFoundException(object entity, object key) : base($"Entity {entity} with {key} was not found."){}
    }
}
