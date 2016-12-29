using System;
using System.Threading;
using BlinkStickDotNet;

namespace FuckingTrainsConsoleService
{
    internal class BlinkstickWrapper
    {
        private const int NumberOfLeds = 2;

        private const string StateChangedColour = "#ffffff";
        private const string OffColour = "#000000";

        private const string OnTimeColour = "#00ff00";
        private const string DelayedColour = "#ffff00";
        private const string CancelledColour = "#ff0000";

        private const string DontKnowColour = "#00ffff";
        private const string CrystallBallColour = "#ff00ff";
        private const string ServiceDown = "#0000ff";
        private const string NoTrainsColour = "#ff0000";
        private readonly BlinkStick _blinkstick = BlinkStick.FindFirst();

        public BlinkstickWrapper()
        {
            if (_blinkstick != null)
            {
                if (_blinkstick.OpenDevice())
                {
                    //Set mode to WS2812. Read more about modes here:
                    //http://www.blinkstick.com/help/tutorials/blinkstick-pro-modes
                    _blinkstick.SetMode(2);

                    Off();
                }
                else
                {
                    Console.WriteLine("Could not open the device");
                }
            }
            else
            {
                Console.WriteLine("BlinkStick not found");
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

        public void FuckingStateChanged()
        {
            Blink(StateChangedColour, 3);
        }

        public void Off()
        {
            Both(OffColour);
        }

        public void IDontFuckingKnow()
        {
            Both(DontKnowColour);
        }

        public void FuckingOnTimeApparently()
        {
            Both(OnTimeColour);
        }

        public void FuckingDelayed(int? delayInMinutes)
        {
            if (delayInMinutes.HasValue)
            {
                Blink(DelayedColour, delayInMinutes.Value);
            }
            Both(DelayedColour);
        }

        public void FuckingCancelled()
        {
            Both(CancelledColour);
        }

        public void YouNeedAFuckingCrystalBall()
        {
            Both(CrystallBallColour);
        }

        public void TheFuckingServiceIsDownOrSomething()
        {
            Both(ServiceDown);
        }

        public void NoFuckingTrains()
        {
            Both(NoTrainsColour);
        }
    }
}