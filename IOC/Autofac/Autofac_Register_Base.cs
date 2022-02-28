using Autofac;
using Autofac.Builder;
using Autofac.Core.Registration;
using cpADD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cpIOC.Autofac
{


    public class czIOC_Autofac_Register_Base : czIIOC_Register_Base
    {
        public readonly ContainerBuilder _Builder;


        protected czIOC_Autofac_Register_Base()
        {
            _Builder = new ContainerBuilder();
        }



        public czIIOC_Container cfBuild()
        {
            if (_Builder == null) throw new Exception("cfBuild_IOC _IOC==Null. First you must registrate something");
            return new czIOC_Autofac_Container(_Builder.Build());
        }



        public void cfRegister_Instance(params object[] cxInstance)
        {
            if (cxInstance?.Any() ?? false) foreach (var cxO in cxInstance) _Builder.RegisterInstance(cxO).AsImplementedInterfaces();
        }


        public void cfRegister_Type_AsSelf<T>()
        {
            _Builder.RegisterType<T>().AsSelf();
        }





        public void cfRegister_Type(Type cxType, Type cxService, bool cxSingle = false)
        {
            _Builder.cfRegisterType(cxType, cxService, cxSingle);
        }


        public void cfRegister_Type_As<T,I>(bool cxSingle = false)
        {
            if(cxSingle) _Builder.RegisterType<T>().As<I>().SingleInstance();
            else _Builder.RegisterType<T>().As<I>();
        }




        public void cfRegister_Type_AsImplemented<T>(bool cxSingle = false)
        {
            if (cxSingle) _Builder.RegisterType<T>().AsImplementedInterfaces().SingleInstance();
            else _Builder.RegisterType<T>().AsImplementedInterfaces();
        }




        //public void cfRegister_Manual()
        //{
        //    //throw new NotImplementedException();

        //    //_Builder.Register(c => new czYandexDisk(c.Resolve<czIAPP_Info>().Path_Cache)).SingleInstance().AsImplementedInterfaces();
        //}



    }




}







//public czIIOC_Container cfBuild(object[] cxInstance, Type[] cxTypes_Single, Action<czIIOC_Container>[] cxActionBuildCallback, bool cxAUTO_Register_Ability, Type cxVM_Type, string cxViews_Type = "czView_", string cxAbility_NameSpace = "cpADD.Ability")
//{
//    Assembly cxCurAssembly = Assembly.GetExecutingAssembly();
//    var cxBuilder = new ContainerBuilder();

//    // Current APP
//    if (cxInstance?.Any() ?? false) foreach (var cxO in cxInstance) if (cxO != null) cxBuilder.cfRegisterInstance_With_Activated(cxO);
//    if (cxTypes_Single?.Any() ?? false) foreach (Type cxT in cxTypes_Single) if (cxT != null) cxBuilder.cfRegisterType_Self_Single_Interface_With_Activated(cxT);

//    //Default Register
//    if (cxAUTO_Register_Ability) cfRegister_Ability(cxBuilder, cxAbility_NameSpace);
//    if (cxVM_Type != null) cfRegister_VM(cxBuilder, cxVM_Type);
//    if (cxViews_Type != null) cfRegister_View(cxBuilder, cxViews_Type);

//    cxBuilder.RegisterAssemblyModules(cxCurAssembly);


//    cxBuilder.RegisterBuildCallback(c =>
//    {
//        _container = c;
//        if (cxActionBuildCallback?.Any() ?? false)
//            foreach (var cxAct in cxActionBuildCallback) cxAct(this);
//    });

//    _container = cxBuilder.Build();

//    return this;
//}




//czIIOC_Controller Qic = _container.Resolve<czIIOC_Controller>();
//Qic.cfIOC_Container_Ready(this);


////cxBuilder.RegisterInstance<czIIOC_Container>(this).SingleInstance();
////cxBuilder.RegisterBuildCallback(c => c.Resolve<czIIOC_Container>().cfPostBuild(cfSet(c. .ComponentRegistry.)));
////cxBuilder.RegisterBuildCallback(c => c.Resolve<czIAPP_IOC>().cfPostBuild());

