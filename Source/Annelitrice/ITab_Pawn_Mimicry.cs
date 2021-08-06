﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Annelitrice
{
	public class ITab_Pawn_Mimicry : ITab
	{
		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (base.SelPawn != null)
				{
					pawn = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.InnerPawn;
					}
				}
				if (pawn == null)
				{
					Log.Error("ITab_Pawn_Mimicry tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		private CompEvolution compEvolution;

		private Pawn pawn;
		public override bool IsVisible => PawnToShowInfoAbout.story != null;
		public ITab_Pawn_Mimicry()
		{
			labelKey = "Annely.Mimicry";
		}

		public Dictionary<BodyPartDef, Rect> bodyPartRects = new Dictionary<BodyPartDef, Rect>();
		public Dictionary<BodyPartDef, Rect> leftBodyPartRects = new Dictionary<BodyPartDef, Rect>();
		public Dictionary<BodyPartDef, Rect> rightBodyPartRects = new Dictionary<BodyPartDef, Rect>();

		public override void OnOpen()
        {
            base.OnOpen();
			pawn = PawnToShowInfoAbout;
			compEvolution = pawn.TryGetComp<CompEvolution>();
			rightBodyPartRects[BodyPartDefOf.Arm] = new Rect(333, 260, 80, 80);
			rightBodyPartRects[BodyPartDefOf.Leg] = new Rect(278, 485, 80, 80);
			leftBodyPartRects[BodyPartDefOf.Arm] = new Rect(88, 260, 80, 80);
			leftBodyPartRects[BodyPartDefOf.Leg] = new Rect(142, 485, 80, 80);
			bodyPartRects[AnnelitriceDefOf.Spine] = new Rect(363, 385, 80, 80);
			bodyPartRects[BodyPartDefOf.Torso] = new Rect(58, 385, 80, 80);
			bodyPartRects[BodyPartDefOf.Head] = new Rect(211, 205, 80, 80);
		}

		[TweakValue("AN", 0, 1000)] public static float menuWidth = 500;
		[TweakValue("AN", 0, 1000)] public static float menuHeight = 625;
		protected override void UpdateSize()
		{
			base.UpdateSize();
			size = new Vector2(menuWidth, menuHeight);
		}

		[TweakValue("AN", 0, 1000)] public static float redX;
		[TweakValue("AN", 0, 1000)] public static float redY;
		[TweakValue("AN", 0, 1000)] public static float redW = 80;
		[TweakValue("AN", 0, 1000)] public static float redH = 80;
		protected override void FillTab()
		{
			UpdateSize();

			var totalRect = new Rect(0, 0, size.x, size.y);
			GUI.DrawTexture(totalRect, Textures.BackgroundMenu);
			var curFont = Text.Font;
			var curColor = GUI.color;
			Text.Font = GameFont.Medium;
			GUI.color = Color.black;

			var redBilePointsRect = new Rect(115, 45, 60, 35);
			Widgets.Label(redBilePointsRect, compEvolution.redBilePoints + " / " + CompEvolution.MaxBilePoints);

			var greenBilePointsRect = new Rect(redBilePointsRect.x, redBilePointsRect.yMax + 15, redBilePointsRect.width, redBilePointsRect.height);
			Widgets.Label(greenBilePointsRect, compEvolution.redBilePoints + " / " + CompEvolution.MaxBilePoints);

			var blueBilePointsRect = new Rect(greenBilePointsRect.x, greenBilePointsRect.yMax + 15, greenBilePointsRect.width, greenBilePointsRect.height);
			Widgets.Label(blueBilePointsRect, compEvolution.blueBilePoints + " / " + CompEvolution.MaxBilePoints);

			GUI.color = curColor;
			Vector2 point = new Vector2(redBilePointsRect.xMax + 21, redBilePointsRect.y + -14);

			DoMinusButton(ref point, ref compEvolution.redBilePoints, "Annely.Red".Translate(), Textures.Red_Minus);
			DoPlusButton(ref point, ref compEvolution.redBilePoints, "Annely.Red".Translate(), Textures.Red_Plus);

			DoMinusButton(ref point, ref compEvolution.greenBilePoints, "Annely.Green".Translate(), Textures.Green_Minus);
			DoPlusButton(ref point, ref compEvolution.greenBilePoints, "Annely.Green".Translate(), Textures.Green_Plus);

			DoMinusButton(ref point, ref compEvolution.blueBilePoints, "Annely.Blue".Translate(), Textures.Blue_Minus);
			DoPlusButton(ref point, ref compEvolution.blueBilePoints, "Annely.Blue".Translate(), Textures.Blue_Plus);

			var healRect = new Rect(350, 29, 115, 115);
			GUI.DrawTexture(healRect, Textures.Heal_Button);
			TooltipHandler.TipRegion(healRect, "Annely.HealTooltip".Translate(CompEvolution.HealingCost));
			if (healRect.IsLeftClicked())
            {
				if (compEvolution.evolutionPoints < CompEvolution.HealingCost)
                {
					Messages.Message("Annely.NotEnoughEPs".Translate(), pawn, MessageTypeDefOf.CautionInput);
				}
				else
                {
					compEvolution.evolutionPoints -= CompEvolution.HealingCost;
				}
			}

			curColor = GUI.color;
			GUI.color = Color.black;

			var evolutionPointsRect = new Rect(365, 155, 85, 35);
			Widgets.Label(evolutionPointsRect, "Annely.EPs".Translate() + ": " + compEvolution.evolutionPoints.ToString());
			
			GUI.color = curColor;

			foreach (var bodyPartRect in rightBodyPartRects)
            {
				var part = GetRightPart(pawn, bodyPartRect.Key);
				var pairs = Textures.rightPairedAppendanges[bodyPartRect.Key];
				var allAppendages = pairs.Keys.ToList();
				var activeAppendage = part != null ? GetActiveAppendage(part, allAppendages, compEvolution.rightAppendagesActive) : null;
				DrawAppendages(bodyPartRect.Value, pairs, activeAppendage, part, allAppendages, compEvolution.rightAppendagesActive);
			}

			foreach (var bodyPartRect in leftBodyPartRects)
			{
				var part = GetLeftPart(pawn, bodyPartRect.Key);
				var pairs = Textures.leftPairedAppendanges[bodyPartRect.Key];
				var allAppendages = pairs.Keys.ToList();
				var activeAppendage = part != null ? GetActiveAppendage(part, allAppendages, compEvolution.leftAppendagesActive) : null;
				DrawAppendages(bodyPartRect.Value, pairs, activeAppendage, part, allAppendages, compEvolution.leftAppendagesActive);
			}

			foreach (var bodyPartRect in bodyPartRects)
			{
				var part = pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault(x => x.def == bodyPartRect.Key);
				var pairs = Textures.pairedAppendanges[bodyPartRect.Key];
				var allAppendages = pairs.Keys.ToList();
				var activeAppendage = part != null ? GetActiveAppendage(part, allAppendages, compEvolution.appendagesActive) : null;
				DrawAppendages(bodyPartRect.Value, pairs, activeAppendage, part, allAppendages, compEvolution.appendagesActive);
			}

			Text.Font = curFont;
			GUI.color = curColor;
		}

		private void DrawAppendages(Rect rect, Dictionary<HediffDef, Texture2D> pairs, HediffDef activeAppendage, BodyPartRecord part, List<HediffDef> allAppendages, Dictionary<BodyPartDef, HediffDef> appendagesActive)
        {
			foreach (var appendages in pairs)
			{
				if (part != null)
				{
					if (activeAppendage != null)
					{
						GUI.DrawTexture(rect, pairs[activeAppendage]);
						if (rect.IsLeftClicked())
						{
							if (compEvolution.evolutionPoints < CompEvolution.AppendageCost)
                            {
								Messages.Message("Annely.NotEnoughEPs".Translate(), pawn, MessageTypeDefOf.CautionInput);
                            }
							else
                            {
								CallFloatMenuForActiveAppendages(part.def, allAppendages.Where(x => x != activeAppendage).ToList(), appendagesActive);
                            }
						}
					}
					else
					{
						if (rect.IsLeftClicked())
						{
							if (compEvolution.evolutionPoints < CompEvolution.AppendageCost)
							{
								Messages.Message("Annely.NotEnoughEPs".Translate(), pawn, MessageTypeDefOf.CautionInput);
							}
							else
                            {
								CallFloatMenuForActiveAppendages(part.def, allAppendages, appendagesActive);
                            }
						}
					}
				}
				else if (rect.IsLeftClicked())
                {
					Messages.Message("Annely.BodyPartIsMissing".Translate(), pawn, MessageTypeDefOf.CautionInput);
				}
			}
		}
		private void CallFloatMenuForActiveAppendages(BodyPartDef bodyPartDef, List<HediffDef> allAppendages, Dictionary<BodyPartDef, HediffDef> appendagesActive)
        {
			var floatMenuOptions = new List<FloatMenuOption>();
			foreach (var appendage in allAppendages)
			{
				floatMenuOptions.Add(new FloatMenuOption("CommandInstall".Translate() + " " + appendage.label, delegate
				{
					appendagesActive[bodyPartDef] = appendage;
					compEvolution.evolutionPoints -= CompEvolution.AppendageCost;
				}));
			}
			Find.WindowStack.Add(new FloatMenu(floatMenuOptions));
		}
		private HediffDef GetActiveAppendage(BodyPartRecord bodyPart, List<HediffDef> allAppendanges, Dictionary<BodyPartDef, HediffDef> appendagesActive)
        {
			if (appendagesActive.TryGetValue(bodyPart.def, out var hediffDef))
            {
				return hediffDef;
            }
			var hediff = pawn.health.hediffSet.hediffs.FirstOrDefault(x => allAppendanges.Contains(x.def) && x.Part == bodyPart);
			if (hediff != null)
            {
				return hediff.def;
            }
			return null;
		}
		private BodyPartRecord GetRightPart(Pawn pawn, BodyPartDef bodyPartDef)
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
		private BodyPartRecord GetLeftPart(Pawn pawn, BodyPartDef bodyPartDef)
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
		private void DoMinusButton(ref Vector2 point, ref int bilePointField, string colorName, Texture minusTexture)
        {
			var bileMinusButton = new Rect(point.x, point.y, 50, 50);
			GUI.DrawTexture(bileMinusButton, minusTexture);
			TooltipHandler.TipRegion(bileMinusButton, "Annely.MinusSignTooltip".Translate(colorName, CompEvolution.BilePointCost));

			if (bileMinusButton.IsLeftClicked())
			{
				if (bilePointField <= 0)
				{
					Messages.Message("Annely.NoBilePoints".Translate(colorName), pawn, MessageTypeDefOf.CautionInput);
				}
				else
				{
					bilePointField -= 1;
					compEvolution.evolutionPoints += CompEvolution.BilePointCost;
				}
			}
			point.x = bileMinusButton.xMax + 4;
		}

		private void DoPlusButton(ref Vector2 point, ref int bilePointField, string colorName, Texture plusTexture)
		{
			var bilePlusButton = new Rect(point.x, point.y, 50, 50);
			GUI.DrawTexture(bilePlusButton, plusTexture);
			TooltipHandler.TipRegion(bilePlusButton, "Annely.PlusSignTooltip".Translate(CompEvolution.BilePointCost, colorName));

			if (bilePlusButton.IsLeftClicked())
			{
				if (bilePointField == CompEvolution.MaxBilePoints)
				{
					Messages.Message("Annely.MaxBilePoints".Translate(colorName), pawn, MessageTypeDefOf.CautionInput);
				}
				else if (compEvolution.evolutionPoints < CompEvolution.BilePointCost)
				{
					Messages.Message("Annely.NotEnoughEPs".Translate(), pawn, MessageTypeDefOf.CautionInput);
				}
				else
				{
					bilePointField += 1;
					compEvolution.evolutionPoints -= CompEvolution.BilePointCost;
				}
			}
			point.x -= 54;
			point.y += 52;
		}
	}
}
