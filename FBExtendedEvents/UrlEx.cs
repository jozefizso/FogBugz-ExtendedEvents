using System;
using System.Collections.Generic;
using System.Text;

namespace FBExtendedEvents
{
    public static class UrlEx
    {
        public static string PluginRawPageUrl(string sPluginId)
        {
            return "default.asp?pg=pgPluginRaw&sPlugin=" + Uri.EscapeDataString(sPluginId);
        }
    }
}
