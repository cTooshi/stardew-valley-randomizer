using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class BulletinBoardBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Populates the bundle with the name, required items, minimum required, and color
		/// </summary>
		protected override void Populate()
		{
			BundleType = Globals.RNGGetAndRemoveRandomValueFromList(RoomBundleTypes);
			List<RequiredItem> potentialItems = new List<RequiredItem>();

			switch (BundleType)
			{
				case BundleTypes.BulletinNews:
					Name = "FAKE NEWS";
					potentialItems = new List<RequiredItem>()
					{
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(1, 8));
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinCleanup:
					Name = "Cleanup";
					RequiredItems = RequiredItem.CreateList(ItemList.GetTrash(), 1, 5);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinHated:
					Name = "Hated";
					potentialItems = RequiredItem.CreateList(NPC.UniversalHates);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinLoved:
					Name = "Loved";
					RequiredItems = RequiredItem.CreateList(NPC.UniversalLoves);
					MinimumRequiredItems = 2;
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinAbigail:
					Name = "Abigail";
					potentialItems = RequiredItem.CreateList(Abigail.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinAlex:
					Name = "Alex";
					potentialItems = RequiredItem.CreateList(Alex.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinCaroline:
					Name = "Caroline";
					potentialItems = RequiredItem.CreateList(Caroline.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinClint:
					Name = "Clint";
					potentialItems = RequiredItem.CreateList(Clint.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinDwarf:
					Name = "Dwarf";
					potentialItems = RequiredItem.CreateList(Dwarf.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinDemetrius:
					Name = "Demetrius";
					potentialItems = RequiredItem.CreateList(Demetrius.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinElliot:
					Name = "Elliot";
					potentialItems = RequiredItem.CreateList(Elliot.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinEmily:
					Name = "Emily";
					potentialItems = RequiredItem.CreateList(Emily.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinEvelyn:
					Name = "Evelyn";
					potentialItems = RequiredItem.CreateList(Evelyn.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinGeorge:
					Name = "George";
					potentialItems = RequiredItem.CreateList(George.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinGus:
					Name = "Gus";
					potentialItems = RequiredItem.CreateList(Gus.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinHaley:
					Name = "Haley";
					potentialItems = RequiredItem.CreateList(Haley.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinHarvey:
					Name = "Harvey";
					potentialItems = RequiredItem.CreateList(Harvey.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinJas:
					Name = "Jas";
					potentialItems = RequiredItem.CreateList(Jas.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinJodi:
					Name = "Jodi";
					potentialItems = RequiredItem.CreateList(Jodi.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinKent:
					Name = "Kent";
					potentialItems = RequiredItem.CreateList(Kent.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinKrobus:
					Name = "Krobus";
					potentialItems = RequiredItem.CreateList(Krobus.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinLeah:
					Name = "Leah";
					potentialItems = RequiredItem.CreateList(Leah.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinLewis:
					Name = "Lewis";
					potentialItems = RequiredItem.CreateList(Lewis.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinLinus:
					Name = "Linus";
					potentialItems = RequiredItem.CreateList(Linus.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinMarnie:
					Name = "Marnie";
					potentialItems = RequiredItem.CreateList(Marnie.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinMaru:
					Name = "Maru";
					potentialItems = RequiredItem.CreateList(Maru.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinPam:
					Name = "Pam";
					potentialItems = RequiredItem.CreateList(Pam.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinPenny:
					Name = "Penny";
					potentialItems = RequiredItem.CreateList(Penny.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BulletinPierre:
					Name = "Pierre";
					potentialItems = RequiredItem.CreateList(Pierre.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinRobin:
					Name = "Robin";
					potentialItems = RequiredItem.CreateList(Robin.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BulletinSam:
					Name = "Sam";
					potentialItems = RequiredItem.CreateList(Sam.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinSandy:
					Name = "Sandy";
					potentialItems = RequiredItem.CreateList(Sandy.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinSebastian:
					Name = "Sebastian";
					potentialItems = RequiredItem.CreateList(Sebastian.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinShane:
					Name = "Shane";
					potentialItems = RequiredItem.CreateList(Shane.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinVincent:
					Name = "Vincent";
					potentialItems = RequiredItem.CreateList(Vincent.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinWilly:
					Name = "Willy";
					potentialItems = RequiredItem.CreateList(Willy.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinWizard:
					Name = "Wizard";
					potentialItems = RequiredItem.CreateList(Wizard.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
			}
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			if (Globals.RNGGetNextBoolean(1))
			{
				Reward = new RequiredItem((int)ObjectIndexes.PrismaticShard);
			}

			else if (Globals.RNGGetNextBoolean(5))
			{
				List<Item> universalLoves = NPC.UniversalLoves.Where(x =>
					x.Id != (int)ObjectIndexes.PrismaticShard).ToList();

				Reward = Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(universalLoves, 5, 10));
			}

			List<RequiredItem> potentialRewards = new List<RequiredItem>
			{
				new RequiredItem((int)ObjectIndexes.JunimoKartArcadeSystem),
				new RequiredItem((int)ObjectIndexes.PrairieKingArcadeSystem),
				new RequiredItem((int)ObjectIndexes.SodaMachine),
				new RequiredItem((int)ObjectIndexes.Beer, 43),
				new RequiredItem((int)ObjectIndexes.Salad, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Bread, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Spaghetti, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Pizza, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Coffee, Range.GetRandomValue(5, 25))
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
