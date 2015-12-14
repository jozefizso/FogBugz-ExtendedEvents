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
    public class ExtendedEventsPlugin : Plugin, IPluginDatabase, IPluginPseudoBugEvent, IPluginRawPageDisplay
    {
        private const int DATABASE_SCHEMA_VERSION = 1;

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

            return new[] { tblCommitEvents };
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
                    DateTime dtCommit = Convert.ToDateTime(row["dtCommit"]);
                    DateTime dtCommitClient = this.api.TimeZone.CTZFromUTC(dtCommit);
                    string sDtCommit = this.api.TimeZone.DateTimeString(dtCommitClient);

                    var sHtml = $@"<div id=""bugevent_{ixCommitEvent}_commit"" class=""bugevent detailed"" style=""background-color: #dff0f7;"">
                                     <div id=""bugeventSummary_{ixCommitEvent}_pe"" class=""summary"">
                                       <span class=""action"">Commit r{sRevision} by {sAuthor}</span>&nbsp;" +
                                    $@"<span class=""date"" dir=""ltr"">{sDtCommit}</a></span>
                                     </div>
                                     <div id=""bugeventChanges_{ixCommitEvent}_pe"" class=""changes"" dir=""ltr""></div>
                                   </div>";

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
    }
}
