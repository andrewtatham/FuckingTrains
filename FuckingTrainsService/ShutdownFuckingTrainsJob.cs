using FuckingTrainsLights;
using Quartz;

namespace FuckingTrainsService
{
    public class ShutdownFuckingTrainsJob : IJob
    {
        private static readonly BlinkstickWrapper blinkstick = BlinkstickWrapper.Instance();

        public void Execute(IJobExecutionContext context)
        {
            blinkstick.BlinkstickOff();
        }
    }
}