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
        public const int AppendageCost = 200;

        public Dictionary<BodyPartDef, HediffDef> rightAppendagesActive;
        public Dictionary<BodyPartDef, HediffDef> leftAppendagesActive;
        public Dictionary<BodyPartDef, HediffDef> appendagesActive;
        private Dictionary<Hediff_MissingPart, int> missingParts = new Dictionary<Hediff_MissingPart, int>();


        private void PreInit()
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
            if (missingParts is null)
            {
                missingParts = new Dictionary<Hediff_MissingPart, int>();
            }
        }
        public CompEvolution()
        {
            PreInit();
        }

        private Pawn __pawn;
        private Pawn pawn
        {
            get
            {
                if (__pawn is null)
                {
                    __pawn = this.parent as Pawn;
                }
                return __pawn;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref blueBilePoints, "blueBilePoints");
            Scribe_Values.Look(ref greenBilePoints, "greenBilePoints");
            Scribe_Values.Look(ref redBilePoints, "redBilePoints");
            Scribe_Values.Look(ref evolutionPoints, "evolutionPoints");
            Scribe_Values.Look(ref nextHealTick, "nextHealTick");
            Scribe_Collections.Look(ref rightAppendagesActive, "rightAppendagesActive", LookMode.Def, LookMode.Def, ref bodyPartKeys1, ref hediffDefValues1);
            Scribe_Collections.Look(ref leftAppendagesActive, "leftAppendagesActive", LookMode.Def, LookMode.Def, ref bodyPartKeys2, ref hediffDefValues2);
            Scribe_Collections.Look(ref appendagesActive, "appendagesActive", LookMode.Def, LookMode.Def, ref bodyPartKeys3, ref hediffDefValues3);
            Scribe_Collections.Look(ref missingParts, "missingParts", LookMode.Reference, LookMode.Value, ref missingPartsKeys, ref intValues);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                PreInit();
                missingParts.RemoveAll(x => x.Key.pawn != pawn || !pawn.health.hediffSet.hediffs.Contains(x.Key));
            }
        }

        private int nextHealTick;
        private int GetNextHealTick
        {
            get
            {
                var baseValue = 250;
                if (greenBilePoints > 0)
                {
                    baseValue -= greenBilePoints * 6;
                }
                return baseValue;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            foreach (var part in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
            {
                if (!missingParts.ContainsKey(part))
                {
                    missingParts[part] = Find.TickManager.TicksGame + 1200;
                }
            }
            
            if (Find.TickManager.TicksGame >= nextHealTick)
            {
                nextHealTick = Find.TickManager.TicksGame + GetNextHealTick;
                foreach (var missingPart in missingParts.InRandomOrder().ToList())
                {
                    if (Find.TickManager.TicksGame >= missingPart.Value)
                    {
                        var part = missingPart.Key.Part;
                        pawn.health.RestorePart(part);
                        var injury = HediffMaker.MakeHediff(AnnelitriceDefOf.Annely_Regeneration, pawn, part);
                        injury.Severity = part.def.GetMaxHealth(pawn) - 1;
                        pawn.health.AddHediff(injury);
                        missingParts.Remove(missingPart.Key);
                        return;
                    }
                }


                foreach (var part in pawn.health.hediffSet.GetInjuredParts().InRandomOrder())
                {
                    var curHP = pawn.health.hediffSet.GetPartHealth(part);
                    var maxHP = part.def.GetMaxHealth(pawn);
                    if (maxHP > curHP)
                    {
                        var diff = (int)Mathf.Clamp(maxHP - curHP, 1, int.MaxValue);
                        var injuries = pawn.health.hediffSet.hediffs.Where(x => x is Hediff_Injury && x.Part == part);
                        for (var i = 0; i < diff; i++)
                        {
                            var hediffs = injuries.Where(x => x.Severity > 0);
                            if (hediffs.TryRandomElement(out var hediff))
                            {
                                hediff.Heal(1);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public BodyPartRecord GetRightPart(BodyPartDef bodyPartDef)
        {
            var parts = pawn.health.hediffSet.GetNotMissingParts().Where(x => x.def == bodyPartDef);
            if (bodyPartDef == BodyPartDefOf.Arm)
            {
                return parts.FirstOrDefault(x => x.untranslatedCustomLabel == "right arm");
            }
            else if (bodyPartDef == BodyPartDefOf.Leg)
            {
                return parts.FirstOrDefault(x => x.untranslatedCustomLabel == "right leg");
            }
            return null;
        }
        public BodyPartRecord GetLeftPart(BodyPartDef bodyPartDef)
        {
            var parts = pawn.health.hediffSet.GetNotMissingParts().Where(x => x.def == bodyPartDef);
            if (bodyPartDef == BodyPartDefOf.Arm)
            {
                return parts.FirstOrDefault(x => x.untranslatedCustomLabel == "left arm");
            }
            else if (bodyPartDef == BodyPartDefOf.Leg)
            {
                return parts.FirstOrDefault(x => x.untranslatedCustomLabel == "left leg");
            }
            return null;
        }

        private List<BodyPartDef> bodyPartKeys1;
        private List<HediffDef> hediffDefValues1;

        private List<BodyPartDef> bodyPartKeys2;
        private List<HediffDef> hediffDefValues2;

        private List<BodyPartDef> bodyPartKeys3;
        private List<HediffDef> hediffDefValues3;

        private List<Hediff_MissingPart> missingPartsKeys;
        private List<int> intValues;
    }
}
