using System;
using System.Threading.Tasks;
using GrekanMonoDaemon.Vk;
using Quartz;

namespace GrekanMonoDaemon.Job
{
    public class GedPublish : IJob
    {
        private readonly GedPublisher _publisher;

        public GedPublish()
        {
            _publisher = new GedPublisher();
        }

        public void Execute(IJobExecutionContext context)
        {
            _publisher.Execute();
        }
    }
}