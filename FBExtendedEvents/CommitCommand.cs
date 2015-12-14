using System;
using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    internal class CommitCommand
    {
        private CPluginApi api;

        public CommitCommand(CPluginApi api)
        {
            this.api = api;
        }

        public int Process()
        {
            var req = this.api.Request;

            var ixRepository = req.GetInt32("ixRepository", 0);
            var sRevision = req.GetString("sRevision", "");
            var dtCommit = req.GetDateTime("dtCommit", DateTime.MinValue.ToUniversalTime());
            var sAuthor = req.GetString("sAuthor", "");
            var ixBug = req.GetInt32("ixBug", 0);
            var ixPerson = this.TryLoadPersonId(sAuthor);

            var entity = new CommitEventEntity
            {
                ixRepository = ixRepository,
                sRevision = sRevision,
                dtCommit = dtCommit,
                ixBug = ixBug,
                sAuthor = sAuthor,
                ixPerson = ixPerson
            };

            var ixCommitEvent = entity.Save(this.api.Database);
            return ixCommitEvent;
        }

        private int TryLoadPersonId(string sAuthor)
        {
            var q = this.api.Person.NewPersonQuery();
            q.IgnorePermissions = true;
            q.AddSuffixLike("sLDAPUid", @"\" + sAuthor);

            var persons = q.List();
            if (persons != null && persons.Length > 0)
            {
                return persons[0].ixPerson;
            }

            return 0;
        }
    }
}