﻿using System;
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
            var sEventType = req.GetString("sEventType", "none");
            var dtEventUtc = req.GetDateTime("dtEventUtc", DateTime.MinValue.ToUniversalTime());
            var sPersonName = req.GetString("sPersonName", null);
            var sMessage = req.GetString("sMessage", null);
            var sExternalUrl = req.GetString("sExternalUrl", null);
            var sCommitRevision = req.GetString("sCommitRevision", null);
            var sBuildName = req.GetString("sBuildName", null);

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
            if (String.IsNullOrEmpty(sAuthor))
            {
                return 0;
            }

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