using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Utils
{
    public static class DateTimeHelperMethods
    {


        public static bool IsLaterThan(this DateTime? value, DateTime? compared)
        {
            if (value == null)
            {
                return false;
            }
            else if (compared == null)
            {
                return true;
            }
            else
            {
                return ((DateTime)value).IsLaterThan((DateTime)compared);
            }
        }

        public static bool IsEarlierThan(this DateTime? value, DateTime? compared)
        {
            if (value == null)
            {
                return false;
            }
            else if (compared == null)
            {
                return false;
            }
            else
            {
                return ((DateTime)value).IsEarlierThan((DateTime)compared);
            }
        }

        public static bool IsNotEarlierThan(this DateTime? value, DateTime? compared)
        {
            if (value == null)
            {
                return false;
            }
            else if (compared == null)
            {
                return true;
            }
            else
            {
                return ((DateTime)value).IsNotEarlierThan((DateTime)compared);
            }
        }

        public static bool IsNotLaterThan(this DateTime? value, DateTime? compared)
        {
            if (value == null)
            {
                return false;
            }
            else if (compared == null)
            {
                return false;
            }
            else
            {
                return ((DateTime)value).IsNotLaterThan((DateTime)compared);
            }
        }


        public static bool IsLaterThan(this DateTime value, DateTime compared)
        {
            return (value.CompareTo(compared) > 0);
        }

        public static bool IsEarlierThan(this DateTime value, DateTime compared)
        {
            return (value.CompareTo(compared) < 0);
        }

        public static bool IsNotEarlierThan(this DateTime value, DateTime compared)
        {
            return (value.CompareTo(compared) >= 0);
        }

        public static bool IsNotLaterThan(this DateTime value, DateTime compared)
        {
            return (value.CompareTo(compared) <= 0);
        }

        public static DateTime DayBefore(this DateTime value)
        {
            return value.Add(TimeSpan.FromDays(-1));
        }

        public static DateTime SetTime(this DateTime value, TimeSpan span)
        {
            return value.Midnight().Add(span);
        }

        public static int DayOfWeekTm(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)value.DayOfWeek);
        }

        public static DateTime Midnight(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static bool AreTheSameWeek(this DateTime date, DateTime comparedDate)
        {
            DateTime baseDateWeekStart = date.WeekStart();
            DateTime comparedDateWeekStart = comparedDate.WeekStart();
            return (baseDateWeekStart.Date == comparedDateWeekStart.Date);
        }

        public static DateTime WeekStart(this DateTime date)
        {
            int weekDay = date.DayOfWeekTm();
            int days = weekDay - 1;
            return date.AddDays(-days).Midnight();
        }

        public static int GetWeeksDifferenceTo(this DateTime date, DateTime endDate)
        {
            DateTime baseDateWeekStart = date.WeekStart();
            DateTime comparedDateWeekStart = endDate.WeekStart();
            return (comparedDateWeekStart - baseDateWeekStart).Days / 7;
        }

        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday);
        }

        public static DateTime GetWeekendStart(this DateTime value)
        {
            int daysToAdd = 6 - value.DayOfWeekTm();
            TimeSpan timeSpan = new TimeSpan(daysToAdd, 0, 0, 0);
            return value.Add(timeSpan).Midnight();
        }

        public static bool IsEqual(this DateTime? value, DateTime? compared)
        {
            if (value == null)
            {
                return (compared == null);
            }
            else
            {
                if (compared != null)
                {
                    return (((DateTime)value).CompareTo((DateTime)compared) == 0);
                }
                else
                {
                    return false;
                } 
            }
        }

    }
}
