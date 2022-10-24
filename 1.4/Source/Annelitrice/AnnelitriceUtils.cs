using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Annelitrice
{
	public static class AnnelitriceUtils
	{
		private static readonly Dictionary<Pawn, CompEvolution> cachedComps = new Dictionary<Pawn, CompEvolution>();

		public static bool TryGetCompEvolution(this Pawn pawn, out CompEvolution comp)
		{
			if (!cachedComps.TryGetValue(pawn, out comp))
			{
				cachedComps[pawn] = comp = pawn.TryGetComp<CompEvolution>();
			}
			return comp != null;
		}

		//Not sure what this does
		public static void UpdateGraphics(this Pawn pawn)
		{
			pawn.Drawer.renderer.graphics.ResolveAllGraphics();
			PortraitsCache.SetDirty(pawn);
			PortraitsCache.PortraitsCacheUpdate();
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
		}

		//Spawn worms upon body destruction
		public static void TrySpawnWorm(this Pawn pawn)
		{
			var raceExtension = pawn?.def.GetModExtension<RaceExtension>();
			if (raceExtension != null)
			{
				var worm = raceExtension.outsiderSpawnThingAfterDestruction != null && pawn.Faction != Faction.OfPlayer
					? ThingMaker.MakeThing(raceExtension.outsiderSpawnThingAfterDestruction) as Worm
					: ThingMaker.MakeThing(raceExtension.colonistSpawnThingAfterDestruction) as Worm;
				worm.savedPawn = pawn;
				GenPlace.TryPlaceThing(worm, pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near);
			}
		}
	}
}
