using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Repository;
using Quartz;

namespace GrekanMonoDaemon.Job
{
    public class GedSync : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            await MemesRepository.Sync();
        }
    }
}