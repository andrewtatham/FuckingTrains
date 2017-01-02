using System;
using System.Linq;
using System.ServiceProcess;
using FuckingTrains;
using FuckingTrainsLights;
using Quartz;
using Quartz.Collection;
using Quartz.Impl;

namespace FuckingTrainsService
{
    public partial class FuckingTrainsService : ServiceBase
    {
        private static readonly BlinkstickManager Blinksticks = BlinkstickManager.Instance();
        private static readonly EventLogHelper EventLogHelper = new EventLogHelper();

        public FuckingTrainsService()
        {
            ServiceName = "FuckingTrainsService";
            InitializeComponent();
        }

        private static void Schedule<TJob>(IScheduler scheduler, string[] crons) where TJob : IJob
        {
            var triggers =
                new HashSet<ITrigger>(
                    crons.Select(cron => TriggerBuilder.Create().StartNow().WithCronSchedule(cron).Build()));
            var job = JobBuilder.Create<TJob>().Build();
            scheduler.ScheduleJob(job, triggers, true);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Clear();

                Schedule<FuckingTrainsJob>(scheduler, Trains.GetCrons());
                Schedule<ShutdownFuckingTrainsJob>(scheduler, Trains.GetOffCrons());

                Blinksticks.Hello();
                Blinksticks.BlinkstickOff();

                scheduler.Start();
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteException(ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Shutdown();

                Blinksticks.BlinkstickOff();
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteException(ex);
                throw;
            }
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            EventLogHelper.WriteEntry(powerStatus.ToString());
            switch (powerStatus)
            {
                case PowerBroadcastStatus.Suspend:
                    DateTime utc = Trains.WhenShouldIWakeUp().ToUniversalTime();
                    EventLogHelper.WriteEntry($"Waking up at {utc} utc");
                    WakeyWakey.WakeUpAt(utc);
                    break;
            }
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            Blinksticks.BlinkstickOff();
            base.OnShutdown();
        }
    }
}