//cxBuilder.RegisterInstance<czIIOC_Container>(this).SingleInstance().OnRegistered(a =>
//{
//    var x = a.ComponentRegistration.Target;
//    var x = a.ComponentRegistry.Registrations;
//});

//cxBuilder.Register(r=>r.)

//cxBuilder.RegisterBuildCallback(c =>
//{
//    c.Resolve<czIIOC_Controller>().cfIOC_Container_Ready(this);

//});


//cxBuilder.Register(c =>
//{
//    c.
//});




//cxBuilder.RegisterCallback(c =>
//{


//    c.Registered += (s, a) =>
//      {
//          //a.ComponentRegistration.PipelineBuilding += (s, e) => 
//          //{
//          //    e.Use(Autofac.Core.Resolving.Pipeline.PipelinePhase.Activation, (context, next) =>
//          //      {
//          //          if (context.Instance is czIIOC_Ability) (context.Instance as czIIOC_Ability).cfIOC_Activated();
//          //      });
//          //};

//          //var T = a.ComponentRegistration.Activator.LimitType;

//          //if(T.GetInterface(nameof(czIIOC_Ability))!=null)
//          //{
//          //    //IRegistrationBuilder cx;


//          //}

//          //a.ComponentRegistration.Activator.ac
//          //(s as IComponentRegistryBuilder).


//      };

//});




////Current czAutofac_Container need this to set _container in BuildCallback befor build end
///and register defalt after all modules are resisters
//cxBuilder.RegisterInstance<czAutofac_Container>(this).SingleInstance();
//cxBuilder.RegisterBuildCallback(c => c.Resolve<czAutofac_Container>().cfSet_Container_BuildCallback(c,cxBuilder, cxBuild_Parameters));


////Custom Register
//cxBuilder.RegisterBuildCallback(c => c.Resolve<czISaver_Modules>().cfLoad_Objects_From_File());
//cxBuilder.Register(c => new czYandexDisk(c.Resolve<czIAPP>().Pathes.Cache)).SingleInstance().AsImplementedInterfaces();



//// For Test to see what is registreted
//cxBuilder.RegisterInstance<czIOC_Autofac_Container>(this).SingleInstance().OnRegistered(a =>
//{

//    var x = a.ComponentRegistry.Registrations;
//});


#region TEST

//cxBuider.RegisterAssemblyTypes(cxCurAssembly).Where(t => t.GetCustomAttribute<czaAutoRegisterModuleAttribute>() != null).SingleInstance();

//cxBuilder.RegisterType<czMSG_Output>().AsImplementedInterfaces();

//cxBuilder.RegisterType<czMSG_Output>().Keyed<czIMSG>(typeof(czMSG_Output));
//cxBuilder.RegisterType<czMSG_Window>().Keyed<czIMSG>(typeof(czMSG_Window));

//cxBuider.Register(c => new czAPP_model_base(c.Resolve<czIAPP_Info>(), c.Resolve<czISaver_Modules>(), null)).AsImplementedInterfaces();





//Where(t => t.GetInterfaces().Where(i => i.Name.Contains(t.Name.Substring(2))).Any()).
//Select(t => new { T = t.Name, I = t.GetInterfaces() });



//var Qti = Assembly.Load(nameof(Start)).GetTypes().
//            Where(t => t.FullName.Contains("Interfaces")).
//            Where(s => s.Name.StartsWith("cz")).
//            Where(s => !s.Name.StartsWith("czI") && !s.Name.StartsWith("cza")).
//            Where(s => s.GetCustomAttribute<czaAutoRegisterIgnoreAttribute>() == null).
//            Select(s => new { T = s, I = s.GetInterface($"czI{s.Name.Substring(2)}"), isSingle = s.GetCustomAttribute<czaAutoRegisterSingleInstanceAttribute>() != null });

