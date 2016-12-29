using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FuckingTrains;

namespace FuckingTrainsConsoleService
{
    class Program
    {
        private static FuckingTrainStates? previousFuckingTrainState;
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
                H = 20,
                M = 12,
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


        private static readonly Random r = new Random();

        static void Main(string[] args)
        {
            try
            {
                var train = Trains.IsMyFuckingTrainOnTime(TestJourney, false);
                //var train = Trains.IsMyFuckingTrainOnTime(Commute, false);

                //Console.WriteLine(train.ToString());
                SetBlinkstickState(train);
                Thread.Sleep(5000);

                //TestBlinkstick();
            }
            finally
            {
                BlinkstickOff();
            }
        }





        private static void TestBlinkstick()
        {
            foreach (var state in Enum.GetValues(typeof (FuckingTrainStates)))
            {
                Console.WriteLine(state.ToString());


                var result = new TrainResult()
                {
                    FuckingTrainState = (FuckingTrainStates) state,
                    FuckingDelayInMinutes =
                        ((FuckingTrainStates) state) == FuckingTrainStates.FuckingDelayed ? r.Next(1, 10) : (int?) null
                };
                SetBlinkstickState(result);
                Thread.Sleep(2000);
            }
        }

        private static void BlinkstickOff()
        {
            blinkstick.Off();
            previousFuckingTrainState = null;
            Thread.Sleep(2000);

        }

        private static void SetBlinkstickState(TrainResult train)
        {
            if (!previousFuckingTrainState.HasValue || previousFuckingTrainState != train.FuckingTrainState)
            {
                blinkstick.FuckingStateChanged();
            }

            previousFuckingTrainState = train.FuckingTrainState;

            switch (train.FuckingTrainState)
            {
                case FuckingTrainStates.IDontFuckingKnow:
                    blinkstick.IDontFuckingKnow();
                    break;
                case FuckingTrainStates.OnFuckingTimeApparently:
                    blinkstick.FuckingOnTimeApparently();
                    break;
                case FuckingTrainStates.FuckingDelayed:
                    blinkstick.FuckingDelayed(train.FuckingDelayInMinutes);
                    break;
                case FuckingTrainStates.FuckingCancelled:
                    blinkstick.FuckingCancelled();
                    break;
                case FuckingTrainStates.YouNeedAFuckingCrystalBall:
                    blinkstick.YouNeedAFuckingCrystalBall();
                    break;
                case FuckingTrainStates.TheFuckingServiceIsDownOrSomething:
                    blinkstick.TheFuckingServiceIsDownOrSomething();
                    break;
                case FuckingTrainStates.NoFuckingTrains:
                    blinkstick.NoFuckingTrains();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
