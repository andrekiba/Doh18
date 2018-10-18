﻿using System;
using System.Collections.Generic;
using Doh18.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;

namespace Doh18.Base
{
    public static class Ach //AppCenterHelper
    {
        public static void TrackError(Exception ex, IDictionary<string, string> properties = null)
        {
            var baseProps = new Dictionary<string, string>
            {
                {"Email", Settings.Current.Email},
                {"Connected", CrossConnectivity.Current.IsConnected.ToString()}
            };

            Crashes.TrackError(ex, baseProps.Merge(properties));
        }

        public static void TrackEvent(string key, IDictionary<string, string> properties = null)
        {
            var baseProps = new Dictionary<string, string>
            {
                {"Email", Settings.Current.Email},
                {"Connected", CrossConnectivity.Current.IsConnected.ToString()}
            };

            Analytics.TrackEvent(key, baseProps.Merge(properties));
        }
    }
}