namespace Essence.Ioc.ExtendableRegistration
{
    public abstract class Registerer
    {
        protected abstract Registrations Registrations { get; }
        
        public static explicit operator Registrations(Registerer registerer)
        {
            return registerer.Registrations;
        }
    }
}