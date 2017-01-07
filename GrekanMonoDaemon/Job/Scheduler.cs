using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace GrekanMonoDaemon.Job
{
    public class Scheduler
    {
        private readonly List<Tuple<IJobDetail, TriggerBuilder>> _jobs;
        private readonly IScheduler _scheduler;

        public Scheduler()
        {
            _scheduler = new StdSchedulerFactory().GetScheduler();
            _jobs = new List<Tuple<IJobDetail, TriggerBuilder>>();
            SetupJobs();
        }

        private void SetupJobs()
        {
            _jobs.Add(new Tuple<IJobDetail, TriggerBuilder>(
                JobBuilder.Create<GedPublish>().Build(),
                TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.WithIntervalInHours(1)
                    .OnEveryDay())
            ));

            _jobs.Add(new Tuple<IJobDetail, TriggerBuilder>(
                JobBuilder.Create<GedSync>().Build(),
                TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(45).RepeatForever())
            ));
        }

        public void Engage()
        {
            _scheduler.Start();
            _jobs.ForEach(tuple =>
                Task.Run(() => _scheduler.ScheduleJob(tuple.Item1, tuple.Item2.ForJob(tuple.Item1).Build())));
        }
    }
}