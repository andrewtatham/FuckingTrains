using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuckingTrains;

namespace FuckingTrainsConsoleService
{
    class Program
    {
        private static readonly BlinkstickWrapper blinkstick = new BlinkstickWrapper();
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
            var train = Trains.IsMyFuckingTrainOnTime(Commute, false);
            Console.WriteLine(train.ToString());
            SetBlinkstickState(train);
            Console.ReadKey();

        }

        private static void SetBlinkstickState(TrainResult train)
        {
            

            if (train.IsTooFuckingFarInTheFuture)
            {
                Console.WriteLine("YOU NEED A FUCKING CRYSTAL BALL");
            }
            else if (train.TheFuckingServiceIsDownOrSomething)
            {
                Console.WriteLine("THE FUCKING SERVICE IS DOWN OR SOMETHING");
            }
            else if (train.NoFuckingServicesAvailable)
            {
                Console.WriteLine("NO FUCKING SERVICES ARE AVAILIABLE");
            }
            else if (train.isFuckingCancelled)
            {
                string cancelled = "FUCKING CANCELLED";
                if (!string.IsNullOrWhiteSpace(train.FuckingCancellationReason))
                {
                    cancelled += string.Format(" DUE TO {0}", train.FuckingCancellationReason);
                }
                Console.WriteLine(cancelled);
            }
            else if (train.isFuckingDelayed)
            {
                string delayed = "FUCKING DELAYED";
                if (!string.IsNullOrWhiteSpace(train.EstimatedTimeOfFuckingDeparture))
                {
                    delayed += string.Format(" ETD {0}", train.EstimatedTimeOfFuckingDeparture);
                }

                if (!string.IsNullOrWhiteSpace(train.FuckingDelayReason))
                {
                    delayed += string.Format(" DUE TO {0}", train.FuckingDelayReason);
                }
                Console.WriteLine(delayed);
            }
            else if (train.isOnTime)
            {
                Console.WriteLine("ON FUCKING TIME APPARENTLY");
            }
            else
            {
                Console.WriteLine("I DONT FUCKING KNOW");
            }

            if (!string.IsNullOrEmpty(train.FuckingPlatform))
            {
                Console.WriteLine(" FUCKING PLATFORM {0}", train.FuckingPlatform);
            }

            if (!string.IsNullOrEmpty(train.ShowerOfBastards))
            {
                Console.WriteLine(" (FUCKING {0})", train.ShowerOfBastards);
            }
            if (train.Messages != null)
            {
                foreach (var nrccMessage in train.Messages)
                {
                    Console.WriteLine(" * {0}", nrccMessage);
                }
            }
        }
    }
}
