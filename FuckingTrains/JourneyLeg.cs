using System;
using System.Linq;

namespace FuckingTrains
{
    public class JourneyLeg
    {
        private string _time;
        private int _h;
        private int _m;
        public string From { get; set; }
        public string To { get; set; }

        public string Time
        {

            set
            {
                var components = value.Split(':').Select(s => Convert.ToInt32(s)).ToArray();
                _h = components[0];
                _m = components[1];
                _time = string.Format("{0:00}:{1:00}", _h, _m);
            }
            get { return _time; }
        }

        public int H
        {
            get { return _h; }

        }
        public int M
        {
            get { return _m; }

        }
    }
}