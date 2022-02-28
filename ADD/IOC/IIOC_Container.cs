﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cpADD
{

    public interface czIIOC_Container
    {
        object cfGetInstance(Type cxType);

        T cfGetInstance<T>();

        T cfGetInstance_Optional<T>() where T : class;
        bool cfIsRegistered(Type cxType);
    }



}
