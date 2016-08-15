using System;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Flobot.InversionOfControl
{
    public class DependencyContainer
    {
        private IUnityContainer unityContainer;

        public DependencyContainer()
        {
            unityContainer = new UnityContainer();
        }

        public static DependencyContainer New()
        {
            return new DependencyContainer();
        }

        public DependencyContainer Clear()
        {
            foreach (var registration in unityContainer.Registrations.Where(r => r.LifetimeManager != null))
            {
                registration.LifetimeManager.RemoveValue();
            }

            return this;
        }

        #region Register<T>

        public DependencyContainer Register<T>()
        {
            unityContainer.RegisterType<T>();

            return this;
        }

        public DependencyContainer Register<T>(Lifetime lifetime)
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();

            unityContainer.RegisterType<T>(lifetimeManager);
            return this;
        }

        public DependencyContainer Register<T>(params object[] ctorParameterValues)
        {
            InjectionConstructor constructor = new InjectionConstructor(ctorParameterValues);
            unityContainer.RegisterType<T>(constructor);

            return this;
        }

        public DependencyContainer Register<T>(Lifetime lifetime, params object[] ctorParameterValues)
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();
            InjectionConstructor constructor = new InjectionConstructor(ctorParameterValues);

            unityContainer.RegisterType<T>(lifetimeManager, constructor);

            return this;
        }

        public DependencyContainer Register<T>(string name, params object[] ctorParameterValues)
        {
            InjectionConstructor constructor = new InjectionConstructor(ctorParameterValues);

            unityContainer.RegisterType<T>(name, constructor);

            return this;
        }

        public DependencyContainer Register<T>(string name, Lifetime lifetime, params object[] ctorParameterValues)
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();
            InjectionConstructor constructor = new InjectionConstructor(ctorParameterValues);

            unityContainer.RegisterType<T>(name, lifetimeManager, constructor);

            return this;
        }

        #endregion // Register<T>

        #region Register <TFrom, TTo>

        public DependencyContainer Register<TFrom, TTo>()
            where TTo : TFrom
        {
            unityContainer.RegisterType<TFrom, TTo>();

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(Lifetime lifetime)
            where TTo : TFrom
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();

            unityContainer.RegisterType<TFrom, TTo>(lifetimeManager);

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(params object[] parameterValues)
            where TTo : TFrom
        {
            InjectionConstructor constructor = new InjectionConstructor(parameterValues);
            unityContainer.RegisterType<TFrom, TTo>(constructor);

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(Lifetime lifetime, params object[] parameterValues)
            where TTo : TFrom
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();
            InjectionConstructor constructor = new InjectionConstructor(parameterValues);

            unityContainer.RegisterType<TFrom, TTo>(lifetimeManager, constructor);

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(string name, params object[] parameterValues)
            where TTo : TFrom
        {
            InjectionConstructor constructor = new InjectionConstructor(parameterValues);

            unityContainer.RegisterType<TFrom, TTo>(name, constructor);

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(string name, Lifetime lifetime, params object[] parameterValues)
            where TTo : TFrom
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();
            InjectionConstructor constructor = new InjectionConstructor(parameterValues);

            unityContainer.RegisterType<TFrom, TTo>(name, lifetimeManager, constructor);

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(string name, Lifetime lifetime)
            where TTo : TFrom
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();
            unityContainer.RegisterType<TFrom, TTo>(name, lifetimeManager);

            return this;
        }

        #endregion // Register <TFrom, TTo>

        #region RegisterInstance<T>

        public DependencyContainer RegisterInstance<T>(T instance)
        {
            unityContainer.RegisterInstance<T>(instance);

            return this;
        }

        public DependencyContainer RegisterInstance<T>(T instance, Lifetime lifetime)
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();

            unityContainer.RegisterInstance<T>(instance, lifetimeManager);

            return this;
        }

        public DependencyContainer RegisterInstance<T>(string name, T instance)
        {
            unityContainer.RegisterInstance<T>(name, instance);

            return this;
        }

        public DependencyContainer RegisterInstance<T>(string name, T instance, Lifetime lifetime)
        {
            LifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();

            unityContainer.RegisterInstance<T>(name, instance, lifetimeManager);

            return this;
        }

        #endregion // RegisterInstance<T>

        #region Resolve<T>

        public T Resolve<T>(params NameValueParameter[] ctorParameters)
        {
            ParameterOverride[] parameters = ctorParameters.Select(p => new ParameterOverride(p.Name, p.Value)).ToArray();

            return unityContainer.Resolve<T>(parameters);
        }

        public T Resolve<T>(string name, params NameValueParameter[] ctorParameters)
        {
            ParameterOverride[] parameters = ctorParameters.Select(p => new ParameterOverride(p.Name, p.Value)).ToArray();

            return unityContainer.Resolve<T>(name, parameters);
        }

        #endregion // Resolve<T>
    }
}
