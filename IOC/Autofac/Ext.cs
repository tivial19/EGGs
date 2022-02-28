using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cpIOC.Autofac
{
    public static class czEXT_IC
    {


        //Ability
        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegisterType(this ContainerBuilder builder, Type cxImplementationType, Type cxServiceType, bool cxSingle)
        {
            if (cxSingle) return cfRegType().SingleInstance();
            else return cfRegType();

            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegType()
            {
                return builder.RegisterType(cxImplementationType).As(cxServiceType);
            }

        }





        //View
        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegisterType_Self_Single_With_Interface(this ContainerBuilder builder, Type cxImplementationType)
        {
            return builder.RegisterType(cxImplementationType).AsSelf().SingleInstance().As(cfGetImplementedInterfaces(cxImplementationType,false));
        }


        static Type[] cfGetImplementedInterfaces(Type cxImplementationType, bool cxShowExeptionNoRegInterface)
        {
            Type[] cxIs = cxImplementationType.GetInterfaces();
            if (!(cxIs?.Any() ?? false)) throw new Exception("cfGetImplementedInterfaces_withInject there is no any interfaces!");

            var QregI = cxIs.Where(t => t.Name.StartsWith("czI"));
            if (cxShowExeptionNoRegInterface && !QregI.Any()) throw new Exception("cfGetImplementedInterfaces_withInject there is no reg interfaces!");

            return QregI.ToArray();
        }








        ////VM
        //public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegisterType_Self_Single_With_Activated(this ContainerBuilder builder, Type cxImplementationType)
        //{
        //    return cfRegisterType_With_Activated(builder, cxImplementationType, cxImplementationType, true);
        //}






        //static Type[] cfGetImplementedInterfaces(object cxInstance)
        //{
        //    return cfGetImplementedInterfaces(cxInstance.GetType(), true);
        //}



        //static (Type[], Type[]) cfGetImplementedInterfaces_withInject(Type cxImplementationType, bool cxShowExeptionNoInject, bool cxShowExeptionNoRegInterface)
        //{
        //    Type[] cxIs = cxImplementationType.GetInterfaces();
        //    if (!(cxIs?.Any() ?? false)) throw new Exception("cfGetImplementedInterfaces_withInject there is no any interfaces!");

        //    var Qinj = cxIs.Where(i => i.Name.Contains(Interface_Inject));
        //    //var Qi = cxIs.Where(i =>i!= typeof(czIOC_Inject)).Where(i=> typeof(czIOC_Inject).IsAssignableFrom(i));
        //    if (cxShowExeptionNoInject && !Qinj.Any()) throw new Exception("cfGetImplementedInterfaces_withInject there is no inject interfaces!");

        //    var QregI = cxIs.Except(Qinj).Where(t => t.Name.StartsWith("czI")).Where(t => t != typeof(czIIOC_Activated));// && t != typeof(czIOC_Inject));
        //    if (cxShowExeptionNoRegInterface && !QregI.Any()) throw new Exception("cfGetImplementedInterfaces_withInject there is no reg interfaces!");

        //    return (QregI.ToArray(), Qinj.ToArray());
        //}


        //static void cfActivate(IActivatedEventArgs<object> a)
        //{
        //    //foreach (var cxIi in cxIInject)
        //    //{
        //    //    var cxMi = cxIi.GetMethods();//.Where(s => s.Name.StartsWith("cf"+Interface_Inject));
        //    //    if (!cxMi.Any()) throw new Exception("cfRegisterInstanceWithInject there is no inject methods!");
        //    //    foreach (var cxInjMeth in cxMi)
        //    //    {
        //    //        var Params = cxInjMeth.GetParameters().Select(s => s.ParameterType);
        //    //        object[] cxResolved = Params.Select(s => a.Context.Resolve(s)).ToArray();
        //    //        cxInjMeth.Invoke(a.Instance, cxResolved);
        //    //    }
        //    //}
        //    if (a.Instance is czIIOC_Activated) (a.Instance as czIIOC_Activated).cfIOC_Activated();
        //}










        //static readonly string Interface_Inject = "IOCInject";


        ////Instance
        //public static IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> cfRegisterInstance_With_Activated(this ContainerBuilder builder, object cxInstance)
        //{
        //    var Qreg = cfGetImplementedInterfaces(cxInstance);
        //    return builder.RegisterInstance(cxInstance).As(Qreg.ToArray()).OnActivated(a => cfActivate(a));
        //}

        ////Single
        //public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegisterType_Self_Single_Interface_With_Activated(this ContainerBuilder builder, Type cxImplementationType)
        //{
        //    var Qreg = cfGetImplementedInterfaces(cxImplementationType, true);
        //    return builder.RegisterType(cxImplementationType).SingleInstance().AsSelf().As(Qreg.ToArray()).OnActivated(a => cfActivate(a));
        //}

        ////Ability
        //public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegisterType_With_Activated(this ContainerBuilder builder, Type cxImplementationType, Type cxServiceType, bool cxSingle)
        //{
        //    if (cxSingle) return cfRegAct().SingleInstance();
        //    else return cfRegAct();

        //    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegAct()
        //    {
        //        Type cxI = cxImplementationType.GetInterface(nameof(czIIOC_Activated));
        //        if (cxI == null) return cfRegType();
        //        else return cfRegType().OnActivated(a => (a.Instance as czIIOC_Activated).cfIOC_Activated());
        //    }

        //    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> cfRegType()
        //    {
        //        return builder.RegisterType(cxImplementationType).As(cxServiceType);
        //    }

        //}



    }
}
