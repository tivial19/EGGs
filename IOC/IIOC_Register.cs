using cpADD;
using System;
using System.Collections.Generic;
using System.Text;

namespace cpIOC
{

    public interface czIIOC_Register_AUTO : czIIOC_Register_Base
    {

        void cfRegister_Ability_AUTO();

        void cfRegister_VM_AUTO();

        void cfRegister_View_AUTO();




        void cfRegister_Ability_AUTO<AI, AS>(string cxAbility_NameSpace) where AI : Attribute where AS : Attribute;

        void cfRegister_VM_AUTO<V>() where V : czVM;

        void cfRegister_View_AUTO(string cxViews_Type);

    }

    public interface czIIOC_Register_Base
    {
        void cfRegister_Instance(params object[] cxInstance);

        void cfRegister_Type_AsSelf<T>();

        void cfRegister_Type(Type cxType, Type cxService, bool cxSingle = false);

        void cfRegister_Type_As<T, I>(bool cxSingle = false);

        void cfRegister_Type_AsImplemented<T>(bool cxSingle = false);

        czIIOC_Container cfBuild();

    }

}