//if (Qti.Where(s => s.I == null).Count() > 0) throw new Exception($"Нет интерфейса");
//foreach (var cxItem in Qti)
//{
//    if (cxItem.isSingle) cxBuider.RegisterType(cxItem.T).As(cxItem.I).SingleInstance();
//    else cxBuider.RegisterType(cxItem.T).As(cxItem.I);
//}




//var Qm = Assembly.Load(nameof(Start)).GetTypes().Where(t => t.GetCustomAttribute<czaAutoRegisterModuleAttribute>() != null);
//foreach (var cxType in Qm)
//{
//    var cxCustomAttribute = cxType.GetCustomAttribute<czaAutoRegisterModuleAttribute>();

//    //if (cxCustomAttribute.EnableClassInterceptors != null && cxCustomAttribute.EnableClassInterceptors.Count() > 0)
//    //    cxBuider.RegisterType(cxType).EnableClassInterceptors().InterceptedBy(cxCustomAttribute.EnableClassInterceptors);
//    //else cxBuider.RegisterType(cxType);
//}


//IInterceptors

//var Qi = Assembly.Load(nameof(Start)).GetTypes().Where(t => t.GetInterfaces().Where(i => i == typeof(IInterceptor)).Count() == 1);
//foreach (var cxType in Qi) cxBuider.RegisterType(cxType);


//cxBuider.RegisterType<czAPP_module>().AsSelf().As<czIAPP_module>();





//var cxpp_module = czContainer.Container.Resolve<czAPP_module>();
//var model = czContainer.Container.Resolve<czmodelTest>();

//czIAPP_Info cxAppI1 = czContainer.Container.Resolve<czAPP_Info>();
//czIAPP_Info cxAppI2 = czContainer.Container.Resolve<czIAPP_Info>();
//var cxS1 = cxAppI1.GetType();
//var cxS2 = cxAppI2.GetType();
//var b1 = cxAppI1 is czIAPP_Info;
//var b2 = cxAppI2 is czIAPP_Info;
//var b = cxAppI1 == cxAppI2;



//cxBuider.RegisterType AssemblyTypes(Assembly.Load(nameof(Start))).Where(t=>t.Namespace.Contains()



//cxBuider.RegisterType<czSaver_JSON>().As<czISaver>();//???????????????????????

//cxBuider.RegisterType<czPropertyChange>().As<czIPropertyChange>();
//cxBuider.RegisterType<czSaver_JSON>().As<czISaver_JSON>();

//cxBuider.RegisterType<czAPP_Info>().As<czIAPP_Info>().SingleInstance();
//cxBuider.RegisterType<czSaver_Modules>().As<czISaver_Modules>().SingleInstance();



//cxBuider.RegisterType<czAPP_module>();//.AsSelf();
//cxBuider.RegisterType<czPing_model>().AsSelf();
//cxBuider.RegisterType<czmodelTest>();//.AsSelf();

//cxBuider.RegisterType<czPing_model>().EnableClassInterceptors().InterceptedBy(typeof(czInterceptor_PropertyChange));

//cxBuider.RegisterType<czmodelTest>().As<czImodelTest>.EnableInterfaceInterceptors();
//cxBuider.RegisterType<czmodelTest>().AsSelf().EnableClassInterceptors().InterceptedBy(typeof(czCallLogger));

//cxBuider.RegisterType<czInterceptor_PropertyChange>();
//cxBuider.Register(c => new czInterceptor_PropertyChange());


//cxBuider.RegisterType<czCallLogger>();

//cb.Register(c => CreateChannelFactory()).SingleInstance();
//cb
//  .Register(c => c.Resolve<ChannelFactory<ITestService>>().CreateChannel())
//  .InterceptTransparentProxy(typeof(IClientChannel))
//  .InterceptedBy(typeof(TestServiceInterceptor))
//  .UseWcfSafeRelease();





//var proxy = new ProxyGenerator();
//czmodelTest model = proxy.CreateClassProxy<czmodelTest>(new czCallLogger());

//model.cfSet_Data();
#endregion