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
                H = 7,
                M = 40,
                From = "KLF",
                To = "ILK"
            },
            Inbound = new JourneyLeg
            {
                H = 16,
                M = 38,
                From = "ILK",
                To = "KLF"
            }
        };

        public static readonly Journey TestJourney = new Journey
        {
            Outbound = new JourneyLeg
            {
                H = 20,
                M = 12,
                From = "LDS",
                To = "MAN"
            },
            Inbound = new JourneyLeg
            {
                H = 19,
                M = 15,
                From = "BDQ",
                To = "LDS"
            }
        };
    }
}
