using System;
using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    internal class CreateEventCommand
    {
        private CPluginApi api;

        public CreateEventCommand(CPluginApi api)
        {
            this.api = api;
        }

        public int Process()
        {
            var req = this.api.Request;

            var ixBug = req.GetInt32("ixBug", 0);
            var sEventType = req.GetString("sEventType", "");
            var dtEventUtc = req.GetDateTime("dtEventUtc", DateTime.MinValue.ToUniversalTime());
            var sPersonName = req.GetString("sPersonName", "");
            var sMessage = req.GetString("sMessage", "");
            var sExternalUrl = req.GetString("sExternalUrl", "");
            var sCommitRevision = req.GetString("sCommitRevision", "");
            var sBuildName = req.GetString("sBuildName", "");

            var ixPerson = this.TryLoadPersonId(sPersonName);

            var entity = new ExtendedEventEntity
            {
                ixBug = ixBug,
                sEventType = sEventType,
                dtEventUtc = dtEventUtc,
                ixPerson = ixPerson,
                sPersonName = sPersonName,
                sMessage = sMessage,
                sExternalUrl = sExternalUrl,
                sCommitRevision = sCommitRevision,
                sBuildName = sBuildName
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