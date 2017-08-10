﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

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
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(35).RepeatForever())
            ));
            
            _jobs.Add(new Tuple<IJobDetail, TriggerBuilder>(
                JobBuilder.Create<GrekileaksPublish>().Build(),
                TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(x => x.OnEveryDay()
                        .StartingDailyAt(new TimeOfDay(1, 0))
                        .EndingDailyAfterCount(1))
            ));

            _jobs.Add(new Tuple<IJobDetail, TriggerBuilder>(
                JobBuilder.Create<GedSync>().Build(),
                TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(12).RepeatForever())
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