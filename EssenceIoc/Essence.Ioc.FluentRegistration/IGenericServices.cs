using System;
using System.Diagnostics.Contracts;

namespace Essence.Ioc.FluentRegistration
{
    public interface IGenericServices
    {
        void ImplementedBy(Type genericServiceImplementationTypeDefinition);

        [Pure]
        IGenericServices AndService(Type genericServiceTypeDefinition);
    }
}