using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace cpADD
{
    public class czObservableRemoveCollection<T>: ObservableCollection<T>
    {
        Func<T, Task<bool>> _Remove_Item_Ask;
        Func<T,Task> _Remove_Item_After;


        IEnumerable<T> _ii => Items;

        public czObservableRemoveCollection(Func<T, Task<bool>> cxRemove_Item_Ask, Func<T, Task> cxRemove_Item_After)
        {
            cfSet_Actions(cxRemove_Item_Ask, cxRemove_Item_After); 
        }

        public czObservableRemoveCollection(Func<T, Task<bool>> cxRemove_Item_Ask, Func<T, Task> cxRemove_Item_After, IEnumerable<T> collection) : base(collection)
        {
            cfSet_Actions(cxRemove_Item_Ask, cxRemove_Item_After);
        }



        private void cfSet_Actions(Func<T, Task<bool>> cxRemove_Item_Ask, Func<T, Task> cxRemove_Item_After)
        {
            _Remove_Item_Ask = cxRemove_Item_Ask;
            _Remove_Item_After = cxRemove_Item_After;
        }




        //public new Task<bool> Remove(T item)
        //{
        //    return cfRemoveItem(item);
        //}

        //public new Task<bool> RemoveAt(int index)
        //{
        //    return cfRemoveItem(Items[index]);
        //}


        public new Task Clear()
        {
            throw new NotImplementedException("Действие запрещено разработчиком!");
        }



        //need for remove from HMI elements by keybord
        protected async override void RemoveItem(int index)
        {
            T cxItem = Items[index];
            if (true == await _Remove_Item_Ask(cxItem))
            {
                base.RemoveItem(index);
                await _Remove_Item_After(cxItem);
            }
        }







    }
}
