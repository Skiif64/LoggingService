namespace LoggingService.Domain.Exceptions;
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type type, object param, string paramName)
        : base($"{type.Name} with {paramName} = {param} was not found")
    {
        
    }
}
