using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace cpADD
{

    public class czNotify_Simple_Collection<T> : ObservableCollection<czNotify_Simple<T>>
    {
        public event EventHandler Item_Changed;

        public czNotify_Simple_Collection(T[] cxValue):base(cxValue.Select(s=>new czNotify_Simple<T>(s)))
        {
            cfSet_Event_Changed(Items);
        }

        public T[] Items_Array => Items.Select(s=>s.Value).ToArray();




        protected void cfItem_Changed()
        {
            Item_Changed?.Invoke(this, null);
         }



        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action==NotifyCollectionChangedAction.Add) cfSet_Event_Changed((IList<czNotify_Simple<T>>)e.NewItems);
        }






        private void cfSet_Event_Changed(IList<czNotify_Simple<T>> cxItems)
        {
            foreach (var item in cxItems) item.Item_Changed+=cfItem_Changed;
        }


    }





    public class czNotify_Simple<T>: czNotify_Object
    {
        public event Action Item_Changed;

        public T Value { get; set; }

        public czNotify_Simple(T cxValue)
        {
            Value=cxValue;
        }



        protected override void cfPropertyChanged(string cxName)
        {
            if (cxName==nameof(Value)) cfItem_Changed();
        }

        protected void cfItem_Changed()
        {
            Item_Changed?.Invoke();
        }


    }












}
