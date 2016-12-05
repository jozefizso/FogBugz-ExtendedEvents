using System;
using System.Collections.Generic;
using System.Text;
using FogCreek.FogBugz.Plugins.Api;
using FogCreek.FogBugz.Plugins.Interfaces;
using FogCreek.FogBugz.UI;

namespace FBExtendedEvents
{
    public class PluginConfigPageDisplay : IPluginConfigPageDisplay
    {
        private readonly CPluginApi api;

        public PluginConfigPageDisplay(CPluginApi api)
        {
            this.api = api;
        }

        public string ConfigPageDisplay()
        {
            string sHTML = "";
            sHTML += GetHeader();
            sHTML += GetForm();
            return sHTML;
        }

        protected string GetHeader()
        {
            return String.Format(
                @"<p>{0}</p>
                  <p>Request token: {1}</p>",

                PageDisplay.Headline("FogBugz Extended Events"),
                Forms.SubmitButton("btnRequestToken", "Create API Token"));
        }

        protected string GetForm()
        {
            return "";
        }
    }
}
