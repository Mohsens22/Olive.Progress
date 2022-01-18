using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.Progress
{
    public class ProgressReport
    {
        public Guid Id { get; set; }
        public int MaxTicks { get; set; }
        public string Message { get; set; }

        public int CurrentTick { get; set; }
        public double Percentage { get; set; }

        public List<ProgressReport> Children { get; set; }

        public override string ToString() => $"{Percentage}% - {Message} ({CurrentTick}/{MaxTicks})";

    }
}
