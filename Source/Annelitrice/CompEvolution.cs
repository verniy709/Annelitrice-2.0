using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Annelitrice
{
    public class CompProperties_Evolution : CompProperties
    {
        public CompProperties_Evolution()
        {
            this.compClass = typeof(CompEvolution);
        }
    }
    public class CompEvolution : ThingComp
    {
        public const int MaxBilePoints = 20;
        public int blueBilePoints;
        public int greenBilePoints;
        public int redBilePoints;
        public const int BilePointCost = 25;
        public int evolutionPoints;
        public const int HealingCost = 50;
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref blueBilePoints, "blueBilePoints");
            Scribe_Values.Look(ref greenBilePoints, "greenBilePoints");
            Scribe_Values.Look(ref redBilePoints, "redBilePoints");
            Scribe_Values.Look(ref evolutionPoints, "evolutionPoints");

        }
    }
}
