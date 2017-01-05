using System;
using System.Linq;

namespace TrainCommuteCheck
{
    public class TimeParser
    {
        private string _time;
        private int _h;
        private int _m;

        public TimeParser(string value)
        {
            var components = value.Split(':').Select(s => Convert.ToInt32((string) s)).ToArray();
            _h = components[0];
            _m = components[1];
            _time = string.Format("{0:00}:{1:00}", _h, _m);
        }

        public int H => _h;
        public int M => _m;
        public string Time => _time;
    }
}