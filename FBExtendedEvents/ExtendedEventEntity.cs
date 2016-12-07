using System;
using System.Collections.Generic;
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

        public static CTable TableDefinition(CDatabaseApi db)
        {
            var tblExtendedEventsName = GetPluginTableName(db);
            var tblExtendedEvents = db.NewTable(tblExtendedEventsName);

            tblExtendedEvents.sDesc = "List of extended events from external systems integrated with FogBugz.";
            tblExtendedEvents.AddAutoIncrementPrimaryKey("ixExtendedEvent");

            tblExtendedEvents.AddIntColumn("ixBug", true, 0, "FogBugz Bug entity identifier.");
            tblExtendedEvents.AddVarcharColumn("sEventType", 50, true, "Extended event type name.");
            tblExtendedEvents.AddDateColumn("dtEventUtc", false, "Date and time of the event in UTC timezone.");
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
