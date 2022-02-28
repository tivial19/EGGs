using System;
using System.Collections.Generic;
using System.Text;

namespace cpWORK
{
    public class czFilter
    {
        public czFilter()
        {
            Text = null;

            isDate_Used = false;
            Date_End = DateTime.Today;
            Date_Start = new DateTime(Date_End.Year, Date_End.Month, 1);
        }

        public string Text { get; set; }

        public bool isDate_Used { get; set; }

        public DateTime Date_Start { get; set; }

        public DateTime Date_End { get; set; }


        public bool isEmpty => string.IsNullOrWhiteSpace(Text) && !isDate_Used;


    }
}
