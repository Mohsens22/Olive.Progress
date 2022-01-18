using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olive.Progress.Samples.Console
{
    static class Integration
    {
        public static void HandleForConsole(this ProgressWatcher watcher)
        {
            watcher.OnRegistered += Watcher_OnRegistered;
        }

        private static void Watcher_OnRegistered(ProgressWatcher sender, Guid e)
        {
            int totalTicks = sender.MaxTicks;

            var pbar = new ProgressBar(totalTicks, sender.Message);
            sender.OnTicked += (s, e) => {
                pbar.Tick(s.Message);
            };
            sender.OnMessageFired += (s, e,l) => 
            {
                pbar.WriteLine(l);
            };
            sender.OnLogFired += (s, e, l) =>
            {
                Debug.WriteLine(l);
            };
            sender.OnChildAdded += (s, e, i) =>
            {
                var childbar = pbar.Spawn(e.MaxTicks,e.Message);
                e.OnTicked += (s, e) =>
                {
                    childbar.Tick(s.Message);
                };
                e.OnFinished += (s, e) =>
                {
                    childbar.Dispose();
                };
            };
            sender.OnFinished += (s, e) =>
            {
                pbar.Tick(s.CurrentTick,s.Message);
                pbar.Dispose();
            };
        }
    }
}
