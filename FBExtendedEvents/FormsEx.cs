using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using FogCreek.Core;
using FogCreek.FogBugz.UI;

namespace FBExtendedEvents
{
    public static class FormsEx
    {
        public static string PasswordInput(string sInputName, string sInputValue, [Optional] IDictionary dictAttr)
        {
            var dict = CopyDictionary(dictAttr);

            FormsEx.PrependClass("wide", dict);
            FormsEx.PrependClass("dlgText", dict);
            return Input("password", sInputName, sInputValue, dict);
        }

        public static string Input(string sType, string sInputName, string sInputValue, [Optional] IDictionary dictAttr)
        {
            var dict = CopyDictionary(dictAttr);

            dict["type"] = sType;
            dict["name"] = sInputName;
            dict["value"] = sInputValue;

            return Html.RenderTag("input", dict, sContent: null);
        }

        private static void PrependClass(string sNewClass, IDictionary dictAttr)
        {
            if (!dictAttr.Contains("class"))
            {
                dictAttr["class"] = sNewClass;
            }
            else
            {
                dictAttr["class"] = sNewClass + " " + dictAttr["class"];
            }
        }

        private static Hashtable CopyDictionary(IDictionary dict)
        {
            Hashtable ht;
            if (dict == null)
            {
                ht = new Hashtable();
            }
            else
            {
                ht = new Hashtable(dict);
            }

            return ht;
        }
    }
}
