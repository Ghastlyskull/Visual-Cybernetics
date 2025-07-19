using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BodyAttachments
{
    public class Settings : ModSettings
    {
        private Dictionary<string, int> defaultSettingsDict = new Dictionary<string, int>();

        public Dictionary<string, int> hediffLayerDict = new Dictionary<string, int>();

        public void CastChanges()
        {
            if (hediffLayerDict.NullOrEmpty())
            {
                return;
            }
            foreach (KeyValuePair<string, int> item in hediffLayerDict)
            {
                HediffDef named = DefDatabase<HediffDef>.GetNamed(item.Key ?? "");
                DrawData.RotationalData rotationalData = (DrawData.RotationalData)AccessTools.Field(typeof(DrawData), "defaultData").GetValue(named.RenderNodeProperties[0].drawData);
                rotationalData.layer = item.Value;
                AccessTools.Field(typeof(DrawData), "defaultData").SetValue(named.RenderNodeProperties[0].drawData, rotationalData);
            }
        }

        public void ResetLayersIfNeeded(bool forced = false)
        {
            if (!(hediffLayerDict.NullOrEmpty() || forced))
            {
                return;
            }
            hediffLayerDict = new Dictionary<string, int>();
            foreach (HediffDef allDef in DefDatabase<HediffDef>.AllDefs)
            {
                if (defaultSettingsDict.TryGetValue(allDef.defName, out var value))
                {
                    hediffLayerDict.Add(allDef.defName, value);
                }
            }
            CastChanges();
        }

        public void FillDefaultSettings()
        {
            defaultSettingsDict = new Dictionary<string, int>();
            foreach (HediffDef allDef in DefDatabase<HediffDef>.AllDefs)
            {
                DefModExtension_HediffLayer modExtension = allDef.GetModExtension<DefModExtension_HediffLayer>();
                if (modExtension != null && !allDef.RenderNodeProperties.NullOrEmpty())
                {
                    DrawData.RotationalData rotationalData = (DrawData.RotationalData)AccessTools.Field(typeof(DrawData), "defaultData").GetValue(allDef.RenderNodeProperties[0].drawData);
                    defaultSettingsDict.Add(allDef.defName, (int)rotationalData.layer.Value);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref hediffLayerDict, "hediffLayerDict", LookMode.Value, LookMode.Value);
        }
    }
}
