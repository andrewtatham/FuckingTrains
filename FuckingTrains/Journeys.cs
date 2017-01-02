using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckingTrains
{
    public static class Journeys
    {
        public static readonly Journey Commute = new Journey
        {
            Outbound = new JourneyLeg
            {
                DepartureTime = new FuckingTime("7:40"),
                From = "KLF",
                To = "ILK",

                Monitor = new MonitorSettings()
                {
                    Days = "MON-FRI",
                    From =   new FuckingTime("6:20"),
                    To =   new FuckingTime("7:40"),
                    Every = 5,
                    Off =   new FuckingTime("7:42")

                }

            },
            Inbound = new JourneyLeg
            {
                DepartureTime = new FuckingTime("16:38"),
                From = "ILK",
                To = "KLF",
                Monitor = new MonitorSettings()
                {
                    Days = "MON-FRI",
                    From = new FuckingTime("16:15"),
                    To = new FuckingTime("16:38"),
                    Every = 5,
                    Off = new FuckingTime("16:40")

                }
     

            }
        };
    }
}
