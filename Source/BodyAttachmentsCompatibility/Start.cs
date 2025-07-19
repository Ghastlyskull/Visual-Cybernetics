using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BodyAttachmentsCompatibility
{
    [StaticConstructorOnStartup]
    public static class Start
    {
        private static Harmony harmonyInstance;

        public static Harmony HarmonyInstance => harmonyInstance;

        static Start()
        {
            harmonyInstance = new Harmony("DimonSever000.BodyAttachmentsCompatibility");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
