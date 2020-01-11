using System;

namespace Essence.Ioc
{
    public abstract class TestCase
    {
        private readonly string _description;

        protected TestCase(string description)
        {
            _description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public override string ToString() => _description;
    }
}