using System;
using System.Collections.Generic;
using System.Text;
using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    public interface IDatabaseEntity
    {
        int Save(CDatabaseApi db);
    }
}
