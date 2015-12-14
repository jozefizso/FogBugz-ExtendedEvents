using System;
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

        public static DateTime GetDateTime(this CPluginRequest request, string name, DateTime defaultValue)
        {
            var val = request[name];

            DateTime value;
            if (DateTime.TryParse(val, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out value))
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