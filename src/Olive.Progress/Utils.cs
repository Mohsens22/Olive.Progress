using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Olive.Progress
{
    public static class Utils
    {
        public static ProgressReport CreateReport(this ProgressWatcher watcher)
        {
            return new ProgressReport
            {
                Id=watcher.Id,
                Children=watcher.Children.Select(x=>x.CreateReport()).ToList(),
                CurrentTick=watcher.CurrentTick,
                MaxTicks=watcher.MaxTicks,
                Message=watcher.Message,
                Percentage=watcher.Percentage
            };
        }
    }
}
