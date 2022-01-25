namespace Olive.Progress
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public partial class ProgressWatcher
    {
        public delegate void OnMessageFiredHandler(ProgressWatcher sender, Guid e, string line);
        public event OnMessageFiredHandler OnMessageFired;

        public delegate void OnLogFiredHandler(ProgressWatcher sender, Guid e, string line, LogLevel logLevel);
        public event OnLogFiredHandler OnLogFired;

        public void WriteLine(string message)
        {
            OnMessageFired?.Invoke(this, this.Id, message);
        }
        private void WriteLine(string message, ProgressWatcher child)
        {
            OnMessageFired?.Invoke(child, child.Id, message);
        }

        private void Log(string message,LogLevel logLevel)
        {
            OnLogFired?.Invoke(this, this.Id, message, logLevel);
        }
        
        private void Log(string message, ProgressWatcher child, LogLevel logLevel)
        {
            OnLogFired?.Invoke(child, child.Id, message, logLevel);
        }

        // Debug
        public void LogDebug(string message, ProgressWatcher child)
        {
            Log(message, child, LogLevel.Debug);
        }
        public void LogDebug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        // Trace
        public void LogTrace(string message, ProgressWatcher child)
        {
            Log(message, child, LogLevel.Trace);
        }
        public void LogTrace(string message)
        {
            Log(message, LogLevel.Trace);
        }

        // Information
        public void LogInformation(string message, ProgressWatcher child)
        {
            Log(message, child, LogLevel.Information);
        }
        public void LogInformation(string message)
        {
            Log(message, LogLevel.Information);
        }

        // Warning
        public void LogWarning(string message, ProgressWatcher child)
        {
            Log(message, child, LogLevel.Warning);
        }
        public void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        // Error
        public void LogError(string message, ProgressWatcher child)
        {
            Log(message, child, LogLevel.Error);
        }
        public void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }
    }
}
