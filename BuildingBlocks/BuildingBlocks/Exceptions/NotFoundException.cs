namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){}
        public NotFoundException(object entity, object key) : base($"{entity} with {key} was not found."){}
    }
}
