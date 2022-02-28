using Android.App;
using Android.Content;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace EGGs.Droid
{


    public class czTimePickerDialog_Ex 
    {
        bool _Hide = false;
        bool _OK = false;


        public TimeSpan? Value { get; private set; }

        czTimePickerDialog _TimePickerDialog;

        public czTimePickerDialog_Ex(Context mainActivity, bool is24HourView) 
        {
            _TimePickerDialog = new czTimePickerDialog(mainActivity, cfCallBack, is24HourView);
            _TimePickerDialog.DismissEvent += (s, e) => _Hide = true;
            _TimePickerDialog.ceTimeChanged += cfOnTimeChanged;
        }


        public async Task<TimeSpan?> cfShow(TimeSpan cxStart_Value, TimeSpan? cxbtn2 = null, TimeSpan? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            Value = cxStart_Value;
            _Hide = false;
            _OK = false;

            _TimePickerDialog.UpdateTime(cxStart_Value.Hours, cxStart_Value.Minutes);
            if (cxTitle != null) _TimePickerDialog.SetTitle(cxTitle);
            if (cxText != null) _TimePickerDialog.SetMessage(cxText);

            _TimePickerDialog.SetButton("OK", (s, e) => _OK = true);//for android 4.4 in 10 not work
            if (cxbtn2 != null) _TimePickerDialog.SetButton2(cxbtn2.Value.ToString(), (s, e) => { Value = cxbtn2; _OK = true; });
            if (cxbtn3 != null) _TimePickerDialog.SetButton3(cxbtn3.Value.ToString(), (s, e) => { Value = cxbtn3; _OK = true; });

            _TimePickerDialog.Show();

            await Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(300);
                } while (!_Hide);
            });

            if (_OK == false || Value == null) return null;
            else return Value.Value.Add(new TimeSpan(0, 0, 0, DateTime.Now.Second, DateTime.Now.Millisecond));
        }

        public void cfOnTimeChanged(TimeSpan cxTime)
        {
            //if need for android 10
            if (_TimePickerDialog.IsShowing && _Hide == false) Value = cxTime;
        }



        //public override void OnTimeChanged(TimePicker view, int hourOfDay, int minute)
        //{
        //    if (IsShowing && _Hide == false) Value = new TimeSpan(hourOfDay, minute, 0);//for android 10
        //    base.OnTimeChanged(view, hourOfDay, minute);
        //}



        //public void cfCallBack()
        //{
        //    _OK = true;//for android 10 not work on 4.4
        //}







        void cfCallBack(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            //((TimePicker)sender).ti
            //((czTimePickerDialog)sender).cfCallBack();
            //throw new czAPP_Exception_Normal("cfCallBack " + sender.GetType().Name);

            _OK = true;//for android 10 not work on 4.4
        }


    }







    public class czTimePickerDialog: TimePickerDialog
    {
        public event Action<TimeSpan> ceTimeChanged;

        public czTimePickerDialog(Context mainActivity, EventHandler<TimeSetEventArgs> callBack, bool is24HourView) : base(mainActivity, callBack, 0, 0, is24HourView)
        {

        }



        public override void OnTimeChanged(TimePicker view, int hourOfDay, int minute)
        {
            ceTimeChanged?.Invoke(new TimeSpan(hourOfDay,minute,0));
            base.OnTimeChanged(view, hourOfDay, minute);
        }



    }




     


}
