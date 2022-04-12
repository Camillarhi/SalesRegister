using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Controllers.Schedulers
{
    public class RemindersJob : IJob
    {
        private readonly IServiceProvider _provider;
        public RemindersJob(IServiceProvider provider)
        {
            _provider = provider;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

            return Task.CompletedTask;
        }
        public void Logs(string message)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Quartz");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "Logs.txt");
            using FileStream fstream = new FileStream(path, FileMode.Create);
            using TextWriter writer = new StreamWriter(fstream);
            writer.WriteLine(message);
        }
    }

}
