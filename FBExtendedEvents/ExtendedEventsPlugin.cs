using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Text;
using FogCreek.FogBugz;
using FogCreek.FogBugz.Plugins;
using FogCreek.FogBugz.Plugins.Api;
using FogCreek.FogBugz.Plugins.Interfaces;

namespace FBExtendedEvents
{
    public class ExtendedEventsPlugin : Plugin, IPluginDatabase
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
    }
}
