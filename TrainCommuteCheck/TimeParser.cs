using System;
using System.CodeDom;
using System.Linq;

namespace TrainCommuteCheck
{
    public class TimeParser : IComparable<DateTime>
    {
        private string _time;
        private int _h;
        private int _m;

        public TimeParser(string value)
        {
            var components = value.Split(':').Select(s => Convert.ToInt32((string)s)).ToArray();
            _h = components[0];
            _m = components[1];
            _time = string.Format("{0:00}:{1:00}", _h, _m);
        }

        public int H => _h;
        public int M => _m;
        public string Time => _time;
        public static bool operator ==(TimeParser a, DateTime b)
        {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(TimeParser a, DateTime b)
        {
            return a.CompareTo(b) != 0;
        }

        public static bool operator <(TimeParser a, DateTime b)
        {
            return a.CompareTo(b) > 0;
        }
        public static bool operator >(TimeParser a, DateTime b)
        {
            return a.CompareTo(b) < 0;
        }
        public int CompareTo(DateTime other)
        {
            var hourCompare = other.Hour.CompareTo(H);
            if (hourCompare != 0)
            {
                return hourCompare;
            }
            else
            {
                var minuteCompare = other.Minute.CompareTo(M);
                return minuteCompare;
            }

        }
    }
}