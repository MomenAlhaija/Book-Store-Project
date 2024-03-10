using Quartz.Impl;
using Quartz;
using Quartz;
using Quartz.Impl;

namespace BookStore.Sechadule
{
    public class QuartzConfig
    {
        public static async void Start()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<DailyCallCartJob>()
                .WithIdentity("dailyCallCartJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("dailyTrigger", "group1")
            .WithDailyTimeIntervalSchedule(x => x
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(13, 0)) 
                .WithIntervalInHours(24)
            )
            .Build();


            await scheduler.ScheduleJob(job, trigger);
        }
    }

}

