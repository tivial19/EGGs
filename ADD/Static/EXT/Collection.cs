using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.EXT
{



    public static class czextCollection
    {

        public static void AddText(this IList<czStringText> list, string Text)
        {
            list.Add(new czStringText(Text));
        }

        public static void AddText(this IList<czStringText2> list, string Text1, string Text2)
        {
            list.Add(new czStringText2(Text1,Text2));
        }



        //public static void Add_<T>(this IList<T> list, T item)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        list.Add(item);
        //    }));
        //}

        public static void AddList<T>(this IList<T> list, List<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        //public static void AddList_<T>(this IList<T> list, List<T> items)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        foreach (var item in items)
        //        {
        //            list.Add(item);
        //        }
        //    }));
        //}

        public static void AddListInObject<T>(this IList<T> list, object items)
        {
            List<T> cxL = ((System.Collections.IEnumerable)items).Cast<T>().ToList();
            list.AddList(cxL);
        }

        //public static void AddListInObject_<T>(this IList<T> list, object items)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        List<T> cxL = ((System.Collections.IEnumerable)items).Cast<T>().ToList(); //System.Collections.IList cxS = (System.Collections.IList)items;
        //        list.AddList(cxL);
        //    }));
        //}


        //public static void Insert_<T>(this IList<T> list, int index, T item)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        list.Insert(index, item);
        //    }));
        //}

        //public static void RemoveAt_<T>(this IList<T> list, int index)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        list.RemoveAt(index);
        //    }));
        //}

        //public static void Clear_<T>(this IList<T> list)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        list.Clear();
        //    }));
        //}




        public static void RemoveListInObject<T>(this IList<T> list, object items)
        {
            List<T> cxL = ((System.Collections.IEnumerable)items).Cast<T>().ToList();
            foreach (T item in cxL)
                list.Remove(item);
        }

        //public static void RemoveListInObject_<T>(this IList<T> list, object items)
        //{
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        List<T> cxL = ((System.Collections.IEnumerable)items).Cast<T>().ToList();
        //        foreach (T item in cxL)
        //            list.Remove(item);
        //    }));
        //}






    }





}
