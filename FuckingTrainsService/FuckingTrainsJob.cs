using System;
using FuckingTrains;
using FuckingTrainsLights;
using Quartz;

namespace FuckingTrainsService
{
    public class FuckingTrainsJob : IJob
    {
        private static readonly EventLogHelper EventLogHelper = new EventLogHelper();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var train = Trains.IsMyFuckingTrainOnTime();
                EventLogHelper.WriteEntry(train.ToString());
                var blinkstick = BlinkstickWrapper.Instance();
                blinkstick.SetBlinkstickState(train);
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteException(ex);
                throw;
            }
        }
    }
}