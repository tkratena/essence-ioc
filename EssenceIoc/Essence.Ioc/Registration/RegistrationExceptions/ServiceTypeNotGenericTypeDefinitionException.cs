namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class ServiceTypeNotGenericTypeDefinitionException : RegistrationException
    {
        public ServiceTypeNotGenericTypeDefinitionException() 
            : base("Service type is not a generic type definition.")
        {
        }
    }
}