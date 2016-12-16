using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FogCreek.FogBugz;
using FogCreek.FogBugz.Plugins;
using FogCreek.FogBugz.Plugins.Api;
using FogCreek.FogBugz.Plugins.Entity;
using FogCreek.FogBugz.Plugins.Interfaces;
using Vereyon.Web;

namespace FBExtendedEvents
{
    public class ExtendedEventsPlugin : Plugin, IPluginDatabase, IPluginPseudoBugEvent, IPluginRawPageDisplay, IPluginConfigPageDisplay, IPluginCSS, IPluginJS
    {
        private const int DATABASE_SCHEMA_VERSION = 9;

        private const int DB_CHANGE_VERSION_EVENTUTC = 5;
        private const int DB_CHANGE_VERSION_MESSAGES_HTML_SANITIZE = 7;

        public ExtendedEventsPlugin(CPluginApi api) : base(api)
        {
        }

        public int DatabaseSchemaVersion()
        {
            return DATABASE_SCHEMA_VERSION;
        }

        public CTable[] DatabaseSchema()
        {
            var tblExtendedEvents = ExtendedEventEntity.TableDefinition(this.api.Database);

            return new[] { tblExtendedEvents };
        }

        public void DatabaseUpgradeBefore(int ixVersionFrom, int ixVersionTo, CDatabaseUpgradeApi apiUpgrade)
        {
            if (ixVersionFrom <= DB_CHANGE_VERSION_EVENTUTC)
            {
                apiUpgrade.ChangeColumnDefinition(ExtendedEventEntity.GetPluginTableName(this.api.Database), "dtEventUtc");
            }
        }

        public void DatabaseUpgradeAfter(int ixVersionFrom, int ixVersionTo, CDatabaseUpgradeApi apiUpgrade)
        {
            if (ixVersionFrom <= DB_CHANGE_VERSION_MESSAGES_HTML_SANITIZE)
            {
                var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

                var selectQuery = this.api.Database.NewSelectQuery(ExtendedEventEntity.GetPluginTableName(this.api.Database));
                selectQuery.AddSelect("ixExtendedEvent, sMessage");
                selectQuery.AddWhere("sEventType <> @sEventType");
                selectQuery.SetParamString("@sEventType", "commit");

                var ds = selectQuery.GetDataSet();
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var sMessage = row["sMessage"] as string;

                        if (!String.IsNullOrEmpty(sMessage))
                        {
                            sMessage = sanitizer.Sanitize(sMessage);

                            var ixExtendedEvent = Convert.ToInt32(row["ixExtendedEvent"]);

                            var updateQuery = this.api.Database.NewUpdateQuery(ExtendedEventEntity.GetPluginTableName(this.api.Database));
                            updateQuery.UpdateString("sMessage", sMessage);
                            updateQuery.AddWhere("ixExtendedEvent = @ixExtendedEvent");
                            updateQuery.SetParamInt("@ixExtendedEvent", ixExtendedEvent);

                            updateQuery.Execute();
                        }
                    }
                }
            }
        }

        public CPseudoBugEvent[] PseudoBugEvents(CBug bug, CBugEvent[] rgBugEvent)
        {
            int ixBug = bug.ixBug;

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

            var query = ExtendedEventEntity.QueryEvents(this.api.Database, ixBug);

            var events = new List<CPseudoBugEvent>();

            foreach (var entity in query)
            {
                string sMessage = entity.sMessage;
                string sTitle = "";

                switch (entity.sEventType)
                {
                    case "commit":
                        sTitle = $"Revision {entity.sCommitRevision} commited by";
                        sMessage = HttpUtility.HtmlEncode(sMessage);
                        sMessage = sMessage.Replace("\n", "<br>\n");
                        break;
                    case "build-success":
                        sTitle = $"Build {entity.sBuildName} successful";
                        sMessage = sanitizer.Sanitize(sMessage);
                        break;
                    case "build-failure":
                        sTitle = $"Build {entity.sBuildName} failed";
                        sMessage = sanitizer.Sanitize(sMessage);
                        break;
                    case "releasenote":
                        sTitle = $"Releasenotes message by";
                        sMessage = sanitizer.Sanitize(sMessage);
                        break;
                    default:
                        sMessage = HttpUtility.HtmlEncode(sMessage);
                        break;
                }

                if (entity.ixPerson == 0 && String.IsNullOrEmpty(entity.sPersonName))
                {
                    sTitle += " " + entity.sPersonName;
                }

                string sChanges = null;
                if (!String.IsNullOrEmpty(entity.sExternalUrl))
                {
                    sChanges = $@"<a href=""{HttpUtility.HtmlAttributeEncode(entity.sExternalUrl)}"">View details</a>";
                }

                var sHtml = this.api.UI.BugEvent(entity.dtEventUtc, entity.ixPerson, sTitle, sMessage, sChanges, $"fbee-{entity.sEventType}");
                var evt = new CPseudoBugEvent(entity.dtEventUtc, sHtml);
                events.Add(evt);
            }

            return events.ToArray();
        }

        public string RawPageDisplay()
        {
            api.Response.ContentType = "application/json";
            var sAction = api.Request["sAction"];

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
