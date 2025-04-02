namespace MyCinemaDiary.Infrastructure.Repositories
{
    public class RepositoryException : Exception
    {
        // Helps make sure that the exception is expected behaviour. 
        public RepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
