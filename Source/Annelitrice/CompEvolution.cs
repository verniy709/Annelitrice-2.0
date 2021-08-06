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

        public Dictionary<BodyPartDef, HediffDef> rightAppendagesActive = new Dictionary<BodyPartDef, HediffDef>();
        public Dictionary<BodyPartDef, HediffDef> leftAppendagesActive = new Dictionary<BodyPartDef, HediffDef>();
        public Dictionary<BodyPartDef, HediffDef> appendagesActive = new Dictionary<BodyPartDef, HediffDef>();

        public const int AppendageCost = 200;
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref blueBilePoints, "blueBilePoints");
            Scribe_Values.Look(ref greenBilePoints, "greenBilePoints");
            Scribe_Values.Look(ref redBilePoints, "redBilePoints");
            Scribe_Values.Look(ref evolutionPoints, "evolutionPoints");
            Scribe_Collections.Look(ref rightAppendagesActive, "rightAppendagesActive", LookMode.Def, LookMode.Def, ref bodyPartKeys1, ref hediffDefValues1);
            Scribe_Collections.Look(ref leftAppendagesActive, "leftAppendagesActive", LookMode.Def, LookMode.Def, ref bodyPartKeys2, ref hediffDefValues2);
            Scribe_Collections.Look(ref appendagesActive, "appendagesActive", LookMode.Def, LookMode.Def, ref bodyPartKeys3, ref hediffDefValues3);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (rightAppendagesActive is null)
                {
                    rightAppendagesActive = new Dictionary<BodyPartDef, HediffDef>();
                }
                if (leftAppendagesActive is null)
                {
                    leftAppendagesActive = new Dictionary<BodyPartDef, HediffDef>();
                }
                if (appendagesActive is null)
                {
                    appendagesActive = new Dictionary<BodyPartDef, HediffDef>();
                }
            }
        }

        private List<BodyPartDef> bodyPartKeys1;
        private List<HediffDef> hediffDefValues1;

        private List<BodyPartDef> bodyPartKeys2;
        private List<HediffDef> hediffDefValues2;

        private List<BodyPartDef> bodyPartKeys3;
        private List<HediffDef> hediffDefValues3;
    }
}
