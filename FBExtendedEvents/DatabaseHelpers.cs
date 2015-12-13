using FogCreek.FogBugz.Plugins.Api;

namespace FBExtendedEvents
{
    public static class DatabaseHelpers
    {
        public static void SaveEntities(this CDatabaseApi db, params IDatabaseEntity[] entities)
        {
            foreach (var entity in entities)
            {
                entity.Save(db);
            }
        }
    }
}