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

        public override void OnOpen()
        {
            base.OnOpen();
			pawn = PawnToShowInfoAbout;
			compEvolution = pawn.TryGetComp<CompEvolution>();
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
		[TweakValue("AN", 0, 1000)] public static float redW;
		[TweakValue("AN", 0, 1000)] public static float redH;
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

			Text.Font = curFont;
			GUI.color = curColor;
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
