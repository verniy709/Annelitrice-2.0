using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Annelitrice
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        public static Harmony harmonyInstance;
        static HarmonyInit()
        {
            harmonyInstance = new Harmony("Annelitrice.Mod");
            harmonyInstance.PatchAll();
        }
    }
    [HarmonyPatch(typeof(Pawn), "Destroy")]
    public static class Pawn_Destroy_Patch
    {
        public static void Prefix(Pawn __instance)
        {
            if (__instance.Corpse is null)
            {
                __instance.TrySpawnWorm();
            }
        }
    }
    [HarmonyPatch(typeof(Corpse), "Destroy")]
    public static class Corpse_Destroy_Patch
    {
        public static void Prefix(Corpse __instance)
        {
            __instance.InnerPawn.TrySpawnWorm();
        }
    }

    [HarmonyPatch(typeof(ITab_Pawn_Character), "PawnToShowInfoAbout", MethodType.Getter)]
    public static class ITab_Pawn_Character_PawnToShowInfoAbout_Patch
    {
        public static bool Prefix(ref Pawn __result)
        {
            if (Find.Selector.SingleSelectedThing is Worm worm)
            {
                __result = worm.savedPawn;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CompTargetEffect_Resurrect), "DoEffectOn")]
    public static class CompTargetEffect_Resurrect_DoEffectOn_Patch
    {
        public static bool Prefix(CompTargetEffect_Resurrect __instance, Pawn user, Thing target)
        {
            if (target is Worm worm)
            {
                if (user.IsColonistPlayerControlled && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
                {
                    Job job = JobMaker.MakeJob(AnnelitriceDefOf.Annely_ResurrectUsingWorm, worm, __instance.parent);
                    job.count = 1;
                    user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    return false;
                }
            }
            return true;
        }
    }
}
