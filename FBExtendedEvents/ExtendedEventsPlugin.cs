using System;
using System.Collections.Generic;
using System.Data;
using FogCreek.FogBugz;
using FogCreek.FogBugz.Plugins;
using FogCreek.FogBugz.Plugins.Api;
using FogCreek.FogBugz.Plugins.Entity;
using FogCreek.FogBugz.Plugins.Interfaces;

namespace FBExtendedEvents
{
    public class ExtendedEventsPlugin : Plugin, IPluginDatabase, IPluginPseudoBugEvent, IPluginRawPageDisplay, IPluginConfigPageDisplay, IPluginCSS, IPluginJS
    {
        private const int DATABASE_SCHEMA_VERSION = 4;

        public ExtendedEventsPlugin(CPluginApi api) : base(api)
        {
        }

        public int DatabaseSchemaVersion()
        {
            return DATABASE_SCHEMA_VERSION;
        }

        public CTable[] DatabaseSchema()
        {
            var tblCommitEvents = CommitEventEntity.TableDefinition(this.api.Database);
            var tblExtendedEvents = ExtendedEventEntity.TableDefinition(this.api.Database);

            return new[] { tblCommitEvents, tblExtendedEvents };
        }

        public void DatabaseUpgradeBefore(int ixVersionFrom, int ixVersionTo, CDatabaseUpgradeApi apiUpgrade)
        {
        }

        public void DatabaseUpgradeAfter(int ixVersionFrom, int ixVersionTo, CDatabaseUpgradeApi apiUpgrade)
        {
        }

        public CPseudoBugEvent[] PseudoBugEvents(CBug bug, CBugEvent[] rgBugEvent)
        {
            int ixBug = bug.ixBug;

            var query = ExtendedEventEntity.QueryEvents(this.api.Database, ixBug);

            var events = new List<CPseudoBugEvent>();

            foreach (var entity in query)
            {
                string sMessage = entity.sMessage;
                sMessage = sMessage.Replace("\n", "<br>\n");
                string sTitle = "";

                switch (entity.sEventType)
                {
                    case "commit":
                        sTitle = $"Revision {entity.sCommitRevision} commited by";
                        break;
                    case "build-success":
                        sTitle = $"Build {entity.sBuildName} successful";
                        break;
                    case "build-failure":
                        sTitle = $"Build {entity.sBuildName} failed";
                        break;
                    case "releasenote":
                        sTitle = $"Releasenotes message by";
                        break;
                }

                if (entity.ixPerson == 0 && String.IsNullOrEmpty(entity.sPersonName))
                {
                    sTitle += " " + entity.sPersonName;
                }

                var sHtml = this.api.UI.BugEvent(entity.dtEventUtc, entity.ixPerson, sTitle, sMessage, null, $"fbee-{entity.sEventType}");
                var evt = new CPseudoBugEvent(entity.dtEventUtc, sHtml);
                events.Add(evt);
            }

            return events.ToArray();
        }

        public string RawPageDisplay()
        {
            api.Response.ContentType = "application/json";
            var sAction = api.Request["sAction"];

            if (sAction == "commit")
            {
                var cc = new CommitCommand(this.api);
                var result = cc.Process();
                return $@"{{ ""ixCommitEvent"": {result} }}";
            }

            if (sAction == "event")
            {
                var cc = new CreateEventCommand(this.api);
                var result = cc.Process();
                return $@"{{ ""ixExtendedEvent"": {result} }}";
            }

            return null;
        }

        public PermissionLevel RawPageVisibility()
        {
            return PermissionLevel.Normal;
        }

        public CCSSInfo CSSInfo()
        {
            var css = new CCSSInfo();
            css.rgsStaticFiles = new[] { @"css\FBExtendedEvents.css" };
            return css;
        }

        public CJSInfo JSInfo()
        {
            var js = new CJSInfo();
            js.rgsStaticFiles = new[] { @"js\ExtendedEvents.js" };
            return js;
        }

        private PluginConfigPageDisplay configPage;

        public string ConfigPageDisplay()
        {
            if (this.configPage == null)
            {
                this.configPage = new PluginConfigPageDisplay(this.api);
            }

            return this.configPage.ConfigPageDisplay();
        }
    }
}
