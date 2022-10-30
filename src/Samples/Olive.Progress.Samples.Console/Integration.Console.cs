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
        static ProgressBar current;
        public static void HandleForConsole(this ProgressWatcher watcher)
        {
            watcher.OnRegistered += Watcher_OnRegistered;
        }

        private static void Watcher_OnRegistered(ProgressWatcher sender, Guid e)
        {
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Green,
                BackgroundColor = ConsoleColor.DarkGray,
            };

            int totalTicks = sender.MaxTicks;

            if (current != null)
                current.Dispose();

            var pbar = new ProgressBar(totalTicks, sender.Message, options);
            current = pbar;
            sender.OnTicked += (s, e) => {
                pbar.Tick(s.Message);
            };
            sender.OnMessageFired += (s, e, l) =>
            {
                pbar.WriteLine(l);
            };
            sender.OnLogFired += (s, e, l, level) =>
            {
#if DEBUG
                Debug.WriteLine(l);
#endif
#if RELEASE
                pbar.WriteLine(l);
#endif
            };
            sender.OnChildAdded += (s, e, i) =>
            {
                var childOptions = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.DarkGray,
                    ProgressCharacter = '─'
                };
                childOptions.CollapseWhenFinished = s.CollapseChildrenWhenDone;
                var childbar = pbar.Spawn(e.MaxTicks, e.Message, childOptions);
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
                pbar.Tick(s.CurrentTick, s.Message);
                pbar.Dispose();
            };
        }
    }
}
