using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace FBExtendedEvents
{
    public static class StringEx
    {
        public static bool IsNullOrWhiteSpace(String value)
        {
            if (value == null)
            {
                return true;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
