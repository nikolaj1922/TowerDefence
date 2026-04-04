using System.Linq;
using System.Collections.Generic;
using Firebase.Analytics;

namespace _Project.Scripts.Services.Analytics.Firebase
{
    public class FirebaseAnalyticsClient : IAnalyticsClient
    {
        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            Parameter[] firebaseParams = null;

            if (parameters != null)
            {
                firebaseParams =
                    parameters
                        .Select(p => new Parameter(p.Key, p.Value.ToString()))
                        .ToArray();
            }

            FirebaseAnalytics.LogEvent(eventName, firebaseParams);
        }
    }
}