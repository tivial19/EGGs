using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.EXT
{

    public static class czextObjects
    {
        //public static double ToDouble(this decimal d)
        //{
        //    return Convert.ToDouble(d);
        //}

        //public static double? ToDouble(this decimal? d)
        //{
        //    if (d == null) return null;
        //    else return Convert.ToDouble(d.Value);
        //}


        public static decimal ToDecimal(this double d)
        {
            return Convert.ToDecimal(d);
        }

        public static decimal? ToDecimal(this double? d)
        {
            if (d == null) return null;
            else return Convert.ToDecimal(d.Value);
        }


        public static string ToSF(this decimal? d, int cxDigits, bool cxAlwaysShow = false)
        {
            if (d == null) return string.Empty;
            else return d.Value.ToSF(cxDigits, cxAlwaysShow);
        }
        public static string ToSF(this decimal d, int cxDigits, bool cxAlwaysShow=false)
        {
            char cxC = cxAlwaysShow ? '0' : '#';
            string cxS = new string(cxC, cxDigits);
            return d.ToString("0." + cxS);
        }


        public static decimal Round(this decimal d, int DigitsAfterPoint)
        {
            return Math.Round(d, DigitsAfterPoint, MidpointRounding.AwayFromZero);
        }

        public static decimal Round_UP(this decimal d, int DigitsAfterPoint)
        {
            decimal cxK = Math.Pow(10, DigitsAfterPoint).ToDecimal();
            decimal cxD = d * cxK;
            decimal cxT = Math.Truncate(cxD);
            if (cxD > cxT) cxT += 1;
            return Math.Round(cxT / cxK, DigitsAfterPoint, MidpointRounding.AwayFromZero);
        }

        public static decimal Round_Smart(this decimal d, int DigitsAll)
        {
            int cxDigits = 0;
            int cxX = (int)d;
            if (cxX >= 1000) cxDigits = 4;
            else
            {
                if (cxX >= 100) cxDigits = 3;
                else
                {
                    if (cxX >= 10) cxDigits = 2;
                    else cxDigits = 1;
                }
            }
            cxDigits = DigitsAll - cxDigits;
            if (cxDigits < 0) cxDigits = 0;

            return Math.Round(d, cxDigits, MidpointRounding.AwayFromZero);
        }



        public static string TrimStart_(this string Text, int cxCutCount)
        {
            string cxR = null;
            if (Text.Length > cxCutCount) cxR = Text.Substring(cxCutCount, Text.Length - cxCutCount);
            return cxR;
        }





    }


}









//EXAMPLES/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
    int? x=null;
    x??=5;

    public Func<int> Func_Delegate { get; set; }
    List<int> cxR = Func_Delegate.GetInvocationList().Cast<Func<int>>().Select(s => s()).ToList();

    //Status = $"Result={czFC.Time_Fix(false)}";

    //int cxT = Thread.CurrentThread.ManagedThreadId;

????????????????????????????????????  
    int? x; x=2; x=null;
    int y = x ?? -1;
    y = x == 3 ? 6 : 8;

    Predicate<int> cxPredicate = null;
    //Predicate<int> cxPredicate = d => cfFunc_Predicate(d);
    bool cxResult = (cxPredicate ?? (d => true))(5);

    Action<int> cxAct1= cfNormalAction;
    (cxAct1 ?? ((d) => cfActionNULL(d)))(7);

    Func<int,int?> cxFunc1 = null;
    int? cxRFp = (cxFunc1??(d=>d++))(5);
    int? cxRF= cxFunc1(9) ?? 5;
    
    isLog_Enable = cxMVVM_Save?.isLog_Enable ?? false;

// Set y to the value of x if x is NOT null; otherwise,
// if x = null, set y to -1.
        int? x;
        int y = x ?? -1;
       
// Assign i to return value of the method if the method's result
// is NOT null; otherwise, if the result is null, set i to the
// default value of int.
        int i = GetNullableInt() ?? default(int);

????????????????????????????????????

***************MVVM EVENTs


MVVM.czMVVM_CMD cxCMD = (MVVM.czMVVM_CMD)((FrameworkElement)sender).Tag;
cxCMD.Execute(((TreeView)sender).SelectedItem);


**************************

StringFormat=\{0:dd.MM.yy HH:mm:ss\}


    */
