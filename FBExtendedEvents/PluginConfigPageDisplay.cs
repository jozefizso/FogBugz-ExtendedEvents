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
            sHTML += @"<div class=""FBExtendedEvents"">";
            sHTML += GetHeader();
            sHTML += GetForm();
            sHTML += GetFooter();
            sHTML += @"</div>";
            
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
                FormsEx.PasswordInput("password", ""),
                Forms.SubmitButton("btnRequestToken", "Create API Token")
                );

        }

        protected string GetFooter()
        {
            return String.Format(
                @"<p>Handler URL: <code>{0}</code></p>",
                api.Url.BaseUrl() + UrlEx.PluginRawPageUrl("FBExtendedEvents@goit.io"));
        }
    }
}
