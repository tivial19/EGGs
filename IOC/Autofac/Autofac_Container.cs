using Autofac;
using cpADD;
using System;
using System.Collections.Generic;
using System.Text;

namespace cpIOC.Autofac
{
    public class czIOC_Autofac_Container : czIIOC_Container
    {
        protected readonly ILifetimeScope _container;



        public czIOC_Autofac_Container(ILifetimeScope container)
        {
            _container = container;
        }



        public void cfDispose()
        {
            _container?.Dispose();
        }




        public object cfGetInstance(Type cxType)
        {
            return _container.Resolve(cxType);
        }

        public T cfGetInstance<T>()
        {
            return _container.Resolve<T>();
        }

        public T cfGetInstance_Optional<T>() where T : class
        {
            return _container.ResolveOptional<T>();
        }


        public bool cfIsRegistered(Type cxType)
        {
            return _container.IsRegistered(cxType);
        }


    }
}
