using System;
using System.Threading;
using FuckingTrains;

namespace FuckingTrainsConsoleService
{
    internal class Program
    {
        private static FuckingTrainStates? previousFuckingTrainState;
        private static readonly BlinkstickWrapper blinkstick = new BlinkstickWrapper();
        private static readonly Random r = new Random();

        private static void Main(string[] args)
        {
            try
            {
                //var train = Trains.IsMyFuckingTrainOnTime(Journeys.TestJourney, false);
                var train = Trains.IsMyFuckingTrainOnTime(Journeys.Commute, false);

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


                var result = new TrainResult
                {
                    FuckingTrainState = (FuckingTrainStates) state,
                    FuckingDelayInMinutes =
                        (FuckingTrainStates) state == FuckingTrainStates.FuckingDelayed ? r.Next(1, 10) : (int?) null
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