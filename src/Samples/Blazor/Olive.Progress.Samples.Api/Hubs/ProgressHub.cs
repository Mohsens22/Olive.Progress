using Microsoft.AspNetCore.SignalR;
using Olive.Progress;
using System.Diagnostics;
namespace Olive.Progress.Samples.Api
{
    public class ProgressHub : Hub
    {
        ILogger _logger;
        public ProgressHub(ILogger<ProgressHub> logger)
        {
            _logger = logger;
        }
        internal static List<KeyValuePair<string, ProgressWatcher>> RunningServices = new List<KeyValuePair<string, ProgressWatcher>>();
        public async Task StartTask(string taskName)
        {
            if (taskName.Equals("RunSingle", StringComparison.OrdinalIgnoreCase))
            {
                if (!RunningServices.Exists(x => x.Key.Equals(taskName, StringComparison.OrdinalIgnoreCase)))
                {
                    var watcher = new ProgressWatcher();
                    ConfigureForSignalR(watcher, taskName);

                    await TestCases.RunSimpleTask(watcher);
                }
            }
            else if (taskName.Equals("RunWithChild", StringComparison.OrdinalIgnoreCase))
            {
                if (!RunningServices.Exists(x => x.Key.Equals(taskName, StringComparison.OrdinalIgnoreCase)))
                {
                    var watcher = new ProgressWatcher();
                    ConfigureForSignalR(watcher, taskName);

                    await TestCases.RunTaskWithChildren(watcher);
                }
            }
            else if (taskName.Equals("RunWithChildStatic", StringComparison.OrdinalIgnoreCase))
            {
                if (!RunningServices.Exists(x => x.Key.Equals(taskName, StringComparison.OrdinalIgnoreCase)))
                {
                    var watcher = new ProgressWatcher();
                    ConfigureForSignalR(watcher, taskName);

                    await TestCases.RunTaskWithStaticChildren(watcher);
                }
            }
        }
        public async IAsyncEnumerable<Dictionary<string, Guid>> StreamRunningServices(CancellationToken cancellationToken)
        {
            while (true)
            {
                var r = RunningServices.ToDictionary(x => x.Key, y => y.Value.Id);
                yield return r;
                await Task.Delay(1000, cancellationToken);
            }
        }
        public async IAsyncEnumerable<ProgressReport> Runnin(Guid id, CancellationToken cancellationToken)
        {
            while (true)
            {
                var r = RunningServices.Where(x => x.Value.Id == id).Select(x => x.Value).FirstOrDefault();
                if (r != null)
                {
                    yield return r.CreateReport();
                }
                else { yield return null; }
                await Task.Delay(1000, cancellationToken);
            }
        }
        async Task WriteLine(Guid id, string message)
        {
            await Clients.All.SendAsync("writeLine", id, message);
        }
        async Task WriteError(Guid id, string message)
        {
            await Clients.All.SendAsync("writeError", id, message);
        }
        private void ConfigureForSignalR(ProgressWatcher watcher, string task)
        {
            watcher.OnRegistered += (s, e) =>
            {
                RunningServices.Add(new KeyValuePair<string, ProgressWatcher>(task, s));
                Console.WriteLine();
                Console.WriteLine(e + " Added into registration " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            };
            watcher.OnFinished += async (s, e) =>
            {
                Console.WriteLine(e + " finished at " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                // wait for clients to understand it's done
                await Task.Delay(5000);
                RunningServices.RemoveAll(x => x.Value.Id == e);
            };

            watcher.OnMessageFired += async (s, e, l) =>
            {
                await WriteLine(e, l);
                Console.WriteLine(e + " " + l);
            };
            watcher.OnLogFired += async (s, e, l,logtype) =>
            {
                await WriteError(e, l);
                
                _logger.Log(logtype,l,e);
            };

        }
    }
}
