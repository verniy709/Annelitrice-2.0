using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Annelitrice
{
    public class WormIncubator : Building_Storage
    {
        public const int MaximumEvolutionPoints = 100000;
        public int evolutionPoints;
        public List<Worm> StoredWorms
        {
            get
            {
                List<Worm> worms = new List<Worm>();
                foreach (var cell in AllSlotCellsList())
                {
                    worms.AddRange(this.Map.thingGrid.ThingsListAtFast(cell).OfType<Worm>());
                }
                return worms;
            }
        }

        public float GetFuelConsumptionRate()
        {
            var storedWorms = this.StoredWorms;
            if (storedWorms.Any())
            {
                return (storedWorms.Count() * 0.1f) / 60000f;
            }
            return 0;
        }
        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame % GenDate.TicksPerDay == 0)
            {
                var evolutionPointsToAdd = StoredWorms.Count;
                var freeSpace = MaximumEvolutionPoints - evolutionPoints;
                evolutionPoints += Mathf.Min(freeSpace, evolutionPointsToAdd);
            }
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine("Annely.StoredEvolutionPoints".Translate(evolutionPoints));
            return stringBuilder.ToString().TrimEndNewlines();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref evolutionPoints, "evolutionPoints");
        }
    }
}
