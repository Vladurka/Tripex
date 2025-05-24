namespace BuildingBlocks.Exceptions
{
    public class ExistsException : Exception
    {
        public ExistsException(object entity, string field,object key) : base($"{entity} with {field} {key} already exists."){}
    }
}
