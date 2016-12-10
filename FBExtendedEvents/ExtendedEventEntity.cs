using System;
using System.Collections.Generic;
using System.Data;
using FogCreek.FogBugz;
using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    public class ExtendedEventEntity
    {
        private const string TABLE_EXTENDED_EVENTS_NAME = "ExtendedEvents";

        public int ixExtendedEvent { get; set; }

        public int ixBug { get; set; }

        public string sEventType { get; set; }

        public DateTime dtEventUtc { get; set; }

        public int ixPerson { get; set; }

        public string sPersonName { get; set; }

        public string sMessage { get; set; }

        public string sExternalUrl { get; set; }

        public string sCommitRevision { get; set; }

        public string sBuildName { get; set; }

        public int Save(CDatabaseApi db)
        {
            var qInsert = db.NewInsertQuery(GetPluginTableName(db));

            qInsert.InsertInt("ixBug", this.ixBug);
            qInsert.InsertString("sEventType", this.sEventType);
            qInsert.InsertDate("dtEventUtc", this.dtEventUtc);
            qInsert.InsertInt("ixPerson", this.ixPerson);
            qInsert.InsertString("sPersonName", this.sPersonName);
            qInsert.InsertString("sMessage", this.sMessage);
            qInsert.InsertString("sExternalUrl", this.sExternalUrl);
            qInsert.InsertString("sCommitRevision", this.sCommitRevision);
            qInsert.InsertString("sBuildName", this.sBuildName);

            return qInsert.Execute();
        }

        public void Load(DataRow row)
        {
            this.ixExtendedEvent = Convert.ToInt32(row["ixExtendedEvent"]);
            this.ixBug = Convert.ToInt32(row["ixBug"]);
            this.sEventType = Convert.ToString(row["sEventType"]);
            this.dtEventUtc = Convert.ToDateTime(row["dtEventUtc"]);
            this.ixPerson = Convert.ToInt32(row["ixPerson"]);
            this.sPersonName = Convert.ToString(row["sPersonName"]);
            this.sMessage = Convert.ToString(row["sMessage"]);
            this.sExternalUrl = Convert.ToString(row["sExternalUrl"]);
            this.sCommitRevision = Convert.ToString(row["sCommitRevision"]);
            this.sBuildName = Convert.ToString(row["sBuildName"]);
        }

        public static IEnumerable<ExtendedEventEntity> QueryEvents(CDatabaseApi db, int ixBug)
        {
            var selectQuery = db.NewSelectQuery(ExtendedEventEntity.GetPluginTableName(db));
            selectQuery.AddSelect("*");
            selectQuery.AddWhere("ixBug = @ixBug");
            selectQuery.SetParamInt("@ixBug", ixBug);


            var ds = selectQuery.GetDataSet();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var entity = new ExtendedEventEntity();
                    entity.Load(row);

                    yield return entity;
                }
            }
        }

        public static CTable TableDefinition(CDatabaseApi db)
        {
            var tblExtendedEventsName = GetPluginTableName(db);
            var tblExtendedEvents = db.NewTable(tblExtendedEventsName);

            tblExtendedEvents.sDesc = "List of extended events from external systems integrated with FogBugz.";
            tblExtendedEvents.AddAutoIncrementPrimaryKey("ixExtendedEvent");

            tblExtendedEvents.AddIntColumn("ixBug", true, 0, "FogBugz Bug entity identifier.");
            tblExtendedEvents.AddVarcharColumn("sEventType", 50, true, "Extended event type name.");
            tblExtendedEvents.AddDateColumn("dtEventUtc", true, DateTimeEx.UnixEpoch, "Date and time of the event in UTC timezone.");
            tblExtendedEvents.AddIntColumn("ixPerson", false, 0, "FogBugz Person entity identifier in case the sAuthor field was matched with a person.");
            tblExtendedEvents.AddVarcharColumn("sPersonName", 50, false, null, "Event author name.");
            tblExtendedEvents.AddTextColumn("sMessage", "Event detailed message.");
            tblExtendedEvents.AddTextColumn("sExternalUrl", "Link to the event in external system that generated the event.");
            tblExtendedEvents.AddVarcharColumn("sCommitRevision", 255, false, null, "Revision number or text from Subversion, Git or other CVS system.");
            tblExtendedEvents.AddVarcharColumn("sBuildName", 255, false, null, "Build name from TeamCity or Jenkins.");

            tblExtendedEvents.AddTableIndex("IX_ixBug", "ixBug", "Index to retrieve events by ixBug efficiently.");
            tblExtendedEvents.AddTableIndex("IX_dtEventUtc", "dtEventUtc", "Index to retrieve events by date efficiently.");

            return tblExtendedEvents;
        }

        public static string GetPluginTableName(CDatabaseApi db)
        {
            return db.PluginTableName(TABLE_EXTENDED_EVENTS_NAME);
        }
    }
}
