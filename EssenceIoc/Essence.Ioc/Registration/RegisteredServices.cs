using System;
using System.Collections.Generic;
using System.Reflection;
using Essence.Ioc.Registration.RegistrationExceptions;

namespace Essence.Ioc.Registration
{
    internal class RegisteredServices
    {
        private readonly ISet<Type> _registeredServices = new HashSet<Type>();
        
        public void MarkRegistered(Type serviceType)
        {
            if (typeof(IDisposable).GetTypeInfo().IsAssignableFrom(serviceType))
            {
                throw new DisposableServiceException(serviceType);
            }
            
            lock (serviceType)
            {
                if (_registeredServices.Contains(serviceType))
                {
                    throw new AlreadyRegisteredException(serviceType);
                }
            
                _registeredServices.Add(serviceType);
            }
        }
    }
}