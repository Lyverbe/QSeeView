using QSeeView.Types;
using System;

namespace QSeeView.Models
{
    public class DateChangingArgs
    {
        public DateChangingArgs(DateType dateType, DateTime newDate)
        {
            DateType = dateType;
            NewDate = newDate;
        }

        public DateType DateType { get; }
        public DateTime NewDate { get; }
    }
}
