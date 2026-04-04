using System.Collections.Generic;

namespace _Project.Scripts.Services.Analytics
{
    public interface IAnalyticsClient
    {
        void LogEvent(string eventName, Dictionary<string, object> parameters = null);
    }
}