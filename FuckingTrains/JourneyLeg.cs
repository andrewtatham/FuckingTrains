using System.Threading;

namespace FuckingTrains
{
    public class JourneyLeg
    {

        public string From { get; set; }
        public string To { get; set; }

        public FuckingTime DepartureTime { get; set; }

        public MonitorSettings Monitor { get; set; }
    }
}