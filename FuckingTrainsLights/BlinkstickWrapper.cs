using System;
using System.CodeDom;
using System.Threading;
using BlinkStickDotNet;
using FuckingTrains;

namespace FuckingTrainsLights
{
    public class BlinkstickWrapper
    {
        private static FuckingTrainStates? previousFuckingTrainState;
        private static readonly Random RandomyRandom = new Random();

        private const int NumberOfLeds = 2;

        private const string StateChangedColour = "#ffffff";
        private const string OffColour = "#000000";

        private const string OnTimeColour = "#00ff00";
        private const string DelayedColour = "#ffff00";
        private const string CancelledColour = "#ff0000";
        private const string NoTrainsColour = "#ff0000";

        private const string DontKnowColour = "#00ffff";
        private const string CrystallBallColour = "#ff00ff";
        private const string ServiceDown = "#0000ff";

        private readonly BlinkStick _blinkstick = BlinkStick.FindFirst();

        private static readonly BlinkstickWrapper _instance = new BlinkstickWrapper();

        public static BlinkstickWrapper Instance()
        {
            return _instance;
        }

        private BlinkstickWrapper()
        {
            if (_blinkstick != null)
            {
                if (_blinkstick.OpenDevice())
                {
                    //Set mode to WS2812. Read more about modes here:
                    //http://www.blinkstick.com/help/tutorials/blinkstick-pro-modes
                    _blinkstick.SetMode(2);

                    Both(OffColour);
                }
                else
                {
                    throw new Exception("Could not open the blinkstick device");
                }
            }
            else
            {
                throw new Exception("BlinkStick not found");
            }
        }

        private void Blink(string hexColour, int n = 1, int durationMilliseconds = 100)
        {
            for (var i = 0; i < n; i++)
            {
                Both(hexColour);
                Thread.Sleep(durationMilliseconds);
                Both(OffColour);
                Thread.Sleep(durationMilliseconds);
            }
        }

        private void Both(string hexColour)
        {
            for (byte i = 0; i < NumberOfLeds; i++)
            {
                _blinkstick.SetColor(0, i, hexColour);
            }
        }


        public void TestBlinkstickTrainStates()
        {
            foreach (var state in Enum.GetValues(typeof(FuckingTrainStates)))
            {
                var result = new TrainResult
                {
                    FuckingTrainState = (FuckingTrainStates)state,
                    FuckingDelayInMinutes =
                        (FuckingTrainStates)state == FuckingTrainStates.FuckingDelayed ? RandomyRandom.Next(1, 10) : (int?)null
                };
                SetBlinkstickState(result);
                Thread.Sleep(500);
            }
        }

        public void BlinkstickOff()
        {
            Both(OffColour);
            previousFuckingTrainState = null;
            Thread.Sleep(100);
        }

        public void Hello()
        {
            Blink("#ff0000");
            Blink("#ffff00");
            Blink("#00ff00");
            Blink("#00ffff");
            Blink("#0000ff");
            Blink("#ff00ff");
        }
        public void SetBlinkstickState(TrainResult train)
        {
            if (!previousFuckingTrainState.HasValue || previousFuckingTrainState != train.FuckingTrainState)
            {
                Blink(StateChangedColour);
            }

            previousFuckingTrainState = train.FuckingTrainState;

            switch (train.FuckingTrainState)
            {
                case FuckingTrainStates.IDontFuckingKnow:
                    Both(DontKnowColour);
                    break;
                case FuckingTrainStates.OnFuckingTimeApparently:
                    Both(OnTimeColour);
                    break;
                case FuckingTrainStates.FuckingDelayed:
                    int? delayInMinutes = train.FuckingDelayInMinutes;
                    if (delayInMinutes.HasValue)
                    {
                        Blink(DelayedColour, delayInMinutes.Value);
                    }
                    Both(DelayedColour);
                    break;
                case FuckingTrainStates.FuckingCancelled:
                    Both(CancelledColour);
                    break;
                case FuckingTrainStates.YouNeedAFuckingCrystalBall:
                    Both(CrystallBallColour);
                    break;
                case FuckingTrainStates.TheFuckingServiceIsDownOrSomething:
                    Both(ServiceDown);
                    break;
                case FuckingTrainStates.NoFuckingTrains:
                    Both(NoTrainsColour);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



    }
}