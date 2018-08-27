using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager
{
    public static class CalcDate
    {
        public static DateTime MonthEnd(DateTime dateEnd)
        {
            int dayInMonth;
            int yy, mm;
            yy = dateEnd.Year;
            mm = dateEnd.Month;
            dayInMonth = DateTime.DaysInMonth(yy, mm);
            DateTime monthEnd = new DateTime(yy, mm, dayInMonth);
            return monthEnd;
        }
        /// <summary>
        /// кількість днів між датами
        /// </summary>
        /// <param name="dateBegin">перша дата</param>
        /// <param name="dateEnd">друга дата</param>
        /// <returns>кількість днів між датами</returns>
        public static int DaysDiff(DateTime dateBegin, DateTime dateEnd)
        {
            int daysDiff;
            daysDiff = dateBegin.Date.Subtract(dateEnd.Date).Days;
            return daysDiff;
        }
        /// <summary>
        /// кількість днів між датами
        /// </summary>
        /// <param name="strDateBegin">перша дата</param>
        /// <param name="strDateEnd">друга дата</param>
        /// <returns>кількість днів між датами</returns>
        public static int DaysDiff(string strDateBegin, string strDateEnd)
        {
            DateTime dateBegin = new DateTime();
            if (!DateTime.TryParse(strDateBegin, out dateBegin))
            {
                dateBegin = DateTime.Now;
            }
            DateTime dateEnd = new DateTime();
            if (!DateTime.TryParse(strDateEnd, out dateEnd))
            {
                dateEnd = DateTime.Now;
            }
            int daysDiff = DaysDiff(dateBegin, dateEnd);
            return daysDiff;
        }
        /// <summary>
        /// кількість днів між заданою датою і сьогоднішньою
        /// </summary>
        /// <param name="strDateBegin">дата</param>
        /// <returns>кількість днів між заданою датою і сьогоднішньою</returns>
        public static int DaysDiffNow(string strDateBegin)
        {
            DateTime dateBegin = new DateTime();
            if (!DateTime.TryParse(strDateBegin, out dateBegin))
            {
                dateBegin = DateTime.Now;
            }
            DateTime dateEnd = new DateTime();
            dateEnd = DateTime.Now;
            int daysDiff = DaysDiff(dateBegin, dateEnd);
            return daysDiff;
        }
    }
}
