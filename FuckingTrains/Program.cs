using System;

namespace FuckingTrains
{
    class Program
    {
        private static readonly Journey Commute = new Journey()
        {
            Outbound = new JourneyLeg()
            {
                H = 7,
                M = 40,
                From = "KLF",
                To = "ILK"
            },
            Inbound = new JourneyLeg()
            {
                H = 16,
                M = 38,
                From = "ILK",
                To = "KLF"
            }
        };
        private static readonly Journey TestJourney = new Journey()
        {
            Outbound = new JourneyLeg()
            {
                H = 7,
                M = 40,
                From = "LDS",
                To = "MAN"
            },
            Inbound = new JourneyLeg()
            {
                H = 19,
                M = 15,
                From = "BDQ",
                To = "LDS"
            }
        };



        static void Main(string[] args)
        {
            //Trains.IsMyFuckingTrainOnTime(TestJourney, false);
            Trains.IsMyFuckingTrainOnTime(Commute, false);

            Console.ReadKey();
        }
    }
}
