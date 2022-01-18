namespace Olive.Progress
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public interface IProgressWatcher
	{
		Guid Id { get; set; }
		void Register(int maxTicks, string message, bool collapseChildren);
		ProgressWatcher Spawn(int maxTicks, string message,bool collapseChildren);

		void Tick(string message = null);
		void Tick(int newTickCount, string message = null);

		int MaxTicks { get; set; }
		string Message { get; set; }

		int CurrentTick { get; }

		List<ProgressWatcher> Children{ get; set; }

        void WriteLine(string message);

	}
}
