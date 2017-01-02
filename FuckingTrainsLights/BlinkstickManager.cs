using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;
using BlinkStickDotNet;
using FuckingTrains;

namespace FuckingTrainsLights
{
    public class BlinkstickManager : ITrainStateLights
    {

        private static readonly BlinkstickManager _instance = new BlinkstickManager();
        private readonly BlinkstickWrapper _blinkstickNano;
        private readonly BlinkstickWrapper _blinkstickFlex;
        private readonly List<BlinkstickWrapper> _blinksticks; 

        public static BlinkstickManager Instance()
        {
            return _instance;
        }

        private BlinkstickManager()
        {
            _blinksticks = new List<BlinkstickWrapper>();
            _blinkstickNano = TryCreateBlinkstick("BS003518-3.0", 2);
            _blinkstickFlex = TryCreateBlinkstick("BS006639-3.1", 32);

        }

        private BlinkstickWrapper TryCreateBlinkstick(string serial, int n)
        {
            var blinkstick = BlinkStick.FindBySerial(serial);
            if (blinkstick != null)
            {
                blinkstick.SetLedCount((byte)n);
                BlinkstickWrapper wrapper = new BlinkstickWrapper(blinkstick, n);
                _blinksticks.Add(wrapper);
                return wrapper;

            }
            return null;
        }


        public void BlinkstickOff()
        {
            foreach (var blinkstick in _blinksticks)
            {
                blinkstick.BlinkstickOff();
            }
        }

        public void Hello()
        {
            foreach (var blinkstick in _blinksticks)
            {
                blinkstick.Hello();
            }
        }

        public void TestBlinkstickTrainStates()
        {
            foreach (var state in Enum.GetValues(typeof(FuckingTrainStates)))
            {
                var fuckingDelayInMinutes = (FuckingTrainStates)state == FuckingTrainStates.FuckingDelayed ? new Random().Next(1, 10) : (int?)null;
                var result = new TrainResult
                {
                    FuckingTrainState = (FuckingTrainStates)state,
                    FuckingDelayInMinutes = fuckingDelayInMinutes
                };
                foreach (var blinkstick in _blinksticks)
                {
                    blinkstick.SetBlinkstickState(result);
                }

                Thread.Sleep(500);
            }


        }

        public void SetBlinkstickState(TrainResult train)
        {
            foreach (var blinkstick in _blinksticks)
            {
                blinkstick.SetBlinkstickState(train);
            }
        }






    }
}