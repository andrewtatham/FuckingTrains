using System;
using System.Threading;
using TrainCommuteCheck;
using TrainCommuteCheckLights;

namespace TrainCommuteCheckConsole
{
    internal class Program
    {
        private static readonly BlinkstickManager blinkstick = BlinkstickManager.Instance();
        private static void Main(string[] args)
        {
            try
            {
                blinkstick.Hello();
                var train = Trains.IsMyTrainOnTime();
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