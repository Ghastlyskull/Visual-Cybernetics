using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BodyAttachments
{

    [StaticConstructorOnStartup]
    public static class Utility
    {
        public static Settings Settings => LoadedModManager.GetMod<BodyAttachmentsMod>().GetSettings<Settings>();

        static Utility()
        {
            CheckSettings();
        }

        public static void CheckSettings()
        {
            Settings.FillDefaultSettings();
            Settings.ResetLayersIfNeeded();
        }
    }
}
