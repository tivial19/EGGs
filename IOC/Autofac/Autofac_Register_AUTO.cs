using Autofac;
using cpADD;
using cpADD.Ability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cpIOC.Autofac
{
    public class czIOC_Autofac_Register_AUTO : czIOC_Autofac_Register_Base, czIIOC_Register_AUTO
    {

        

        public czIOC_Autofac_Register_AUTO()
        {

        }




        public void cfRegister_Ability_AUTO()
        {
            cfRegister_Ability_AUTO<czaAutoRegisterIgnoreAttribute, czaAutoRegisterSingleInstanceAttribute>("cpADD.Ability");
        }

        public void cfRegister_VM_AUTO()
        {
            cfRegister_VM_AUTO<czVM>();
        }

        public void cfRegister_View_AUTO()
        {
            cfRegister_View_AUTO("czView_");
        }










        public void cfRegister_Ability_AUTO<AI, AS>(string cxAbility_NameSpace) where AI : Attribute where AS : Attribute
        {
            //var Qs = from cxT in Qta
            //         where cxT.Namespace.StartsWith(cxBuild_Parameters.Ability_NameSpace + ".")
            //         where cxT.GetCustomAttribute<czaAutoRegisterIgnoreAttribute>() == null
            //         where !cxT.IsInterface
            //         where cxT.GetInterfaces().Any()
            //         from cxI in cxT.GetInterfaces().Where(i => i != typeof(System.ComponentModel.INotifyPropertyChanged))
            //         where cxI.Namespace == cxBuild_Parameters.Ability_NameSpace
            //         where cxI.GetCustomAttribute<czaAutoRegisterIgnoreAttribute>() == null
            //         where cxT.BaseType.GetInterfaces().All(i => i != cxI) //all interfaces from first level (in base class there is no same interface)
            //         select new { Interface = cxI, type = cxT, SingleInstance = cxT.GetCustomAttribute<czaAutoRegisterSingleInstanceAttribute>() != null};

            var Qta = Assembly.GetExecutingAssembly().GetExportedTypes();


            var Qt = from cxT in Qta
                     where cxT.Namespace.StartsWith(cxAbility_NameSpace)
                     where !cxT.IsInterface
                     where cxT.GetInterfaces().Any()
                     where cxT.GetCustomAttribute<AI>(false) == null
                     select cxT;

            var Qi = from cxI in Qta
                     where cxI.Namespace == cxAbility_NameSpace
                     where cxI.IsInterface
                     //where (cxI!=typeof(czIIOC_Activated) && cxI != typeof(System.ComponentModel.INotifyPropertyChanged))
                     where cxI != typeof(System.ComponentModel.INotifyPropertyChanged)
                     where cxI.GetCustomAttribute<AI>(false) == null
                     select cxI;

            var Qit = from cxT in Qt
                      from cxI in Qi
                      where cxI.IsAssignableFrom(cxT)
                      where cxT.BaseType.GetInterfaces().All(i => i != cxI) //all interfaces from first level (in base class there is no same interface)
                      where cxT.GetInterfaces().Count() == 1 || cxT.GetInterfaces().Where(i => i != cxI).Any(s => !cxI.IsAssignableFrom(s)) //Use interfaces of 1 level (czInterface_Level1:czInterface_Level2) 
                      select new { Interface = cxI, type = cxT, SingleInstance = cxT.GetCustomAttribute<AS>() != null };

            foreach (var cxItem in Qit) _Builder.cfRegisterType(cxItem.type, cxItem.Interface, cxItem.SingleInstance);
            //{
            //    //if (cxService.SingleInstance) cxBuilder.RegisterType(cxService.type).As(cxService.Interface).IfNotRegistered(cxService.type).SingleInstance();
            //    //else cxBuilder.RegisterType(cxService.type).As(cxService.Interface).IfNotRegistered(cxService.type);
            //    if (cxService.SingleInstance) cxBuilder.RegisterType(cxService.type).As(cxService.Interface).SingleInstance();
            //    else cxBuilder.RegisterType(cxService.type).As(cxService.Interface);
            //}
        }


        public void cfRegister_VM_AUTO<V>() where V : czVM
        {
            Type cxVM_Type = typeof(V);
            //var QV = CurAssembly.GetTypes().Where(t=>!t.IsGenericType).Where(t => !t.Name.Contains("<")).Where(t => t.Name.StartsWith("czVM_"));
            var QV = Assembly.GetExecutingAssembly().GetTypes().Where(t => cxVM_Type.IsAssignableFrom(t) && t != cxVM_Type);
            foreach (var cxItem in QV) _Builder.RegisterType(cxItem).SingleInstance().AsSelf().OnActivated(o => (o.Instance as V).cfIOC_Activated());

            //foreach (var cxItem in QV) _Builder.cfRegisterType_Self_Single_With_Activated(cxItem);


            //foreach (var cxItem in QV) cxBuilder.RegisterType(cxItem).AsSelf().SingleInstance();
            //foreach (var cxItem in QV.Where(t => t.GetInterface(nameof(czIIOC_Activated)) != null)) cxBuilder.RegisterType(cxItem).As<czIIOC_Activated>().OnActivated(a => (a.Instance as czIIOC_Activated).cfIOC_Activated());


            //foreach (var cxItem in QV) cxBuilder.RegisterType(cxItem).AsSelf().SingleInstance().AutoActivate().OnActivated(a => (a.Instance as czIAUTO_Activate).cfActivate());

            //foreach (var cxItem in QV) cxBuilder.RegisterType(cxItem).AsSelf().SingleInstance().OnActivated(a => (a.Instance as czIAUTO_Activate).cfActivate());

            //cxBuilder.RegisterBuildCallback(c =>
            //{
            //    IEnumerable<czVM> cxVMs = c.Resolve<IEnumerable<czVM>>();
            //    foreach (czVM cxVM in cxVMs) cxVM.cfActivate();
            //});
        }


        public void cfRegister_View_AUTO(string cxViews_Type)
        {
            var Qv = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.StartsWith(cxViews_Type));
            foreach (var cxItem in Qv) _Builder.cfRegisterType_Self_Single_With_Interface(cxItem);
        }











        //public void cfRegister_Static_Classes(string cxStatic_Register_Type_Name= "czRegister_IOC")
        //{
        //    try
        //    {
        //        var QV = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name==cxStatic_Register_Type_Name);
        //        foreach (var cxItem in QV)
        //        {
        //            var cxM = cxItem.GetMethods(BindingFlags.Static| BindingFlags.Public).Single();
        //            /*if (cxM != null)*/ 
        //            cxM.Invoke(null, new object[] { this });
        //        }
        //    }
        //    catch (Exception cxE)
        //    {
        //        string cxInfo = $"Статический класс {cxStatic_Register_Type_Name} должен содержать один статический публичный метод с кодом регистрации компонентов!";
        //        throw new Exception(cxE.Message + Environment.NewLine + cxInfo);
        //    }
        //}




    }

}




//public static class czRegister_IOC
//{
//    public static void cfRegister(czIIOC_Register_Base _IOC_register)
//    {
//        //_IOC_register.cfRegister_Type_As<czMongoDB, czIRemote_DB>(false);
//    }

//}