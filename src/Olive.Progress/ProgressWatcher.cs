namespace Olive.Progress
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public partial class ProgressWatcher : IProgressWatcher
    {
        protected readonly DateTime _startDate = DateTime.Now;
        private int _maxTicks;
        private int _currentTick;
        private string _message;
        private TimeSpan _estimatedDuration;

        public delegate void OnRegisteredHandler(ProgressWatcher sender, Guid e);
        public event OnRegisteredHandler OnRegistered;

        public delegate void OnTickedHadler(ProgressWatcher sender, int e);
        public event OnTickedHadler OnTicked;

        public delegate void OnFinishedHandler(ProgressWatcher sender, Guid e);
        public event OnFinishedHandler OnFinished;


        public delegate void ChildHandler(ProgressWatcher sender, ProgressWatcher child, Guid childId);
        public event ChildHandler OnChildAdded;
        public event ChildHandler OnChildRemoved;

        public ProgressWatcher()
        {
            
            this.Id= Guid.NewGuid();
            this.Children= new List<ProgressWatcher>();
        }
        public void Register(int maxTicks,string message, bool collapseChildren=true)
        {
            this._maxTicks = Math.Max(0, maxTicks);
            this._message = message;
            OnRegistered?.Invoke(this,this.Id);
            CollapseChildrenWhenDone= collapseChildren;
        }
        public Guid Id { get; set; }
        public int CurrentTick => _currentTick;
        public bool CollapseChildrenWhenDone { get; private set; }
        public int MaxTicks
        {
            get => _maxTicks;
            set
            {
                Interlocked.Exchange(ref _maxTicks, value);
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                Interlocked.Exchange(ref _message, value);
            }
        }

        public TimeSpan EstimatedDuration
        {
            get => _estimatedDuration;
            set
            {
                _estimatedDuration = value;
            }
        }

        public double Percentage
        {
            get
            {
                var percentage = Math.Max(0, Math.Min(100, (100.0 / this._maxTicks) * this._currentTick));
                // Gracefully handle if the percentage is NaN due to division by 0
                if (double.IsNaN(percentage) || percentage < 0) percentage = 100;

                percentage = Math.Round(percentage,2);
                return percentage;
            }
        }
        public DateTime? EndTime { get; protected set; }

        public List<ProgressWatcher> Children { get; set; }


        void OnDone()
        {
            OnFinished?.Invoke(this,this.Id);
        }

        public ProgressWatcher Spawn(int maxTicks, string message, bool collapseChildren = true)
        {
            var pbar = new ProgressWatcher();
            pbar.Register(maxTicks,message,collapseChildren);
            //OnChildAttached and Detached
            this.Children.Add(pbar);
            OnChildAdded?.Invoke(this,pbar,pbar.Id);

            pbar.OnMessageFired += Pbar_OnMessageFired;
            pbar.OnLogFired += Pbar_OnLogFired;

            pbar.OnFinished += (s, e) =>
            {
                if (CollapseChildrenWhenDone)
                {
                    this.Children.Remove(pbar);
                }
                this.OnChildRemoved?.Invoke(this, pbar, pbar.Id);
                pbar.OnMessageFired -= Pbar_OnMessageFired;
                pbar.OnLogFired -= Pbar_OnLogFired;
            };
            return pbar;
        }

        private void Pbar_OnLogFired(ProgressWatcher sender, Guid e, string line, LogLevel logLevel)
        {
            Log(line, sender, logLevel);
        }

        private void Pbar_OnMessageFired(ProgressWatcher sender, Guid e, string line)
        {
            WriteLine(line,sender);
        }

        public void Tick(string message = null)
        {
            Interlocked.Increment(ref _currentTick);
            FinishTick(message);
        }

        public void Tick(int newTickCount, string message = null)
        {
            Interlocked.Exchange(ref _currentTick, newTickCount);
            FinishTick(message);
        }
        private void FinishTick(string message)
        {
            if (message != null)
                Interlocked.Exchange(ref _message, message);

            if (_currentTick >= _maxTicks)
            {
                this.EndTime = DateTime.Now;
                this.OnDone();
            }
            OnTicked?.Invoke(this,this.CurrentTick);
        }
        

        public override string ToString()=> $"{Percentage}% - {Message} ({CurrentTick}/{MaxTicks})";
    }
}
