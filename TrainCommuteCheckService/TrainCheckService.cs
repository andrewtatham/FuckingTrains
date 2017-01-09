using System;
using System.Linq;
using System.ServiceProcess;
using Quartz;
using Quartz.Collection;
using Quartz.Impl;
using TrainCommuteCheck;
using TrainCommuteCheckLights;

namespace TrainCommuteCheckService
{
    public partial class TrainCommuteCheckService : ServiceBase
    {
        private static readonly BlinkstickManager Blinksticks = BlinkstickManager.Instance();
        private static readonly EventLogHelper EventLogHelper = new EventLogHelper();

        public TrainCommuteCheckService()
        {
            ServiceName = "TrainCommuteCheckService";
            CanHandlePowerEvent = true;
            CanShutdown = true;
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

                Schedule<TrainCheckJob>(scheduler, Trains.GetCrons());
                Schedule<ShutdownTrainsJob>(scheduler, Trains.GetOffCrons());
                Schedule<NotifyJob>(scheduler, Trains.GetNotificationCrons());

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
            }
        }


        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            try
            {

                switch (powerStatus)
                {
                    case PowerBroadcastStatus.QuerySuspend:
                        bool active = Trains.IsMonitoringActive();
                        if (active)
                        {
                            EventLogHelper.WriteEntry($"Not suspending, monitoring is active");
                        }
                        return !active;
                    case PowerBroadcastStatus.Suspend:
                        DateTime utc = Trains.WhenShouldIWakeUp().ToUniversalTime();
                        EventLogHelper.WriteEntry($"Waking up at {utc} utc");
                        WakeyWakey.WakeUpAt(utc);
                        break;
                }             
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteException(ex);
            }
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            try
            {
                Blinksticks.BlinkstickOff();
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteException(ex);
            }
            base.OnShutdown();
        }
    }
}