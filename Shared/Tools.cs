using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Tools
    {

        public static string ToPersianDate(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                var persianCalendar = new System.Globalization.PersianCalendar();
                int year = persianCalendar.GetYear(dt.Value);
                int month = persianCalendar.GetMonth(dt.Value);
                int day = persianCalendar.GetDayOfMonth(dt.Value);
                int hour = dt.Value.Hour;
                int minute = dt.Value.Minute;
                int second = dt.Value.Second;

                return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";
            }
            return string.Empty;
        }
    }
}
