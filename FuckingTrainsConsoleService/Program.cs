using System;
using System.Threading;
using FuckingTrains;
using FuckingTrainsLights;

namespace FuckingTrainsConsoleService
{
    internal class Program
    {
        private static readonly BlinkstickWrapper blinkstick = BlinkstickWrapper.Instance();
        private static void Main(string[] args)
        {
            try
            {
                blinkstick.Hello();
                var train = Trains.IsMyFuckingTrainOnTime();
                Console.WriteLine(train.ToString());
                blinkstick.SetBlinkstickState(train);
                Thread.Sleep(5000);
                //Console.ReadKey();
            }
            finally
            {
                blinkstick.BlinkstickOff();
            }
        }
    }
}