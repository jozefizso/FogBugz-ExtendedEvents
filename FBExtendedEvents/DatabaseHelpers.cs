﻿using System;
using System.Globalization;
using FogCreek.FogBugz.Plugins;
using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    public static class DatabaseHelpers
    {
        public static void SaveEntities(this CDatabaseApi db, params IDatabaseEntity[] entities)
        {
            foreach (var entity in entities)
            {
                entity.Save(db);
            }
        }

        public static int GetInt32(this CPluginRequest request, string name, int defaultValue)
        {
            var val = request[name];

            int value;
            if (int.TryParse(val, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }

            return defaultValue;
        }

        // bugfix for .NET 2.0
        // .NET 2.0 cannot parse ISO format from SVN log by default because it expects exactly 7 digits at microseconds
        // 2015-01-01T17:47:27.064563Z => will fail on .NET 2.0, works on newer .NET runtimes
        // 2015-01-01T17:47:27.0645630Z => will work on .NET 2.0
        // 
        // Supported formats:
        // 2015-01-01T17:47:27.0645630Z
        // 2015-01-01T17:47:27.064563Z
        // 2015-01-01T17:47:27Z
        // 2015-01-01T17:47:27.0000000+01:00
        private static string[] ROUNDTRIP_DATE_FORMATS =
        {
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ssK"
        };

        public static DateTime GetDateTime(this CPluginRequest request, string name, DateTime defaultValue)
        {
            var val = request[name];

            DateTime value;
            if (DateTime.TryParseExact(val, ROUNDTRIP_DATE_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out value))
            {
                return value;
            }

            return defaultValue;
        }

        public static string GetString(this CPluginRequest request, string name, string defaultValue = null)
        {
            var val = request[name];
            if (val == null)
            {
                return defaultValue;
            }

            return val;
        }
    }
}