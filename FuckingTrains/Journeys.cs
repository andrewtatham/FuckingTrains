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
                Time = "7:40",
                From = "KLF",
                To = "ILK"
            },
            Inbound = new JourneyLeg
            {
                Time="16:38",
                From = "ILK",
                To = "KLF"
            }
        };

        public static readonly Journey TestJourney = new Journey
        {
            Outbound = new JourneyLeg
            {
                Time = "7:40",
                From = "LDS",
                To = "KGX"
            },
            Inbound = new JourneyLeg
            {
                Time = "17:40",
                From = "BDQ",
                To = "LDS"
            }
        };
    }
}
