﻿using System;
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
        private const int DATABASE_SCHEMA_VERSION = 3;

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

            var selectQuery = this.api.Database.NewSelectQuery(CommitEventEntity.GetPluginTableName(this.api.Database));
            selectQuery.AddSelect("*");
            selectQuery.AddWhere("ixBug = @ixBug");
            selectQuery.SetParamInt("@ixBug", ixBug);

            var commitEvents = new List<CPseudoBugEvent>();

            var ds = selectQuery.GetDataSet();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int ixCommitEvent = Convert.ToInt32(row["ixCommitEvent"]);
                    string sRevision = Convert.ToString(row["sRevision"]);
                    string sAuthor = Convert.ToString(row["sAuthor"]);
                    int ixPerson = Convert.ToInt32(row["ixPerson"]);
                    string sMessage = Convert.ToString(row["sMessage"]);
                    sMessage = sMessage.Replace("\n", "<br>\n");

                    DateTime dtCommit = Convert.ToDateTime(row["dtCommit"]);
                    DateTime dtCommitClient = this.api.TimeZone.CTZFromUTC(dtCommit);
                    string sDtCommit = this.api.TimeZone.DateTimeString(dtCommitClient);

                    //var sTitle = $"Commited revision {sRevision} by";
                    var sTitle = $"Revision {sRevision} commited by";
                    if (ixPerson == 0)
                    {
                        sTitle += " " + sAuthor;
                    }

                    var sHtml = this.api.UI.BugEvent(dtCommit, ixPerson, sTitle, sMessage, null, "fbee-commit");
                    var evt = new CPseudoBugEvent(dtCommit, sHtml);
                    commitEvents.Add(evt);
                }
            }

            return commitEvents.ToArray();
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
