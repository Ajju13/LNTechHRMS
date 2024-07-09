using Hangfire;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace HRMS.Functions
{
    public class SimpleInjectorJobActivator : JobActivator
    {
        private readonly Container _container;

        public SimpleInjectorJobActivator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public override object ActivateJob(Type jobType)
        {
            var scope = AsyncScopedLifestyle.BeginScope(_container);
            return _container.GetInstance(jobType);
        }

        public override JobActivatorScope BeginScope()
        {
            return new SimpleInjectorScope(_container);
        }
    }

    public class SimpleInjectorScope : JobActivatorScope
    {
        private readonly Scope _scope;

        public SimpleInjectorScope(Container container)
        {
            _scope = AsyncScopedLifestyle.BeginScope(container);
        }

        public override void DisposeScope()
        {
            _scope.Dispose();
        }

        public override object Resolve(Type jobType)
        {
            return _scope.GetInstance(jobType);
        }
    }
}
