using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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

    [HarmonyPatch(typeof(CompRefuelable), "ConsumptionRatePerTick", MethodType.Getter)]
    public static class CompRefuelable_ConsumptionRatePerTick_Patch
    {
        public static bool Prefix(CompRefuelable __instance, ref float __result)
        {
            if (__instance.parent is WormIncubator incubator)
            {
                __result = incubator.GetFuelConsumptionRate();

                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(CompRefuelable), "CompInspectStringExtra")]
    public static class CompRefuelable_CompInspectStringExtra_Patch
    {
        public static bool Prefix(CompRefuelable __instance, ref string __result)
        {
            if (__instance.parent is WormIncubator incubator)
            {
                __result = CompInspectStringExtra(incubator, __instance);
                return false;
            }
            return true;
        }
        public static string CompInspectStringExtra(WormIncubator incubator, CompRefuelable instance)
        {
            if (instance.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
            {
                return string.Empty;
            }
            string text = instance.Props.FuelLabel + ": " + instance.Fuel.ToStringDecimalIfSmall() + " / " + instance.Props.fuelCapacity.ToStringDecimalIfSmall();
            if (!instance.Props.consumeFuelOnlyWhenUsed && instance.HasFuel)
            {
                int numTicks = (int)((instance.Fuel / incubator.GetFuelConsumptionRate()));// * 60000f);
                text = text + " (" + numTicks.ToStringTicksToPeriod() + ")";
            }
            if (!instance.HasFuel && !instance.Props.outOfFuelMessage.NullOrEmpty())
            {
                text += $"\n{instance.Props.outOfFuelMessage} ({instance.GetFuelCountToFullyRefuel()}x {instance.Props.fuelFilter.AnyAllowedDef.label})";
            }
            if (instance.Props.targetFuelLevelConfigurable)
            {
                text += "\n" + "ConfiguredTargetFuelLevel".Translate(instance.TargetFuelLevel.ToStringDecimalIfSmall());
            }
            return text;
        }
    }

    [HarmonyPatch(typeof(Pawn_DraftController), "GetGizmos")]
    public class Pawn_DraftController_GetGizmos_Patch
    {
        protected static TargetingParameters GetHumanCorpseTargetParameters(Pawn caster)
        {
            return new TargetingParameters
            {
                canTargetAnimals = false,
                canTargetHumans = false,
                canTargetMechs = false,
                mapObjectTargetsMustBeAutoAttackable = false,
                canTargetPawns = false,
                canTargetSelf = false,
                canTargetBuildings = false,
                canTargetItems = true,
                validator = (TargetInfo targ) => targ.Thing is Corpse corpse && corpse.InnerPawn.RaceProps.Humanlike && corpse.Position.DistanceTo(caster.Position) <= 2.9f
            };
        }

        private static Dictionary<Pawn, CompEvolution> cachedComps = new Dictionary<Pawn, CompEvolution>();
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn_DraftController __instance)
        {
            foreach (var g in __result)
            {
                yield return g;
            }
            Pawn pawn = __instance.pawn;
            if (!cachedComps.TryGetValue(pawn, out var comp))
            {
                cachedComps[pawn] = pawn.TryGetComp<CompEvolution>();
            }
            if (comp == null)
            {
                yield break;
            }

            Command_Action item = new Command_Action()
            {
                defaultLabel = "Annely.Assimilate".Translate(),
                defaultDesc = "Annely.AssimilateDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("Anneli_Skill/Assimilate"),
                action = delegate
                {
                    Find.Targeter.BeginTargeting(GetHumanCorpseTargetParameters(pawn), delegate (LocalTargetInfo t)
                    {
                        Job job = JobMaker.MakeJob(AnnelitriceDefOf.Annely_AssimilateCorpse, t.Thing);
                        pawn.jobs.TryTakeOrderedJob(job);
                    }, pawn);
                }
            };
            yield return item;
        }
    }
}
