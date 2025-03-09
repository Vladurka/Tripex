namespace BuildingBlocks.Exceptions
{
    public class ExistsException : Exception
    {
        public ExistsException(string message) : base(message){}
        public ExistsException(object entity, object key) : base($"Entity {entity} with {key} already exists."){}
    }
}
