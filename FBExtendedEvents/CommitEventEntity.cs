using System;
using FogCreek.FogBugz;
using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    public class CommitEventEntity : IDatabaseEntity
    {
        private const string TABLE_COMMIT_EVENTS_NAME = "CommitEvents";

        public int ixCommitEvent { get; set; }

        public int ixRepository { get; set; }

        public string sRevision { get; set; }

        public DateTime dtCommit { get; set; }

        public string sAuthor { get; set; }

        public int ixPerson { get; set; }

        public int ixBug { get; set; }

        public string sMessage { get; set; }

        public int Save(CDatabaseApi db)
        {
            var qInsert = db.NewInsertQuery(GetPluginTableName(db));

            qInsert.InsertInt("ixRepository", this.ixRepository);
            qInsert.InsertString("sRevision", this.sRevision);
            qInsert.InsertDate("dtCommit", this.dtCommit);
            qInsert.InsertString("sAuthor", this.sAuthor);
            qInsert.InsertInt("ixPerson", this.ixPerson);
            qInsert.InsertInt("ixBug", this.ixBug);
            qInsert.InsertString("sMessage", this.sMessage);

            return qInsert.Execute();
        }

        public static CTable TableDefinition(CDatabaseApi db)
        {
            var tblCommitEventsName = GetPluginTableName(db);
            var tblCommitEvents = db.NewTable(tblCommitEventsName);

            tblCommitEvents.sDesc = "List of commits from CVS systems";
            tblCommitEvents.AddAutoIncrementPrimaryKey("ixCommitEvent");
            tblCommitEvents.AddIntColumn("ixRepository", true, 0, "FogBugz Repository entity identifier.");
            tblCommitEvents.AddVarcharColumn("sRevision", 255, true, "", "Revision number from Subversion, Git or other CVS system.");
            tblCommitEvents.AddDateColumn("dtCommit", false, "Date and time of the commit in UTC.");
            tblCommitEvents.AddVarcharColumn("sAuthor", 50, false, null, "Committer name.");
            tblCommitEvents.AddIntColumn("ixPerson", true, 0, "FogBugz Person entity identifier in case the sAuthor field was matched with a person.");
            tblCommitEvents.AddIntColumn("ixBug", true, 0, "FogBugz Bug entity identifier.");
            tblCommitEvents.AddTextColumn("sMessage", "Commit message.");

            tblCommitEvents.AddTableIndex("IX_ixBug", "ixBug", "Index to retrieve events by ixBug efficiently.");

            return tblCommitEvents;
        }

        public static string GetPluginTableName(CDatabaseApi db)
        {
            return db.PluginTableName(TABLE_COMMIT_EVENTS_NAME);
        }
    }
}
