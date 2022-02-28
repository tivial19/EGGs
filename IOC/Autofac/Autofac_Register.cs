using Autofac;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using cpADD;
using cpADD.RemoteFileServer;
using cpADD.RemoteFileServer.Yandex;
using cpWORK;

namespace cpIOC.Autofac
{
    public class czIOC_Autofac_Register : czIOC_Autofac_Register_AUTO
    {



        public czIOC_Autofac_Register()
        {

        }



        public async Task<czIIOC_Container> cfBuild_and_Inject(params Func<czIIOC_Container, Task>[] cxInjections)
        {
            var cxR = cfBuild();
            if (cxInjections.Any())
            {
                Task[] cxInjs = new Task[cxInjections.Length];
                for (int i = 0; i < cxInjections.Length; i++) cxInjs[i]=cxInjections[i].Invoke(cxR);
                await Task.WhenAll(cxInjs);
            }
            else throw new Exception("czIOC_Autofac_Register cfBuild_and_Inject _Injections is empty!");
            return cxR;
        }





        public void cfRegister_ALL(czIAPP cxAPP_Instance, params object[] cxInstances)
        {
            if (cxAPP_Instance==null) throw new Exception("czAutofac_Register cfRegister_Common _APP_Instance==null");
            cfRegister_Instance(cxAPP_Instance);
            cfRegister_Instance(cxInstances);
            cfRegister_Ability_AUTO();
            cfRegister_VM_AUTO();
            cfRegister_View_AUTO();


            cfRegister_Type_AsImplemented<czWork>(true);
            //cfRegister_Type_AsImplemented<czBill_model>(true);
            //cfRegister_Type_AsImplemented<czNakls_model>(true);


            bool isEnable_Inet = true;
            if(isEnable_Inet)
            {
                _Builder.Register(c => new czYandexDisk_app(c.Resolve<czIAPP>(), new czYandex_Base_Nagaev())).As<czIRemoteFileServer>().SingleInstance();
                //cfReg_Mongo(ceRemoteFileServer_DB_Name.Items);
                //cfReg_Mongo(ceRemoteFileServer_DB_Name.Bills);
            }
            else
            {
                cfRegister_Type_As<czMOQ_RemoteFileServer, czIRemoteFileServer>(true);
                //_Builder.Register(c => new czRemoteDB_Moq(ceRemoteFileServer_DB_Name.Items)).As<czIRemote_DB_Named>().SingleInstance();
                //_Builder.Register(c => new czRemoteDB_Moq(ceRemoteFileServer_DB_Name.Bills)).As<czIRemote_DB_Named>().SingleInstance();
            }


            //void cfReg_Mongo(ceRemoteFileServer_DB_Name cxDbName)
            //{
            //    _Builder.Register(c => new czMongoDB_Named(cxDbName)).As<czIRemote_DB_Named>().SingleInstance();
            //}
        }




    }





}


////////ceRemoteFileServer_DB_Name[] cxValues = (ceRemoteFileServer_DB_Name[])Enum.GetValues(typeof(ceRemoteFileServer_DB_Name));
////////foreach (var item in cxValues) _Builder.Register(c => new czMongoDB_Named(item)).As<czIRemote_DB_Named>().SingleInstance();
///


//_Builder.Register(c => new czYandexDisk(c.Resolve<czIAPP_Info>().Path_Cache)).SingleInstance().AsImplementedInterfaces();

//_IOC_register.cfRegister_Type_As<czMongoDB, czIRemote_DB>(false);


//_IOC_register.cfRegister_Type_As<czMOQ_RemoteFileServer, czIRemoteFileServer>(true);

//_Builder.Register(c => new czYandexDisk(c.Resolve<czIAPP_Info>().Path_Cache)).SingleInstance().AsImplementedInterfaces();
//_IOC_register.cfRegister_Type_As<czYandexDisk, czIRemoteFileServer>(true);











//_Builder.RegisterType<czPing_model>().As<czIPing>().SingleInstance();



//_Builder.RegisterType<czWork>().AsImplementedInterfaces().SingleInstance();



//////cxBuilder.RegisterType<czItems_Place_Collection>().SingleInstance().AsImplementedInterfaces(); //PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
//cxBuilder.RegisterType<czItems_Place_Collection>().SingleInstance().AsImplementedInterfaces();//.OnActivated(a => a.Instance.cfInject(a.Context.Resolve<czIBill_for_ItemsPlace>()));
//////cxBuilder.RegisterBuildCallback(c => c.Resolve<czIItems_Place>().cfActivate());
//cxBuilder.RegisterType<czBill_model>().AsImplementedInterfaces().SingleInstance();// if connected to VM must be SingleInstance (this is model)

////cxBuilder.RegisterType<czBill_Files>().AsImplementedInterfaces();// this is only helped component (can be create many times in many models)
///



//_Builder.RegisterType<czPing_model>().As<czIPing>().SingleInstance();



//cxBuilder.RegisterType<czPing_model>().WithParameter(new ResolvedParameter((p, e) => p.ParameterType == typeof(czIMSG), cxValueAccessor));


//cxBuilder.RegisterType<czPing_model>().WithParameter(new ResolvedParameter((p, e) => p.ParameterType == typeof(czIMSG), (p,e)=>e.Resolve<czMSG_Output>())).As<czIPing>().SingleInstance();

//cxBuilder.RegisterType<czPing_model>().WithParameter(new ResolvedParameter((p, e) => p.ParameterType == typeof(string), (p, e) => "Fuck you!" )).As<czIPing>().SingleInstance();

//_Builder.RegisterType<czTrace>().As<czITrace>().SingleInstance();
//_Builder.RegisterType<czYandexDisk>().SingleInstance().AsImplementedInterfaces();

//_Builder.RegisterType<czLogger>().As<czILogger>().SingleInstance();

//_Builder.RegisterType<czAsyncModuleStartControl>().As<czIAsyncModuleStartControl>().SingleInstance();
//_Builder.RegisterType<czSaverOPT>().As<czISaverOPT>().SingleInstance();

//_Builder.Register(c => new czYandexDisk(c.Resolve<czIAPP_Info>().Path_Cache)).SingleInstance().AsImplementedInterfaces(); czIRemoteFileServer



//_Builder.RegisterType<czJson_UTF8>().As<czIJSON>();
//_Builder.RegisterType<czSerializer_JSON>().As<czISerializer>();
//_Builder.RegisterType<czMSG_Output>().As<czIMSG>();



//_Builder.RegisterType<czLiteDB>().As<czIDB_Doc>();
//_Builder.RegisterType<czMongo>().As<czIDB_Remote>();