namespace TrainCommuteCheck
{
    public class JourneyLeg
    {
        public string From { get; set; }
        public string To { get; set; }

        public TimeParser DepartureTime { get; set; }

        public MonitorSettings Monitor { get; set; }
    }
}