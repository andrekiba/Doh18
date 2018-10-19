using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Doh18.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;

namespace Doh18.Base
{
    public static class AppCenterHelper
    {
        public static void TrackError(this Exception ex, IDictionary<string, string> properties = null, [CallerMemberName]string caller = "")
        {
            var baseProps = new Dictionary<string, string>
            {
                {"Email", Settings.Current.Email},
                {Constants.Where, caller},
                {"Connected", CrossConnectivity.Current.IsConnected.ToString()}
            };

            Crashes.TrackError(ex, baseProps.Merge(properties));
        }

        public static void TrackError(this string error, IDictionary<string, string> properties = null, [CallerMemberName]string caller = "")
        {
            var baseProps = new Dictionary<string, string>
            {
                {"Email", Settings.Current.Email},
                {Constants.Where, caller},
                {"Connected", CrossConnectivity.Current.IsConnected.ToString()}
            };

            Analytics.TrackEvent(error, baseProps.Merge(properties));
        }

        public static void TrackEvent(this string key, IDictionary<string, string> properties = null, [CallerMemberName]string caller = "")
        {
            var baseProps = new Dictionary<string, string>
            {
                {"Email", Settings.Current.Email},
                {Constants.Where, caller},
                {"Connected", CrossConnectivity.Current.IsConnected.ToString()}
            };

            Analytics.TrackEvent(key, baseProps.Merge(properties));
        }
    }
}
