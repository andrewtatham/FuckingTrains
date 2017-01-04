using Quartz;
using TrainCommuteCheckLights;

namespace TrainCommuteCheckService
{
    public class ShutdownTrainsJob : IJob
    {
        private static readonly BlinkstickManager blinkstick = BlinkstickManager.Instance();

        public void Execute(IJobExecutionContext context)
        {
            blinkstick.BlinkstickOff();
        }
    }
}