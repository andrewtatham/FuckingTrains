using FuckingTrainsLights;
using Quartz;

namespace FuckingTrainsService
{
    public class ShutdownFuckingTrainsJob : IJob
    {
        private static readonly BlinkstickManager blinkstick = BlinkstickManager.Instance();

        public void Execute(IJobExecutionContext context)
        {
            blinkstick.BlinkstickOff();
        }
    }
}