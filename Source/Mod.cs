using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BodyAttachments
{
    public class BodyAttachmentsMod : Mod
    {
        private Settings settings;

        private Vector2 scrollPositionProstheses;

        private Vector2 scrollPositionLayers;

        private float prosthesesHeight = 0f;

        private float layersHeight = 0f;

        public BodyAttachmentsMod(ModContentPack content)
            : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (settings == null)
            {
                settings = GetSettings<Settings>();
            }
            settings.ResetLayersIfNeeded();
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width / 2f, inRect.height);
            DrawLeftRect(rect);
            Rect rect2 = new Rect(rect.x + rect.width, rect.y, rect.width, rect.height);
            DrawRightRect(rect2);
            Rect rect3 = new Rect(rect2.x + rect2.width - 80f, rect2.y, 80f, 40f);
            if (Widgets.ButtonText(rect3, "BodyAttachments.Reset".Translate()))
            {
                settings.ResetLayersIfNeeded(forced: true);
            }
        }

        private void DrawRightRect(Rect rect)
        {
            Widgets.BeginScrollView(viewRect: new Rect(0f, 0f, rect.width - 20f, layersHeight), outRect: rect, scrollPosition: ref scrollPositionLayers);
            float height2 = 0f;
            DrawChild(PawnRenderTreeDefOfLocal.Humanlike.root, ref height2, 0);
            layersHeight = height2;
            Widgets.EndScrollView();
            void DrawChild(PawnRenderNodeProperties root, ref float height, int level)
            {
                Rect rect2 = new Rect(0f, height, rect.width - 20f, 25f);
                Widgets.Label(rect2, $"{new string(' ', level * 4)}{root.debugLabel} : {root.baseLayer}");
                height += rect2.height;
                if (!root.children.NullOrEmpty())
                {
                    foreach (PawnRenderNodeProperties child in root.children)
                    {
                        DrawChild(child, ref height, level + 1);
                    }
                }
            }
        }

        private void DrawLeftRect(Rect rect)
        {
            Widgets.BeginScrollView(viewRect: new Rect(0f, 0f, rect.width - 20f, prosthesesHeight), outRect: rect, scrollPosition: ref scrollPositionProstheses);
            float num = 0f;
            for (int i = 0; i < settings.hediffLayerDict.Count; i++)
            {
                KeyValuePair<string, int> keyValuePair = settings.hediffLayerDict.ElementAt(i);
                HediffDef namedSilentFail = DefDatabase<HediffDef>.GetNamedSilentFail(keyValuePair.Key ?? "");
                if (namedSilentFail != null)
                {
                    Rect rect2 = new Rect(0f, num, rect.width - 20f, 35f);
                    int value = (int)Widgets.HorizontalSlider(rect2, keyValuePair.Value, 0f, 100f, middleAlignment: true, $"{namedSilentFail.LabelCap}: {keyValuePair.Value}");
                    settings.hediffLayerDict[keyValuePair.Key] = value;
                    num += rect2.height;
                }
            }
            prosthesesHeight = num;
            Widgets.EndScrollView();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            Utility.CheckSettings();
        }

        public override string SettingsCategory()
        {
            return "BodyAttachments.BodyAttachmentsMod".Translate();
        }
    }
}
