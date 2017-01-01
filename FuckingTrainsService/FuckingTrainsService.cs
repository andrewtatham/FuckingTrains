using System;
using System.Linq;
using System.ServiceProcess;
using FuckingTrainsLights;
using Quartz;
using Quartz.Collection;
using Quartz.Impl;

namespace FuckingTrainsService
{
    public partial class FuckingTrainsService : ServiceBase
    {
        private readonly string[] _morningJobCron =
        {
            "0 20-59/5 6 ? * MON-FRI",
            "0 0-40/5 7 ? * MON-FRI"
        }; // Every 5 mins 6:20 to 7:40 MON-FRI
        private const string EveningJobCron = "0 0-38/5 16 ? * MON-FRI"; // Every 5 mins 16:00 to 16:38 MON-FRI
        private const string MorningShutdownCron = "0 42 7 ? * MON-FRI"; // 07:42 MON-FRI
        private const string EveningshutdownCron = "0 40 16 ? * MON-FRI"; // 16:40 MON-FRI

        private const string TestCron = "0/15 * * ? * *";
        private const string TestShutdownCron = "30 4/5 * ? * MON-FRI";

        private static readonly BlinkstickWrapper Blinkstick = BlinkstickWrapper.Instance();
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


        private static void Schedule<TJob>(IScheduler scheduler, string cron) where TJob : IJob
        {
            var job = JobBuilder.Create<TJob>().Build();
            var trigger = TriggerBuilder.Create().StartNow().WithCronSchedule(cron).Build();
            scheduler.ScheduleJob(job, trigger);
        }


        protected override void OnStart(string[] args)
        {
            try
            {
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Clear();

                //Schedule<FuckingTrainsJob>(scheduler, TestCron);
                //Schedule<ShutdownFuckingTrainsJob>(scheduler, TestShutdownCron);

                Schedule<FuckingTrainsJob>(scheduler, _morningJobCron);
                Schedule<FuckingTrainsJob>(scheduler, EveningJobCron);
                Schedule<ShutdownFuckingTrainsJob>(scheduler, MorningShutdownCron);
                Schedule<ShutdownFuckingTrainsJob>(scheduler, EveningshutdownCron);

                Blinkstick.Hello();
                Blinkstick.BlinkstickOff();

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

                Blinkstick.BlinkstickOff();
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteException(ex);
                throw;
            }
        }
    }
}