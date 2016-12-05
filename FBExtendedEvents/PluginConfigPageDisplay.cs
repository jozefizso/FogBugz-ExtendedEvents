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
                  <p>Request new API token:</p>",
                PageDisplay.Headline("FogBugz Extended Events"));
        }

        protected string GetForm()
        {
            return String.Format(
                @"<p>Username: {0}</p>
                  <p>Password: {1}</p>
                  <p>Request token: {2}</p>
                  <p>New token value: <strong id=""lblToken""></strong></p>",
                Forms.TextInput("username", ""),
                Forms.TextInput("password", ""),
                Forms.SubmitButton("btnRequestToken", "Create API Token")
                );

        }
    }
}
