//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;

//namespace BlocksCore.Autofac.Extensions.DependencyInjection
//{
//    public sealed class AutofacServiceCollection : IServiceCollection
//    {
//        public readonly ContainerBuilder Builder;

//        public AutofacServiceCollection()
//        {
//            Builder = new ContainerBuilder();
//            AddSingleton<IServiceProvider, AutofacServiceProvider>();
//        }

//        public ServiceDescriptor this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public int Count => throw new NotImplementedException();

//        public bool IsReadOnly => throw new NotImplementedException();

//        public void Add(ServiceDescriptor item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(ServiceDescriptor item)
//        {
//            throw new NotImplementedException();
//        }

//        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerator<ServiceDescriptor> GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        public int IndexOf(ServiceDescriptor item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Insert(int index, ServiceDescriptor item)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Remove(ServiceDescriptor item)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveAt(int index)
//        {
//            throw new NotImplementedException();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddTransient<TContract, TService>() where TService : TContract
//        //{
//        //    Builder.RegisterType<TService>().As<TContract>().InstancePerDependency();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddTransient<TService>(Func<IServiceProvider, TService> factory)
//        //{
//        //    Builder.Register<TService>(context => factory(context.Resolve<IServiceProvider>())).InstancePerDependency();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddScoped<TContract, TService>() where TService : TContract
//        //{
//        //    Builder.RegisterType<TService>().As<TContract>().InstancePerRequest();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddScoped<TService>(Func<IServiceProvider, TService> factory)
//        //{
//        //    Builder.Register<TService>(context => factory(context.Resolve<IServiceProvider>())).InstancePerRequest();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddSingleton<TContract, TService>() where TService : TContract
//        //{
//        //    Builder.RegisterType<TService>().As<TContract>().SingleInstance();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddSingleton<TService>(Func<IServiceProvider, TService> factory)
//        //{
//        //    Builder.Register<TService>(context => factory(context.Resolve<IServiceProvider>())).SingleInstance();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public void AddSingleton<TService>(TService service)
//        //{
//        //    Builder.Register<TService>(context => service).SingleInstance();
//        //}

//        //[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
//        //public IServiceProvider Build() => Builder.Build().Resolve<IServiceProvider>();
//    }
//}
