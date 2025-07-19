using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BodyAttachmentsCompatibility
{
    [HarmonyPatch(typeof(HandDrawer))]
    [HarmonyPatch("HandColor", MethodType.Getter)]
    public class HandDrawer_HandColor_BodyAttachmentsPatch
    {
        private static void Postfix(ref HandDrawer __instance, ref Color __result)
        {
            if (GenTicks.TicksAbs % 100 != 0 || !(__instance.parent is Pawn pawn))
            {
                return;
            }
            List<Hediff> list = pawn.health?.hediffSet?.hediffs.Where((Hediff hediff) => hediff is Hediff_AddedPart hediff_AddedPart && ShowMeYourHandsMain.HediffContainsHand(hediff_AddedPart.Part)).ToList();
            if (list.Count <= 0)
            {
                return;
            }
            if (list.Count == 1)
            {
                Hediff hediff2 = list[0];
                DefModExtension_ArmGraphicPath modExtension = hediff2.def.GetModExtension<DefModExtension_ArmGraphicPath>();
                if (modExtension != null && !modExtension.path.NullOrEmpty())
                {
                    Graphic graphic = GraphicDatabase.Get<Graphic_Single>(modExtension.path, ShaderDatabase.Cutout, new Vector2(1f, 1f), Color.white);
                    if (graphic != null)
                    {
                        ShowMeYourHandsMain.mainHandGraphics[pawn] = graphic;
                    }
                }
                return;
            }
            Hediff hediff3 = list[0];
            DefModExtension_ArmGraphicPath modExtension2 = hediff3.def.GetModExtension<DefModExtension_ArmGraphicPath>();
            if (modExtension2 != null && !modExtension2.path.NullOrEmpty())
            {
                Graphic graphic2 = GraphicDatabase.Get<Graphic_Single>(modExtension2.path, ShaderDatabase.Cutout, new Vector2(1f, 1f), Color.white);
                if (graphic2 != null)
                {
                    ShowMeYourHandsMain.mainHandGraphics[pawn] = graphic2;
                }
            }
            Hediff hediff4 = list[1];
            DefModExtension_ArmGraphicPath modExtension3 = hediff3.def.GetModExtension<DefModExtension_ArmGraphicPath>();
            if (modExtension3 != null && !modExtension3.path.NullOrEmpty())
            {
                Graphic graphic3 = GraphicDatabase.Get<Graphic_Single>(modExtension3.path, ShaderDatabase.Cutout, new Vector2(1f, 1f), Color.white);
                if (graphic3 != null)
                {
                    ShowMeYourHandsMain.mainHandGraphics[pawn] = graphic3;
                }
            }
        }
    }
}
