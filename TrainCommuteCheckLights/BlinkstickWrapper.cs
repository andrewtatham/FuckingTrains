using System;
using System.Threading;
using BlinkStickDotNet;
using TrainCommuteCheck;

namespace TrainCommuteCheckLights
{
    class BlinkstickWrapper : ITrainStateLights
    {
        private readonly int _n;

        private const string StateChangedColour = "#101010";
        private const string OffColour = "#000000";

        private const string OnTimeColour = "#001000";
        private const string DelayedColour = "#101000";
        private const string CancelledColour = "#100000";
        private const string NoTrainsColour = "#100000";

        private const string DontKnowColour = "#001010";
        private const string CrystallBallColour = "#100010";
        private const string ServiceDown = "#000010";

        private const string HelloColour = "#101010";
        private const string NotifyColour = "#FFFFFF";

        private readonly BlinkStick _blinkstick;

        private static TrainStatus? _previousTrainState;


        public BlinkstickWrapper(BlinkStick blinkstick, int n)
        {
            _n = n;
            _blinkstick = blinkstick;

            if (_blinkstick != null)
            {
                if (_blinkstick.OpenDevice())
                {
                    //Set mode to WS2812. Read more about modes here:
                    //http://www.blinkstick.com/help/tutorials/blinkstick-pro-modes
                    _blinkstick.SetMode(2);

                    All(OffColour);
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
                All(hexColour);
                Thread.Sleep(durationMilliseconds);
                All(OffColour);
                Thread.Sleep(durationMilliseconds);
            }
        }

        private void All(string hexColour)
        {
            for (byte i = 0; i < _n; i++)
            {
                _blinkstick.SetColor(0, i, hexColour);
            }
        }


        public void BlinkstickOff()
        {
            All(OffColour);
            _previousTrainState = null;
            Thread.Sleep(100);
        }

        public void Hello()
        {
            Blink(HelloColour);
        }


        public void SetBlinkstickState(TrainResult train)
        {
            SetBlinkstickState(train.TrainState, train.DelayInMinutes);
        }

        private void SetBlinkstickState(TrainStatus status, int? delay)
        {
            if (!_previousTrainState.HasValue || _previousTrainState != status)
            {
                Blink(StateChangedColour);
            }

            _previousTrainState = status;

            switch (status)
            {
                case TrainStatus.Unknown:
                    All(DontKnowColour);
                    break;
                case TrainStatus.OnTime:
                    All(OnTimeColour);
                    break;
                case TrainStatus.Delayed:
                    if (delay.HasValue)
                    {
                        Blink(DelayedColour, delay.Value);
                    }
                    All(DelayedColour);
                    break;
                case TrainStatus.Cancelled:
                    All(CancelledColour);
                    break;
                case TrainStatus.TooFarAhead:
                    All(CrystallBallColour);
                    break;
                case TrainStatus.ServiceDown:
                    All(ServiceDown);
                    break;
                case TrainStatus.NoTrains:
                    All(NoTrainsColour);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Notify()
        {
            Blink(NotifyColour, 3);
            if (_previousTrainState != null)
            {
                SetBlinkstickState(_previousTrainState.Value, null);
            }

        }
    }
}