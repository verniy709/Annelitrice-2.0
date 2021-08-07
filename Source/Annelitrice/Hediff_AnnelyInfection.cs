using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Annelitrice
{
    public class Hediff_AnnelyInfection : HediffWithComps
    {
        public override bool ShouldRemove => base.ShouldRemove;
        public override void Tick()
        {
            base.Tick();
            if (this.Severity >= 1f)
            {
                var pos = this.pawn.Position;
                var map = this.pawn.Map;
                this.pawn.Kill(null, this);
                this.pawn.Corpse.Destroy();
                if (Rand.Chance(0.85f))
                {
                    var meat = ThingMaker.MakeThing(ThingDef.Named("Meat_Megaspider"));
                    meat.stackCount = 50;
                    GenSpawn.Spawn(meat, pos, map);
                }
                else
                {
                    var newPawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(AnnelitriceDefOf.Anneli_Player, Faction.OfPlayer, fixedBiologicalAge: 14, fixedChronologicalAge: 14));
                    GenSpawn.Spawn(newPawn, pos, map);
                }
            }
        }
    }
}
