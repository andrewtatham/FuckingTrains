using System;
using Quartz;
using TrainCommuteCheck;
using TrainCommuteCheckLights;

namespace TrainCommuteCheckService
{
    public class TrainCheckJob : IJob
    {
        private static readonly EventLogHelper EventLogHelper = new EventLogHelper();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var train = Trains.IsMyTrainOnTime();
                EventLogHelper.WriteEntry(train.ToString());
                var blinkstick = BlinkstickManager.Instance();
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