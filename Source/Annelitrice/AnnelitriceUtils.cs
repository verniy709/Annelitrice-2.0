using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Annelitrice
{
    public static class AnnelitriceUtils
    {
        private static Dictionary<Pawn, CompEvolution> cachedComps = new Dictionary<Pawn, CompEvolution>();

        public static bool TryGetCompEvolution(this Pawn pawn, out CompEvolution comp)
        {
            if (!cachedComps.TryGetValue(pawn, out comp))
            {
                cachedComps[pawn] = comp = pawn.TryGetComp<CompEvolution>();
            }
            return comp != null;
        }
        public static void TrySpawnWorm(this Pawn pawn)
        {
            var raceExtension = pawn?.def.GetModExtension<RaceExtension>();
            if (raceExtension != null)
            {
                Worm worm;
                if (raceExtension.outsiderSpawnThingAfterDestruction != null && pawn.Faction != Faction.OfPlayer)
                {
                    worm = ThingMaker.MakeThing(raceExtension.outsiderSpawnThingAfterDestruction) as Worm;
                }
                else
                {
                    worm = ThingMaker.MakeThing(raceExtension.colonistSpawnThingAfterDestruction) as Worm;
                }
                worm.savedPawn = pawn;
                GenPlace.TryPlaceThing(worm, pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near);
            }
        }
    }
}
