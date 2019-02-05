using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class CraftingRoomBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Creates a bundle for the crafts room
		/// </summary>
		protected override void Populate()
		{
			// Force one resource bundle so that there's one possible bundle to complete
			if (!RoomBundleTypes.Contains(BundleTypes.CraftingResource))
			{
				BundleType = Globals.RNGGetAndRemoveRandomValueFromList(RoomBundleTypes);
			}
			else
			{
				RoomBundleTypes.Remove(BundleTypes.CraftingResource);
				BundleType = BundleTypes.CraftingResource;
			}

			List<RequiredItem> potentialItems;
			int numberOfChoices;
			switch (BundleType)
			{
				case BundleTypes.CraftingResource:
					Name = "Resource";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Wood, 100, 250),
						new RequiredItem((int)ObjectIndexes.Stone, 100, 250),
						new RequiredItem((int)ObjectIndexes.Fiber, 10, 50),
						new RequiredItem((int)ObjectIndexes.Clay, 10, 50),
						new RequiredItem((int)ObjectIndexes.Hardwood, 1, 10)
					};
					Color = BundleColors.Orange;
					break;
				case BundleTypes.CraftingHappyCrops:
					Name = "Happy Crops";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Sprinkler, 1, 5),
						new RequiredItem((int)ObjectIndexes.QualitySprinkler, 1, 5),
						new RequiredItem((int)ObjectIndexes.IridiumSprinkler, 1),
						new RequiredItem((int)ObjectIndexes.BasicFertilizer, 10, 20),
						new RequiredItem((int)ObjectIndexes.QualityFertilizer, 10, 20),
						new RequiredItem((int)ObjectIndexes.BasicRetainingSoil, 10, 20),
						new RequiredItem((int)ObjectIndexes.QualityRetainingSoil, 10, 20),
						//TODO: add one random quality crop
					};
					numberOfChoices = Range.GetRandomValue(6, 8);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, numberOfChoices);
					MinimumRequiredItems = Range.GetRandomValue(numberOfChoices - 2, numberOfChoices);
					Color = BundleColors.Green;
					break;
				case BundleTypes.CraftingTree:
					Name = "Tree";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.MapleSeed, 1, 5),
						new RequiredItem((int)ObjectIndexes.Acorn, 1, 5),
						new RequiredItem((int)ObjectIndexes.PineCone, 1),
						new RequiredItem((int)ObjectIndexes.OakResin, 1),
						new RequiredItem((int)ObjectIndexes.MapleSyrup, 1),
						new RequiredItem((int)ObjectIndexes.PineTar, 1),
						new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetFruit()), 1),
						new RequiredItem((int)ObjectIndexes.Wood, 100, 200),
						new RequiredItem((int)ObjectIndexes.Hardwood, 25, 50),
						new RequiredItem((int)ObjectIndexes.Driftwood, 5, 10),
					};
					numberOfChoices = Range.GetRandomValue(6, 8);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, numberOfChoices);
					MinimumRequiredItems = Range.GetRandomValue(numberOfChoices - 2, numberOfChoices);
					Color = BundleColors.Green;
					break;
				case BundleTypes.CraftingTotems:
					Name = "Totems";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.WarpTotemFarm),
						new RequiredItem((int)ObjectIndexes.WarpTotemBeach),
						new RequiredItem((int)ObjectIndexes.WarpTotemMountains),
						new RequiredItem((int)ObjectIndexes.RainTotem),
					};
					MinimumRequiredItems = Range.GetRandomValue(3, 4);
					Color = BundleColors.Red;
					break;
				case BundleTypes.CraftingBindle:
					Name = "Bindle";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Cloth),
						new RequiredItem((int)ObjectIndexes.ChewingStick),
						new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetCookeditems())),
						new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetForagables())),
						new RequiredItem(Globals.RNGGetRandomValueFromList(
							ItemList.GetFish().Where(x => x.DifficultyToObtain != ObtainingDifficulties.EndgameItem).ToList()).Id
						),
						new RequiredItem(Globals.RNGGetRandomValueFromList(
							ItemList.Items.Values.Where(x => x.Id >= -4 && x.DifficultyToObtain <= ObtainingDifficulties.LargeTimeRequirements).ToList()).Id
						),
					};
					numberOfChoices = Range.GetRandomValue(4, 5);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, numberOfChoices);
					MinimumRequiredItems = Range.GetRandomValue(numberOfChoices - 1, numberOfChoices);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.CraftingSpringForaging:
					GenerateForagingBundle(Seasons.Spring, BundleColors.Green);
					break;
				case BundleTypes.CraftingSummerForaging:
					GenerateForagingBundle(Seasons.Summer, BundleColors.Red);
					break;
				case BundleTypes.CraftingFallForaging:
					GenerateForagingBundle(Seasons.Fall, BundleColors.Orange);
					break;
				case BundleTypes.CraftingWinterForaging:
					GenerateForagingBundle(Seasons.Winter, BundleColors.Cyan);
					break;
					//TODO: make the colored bundles here instead of in all
			}
		}

		/// <summary>
		/// Generates the bundle for foraging items
		/// </summary>
		/// <param name="season">The season</param>
		/// <param name="color">The color of the bundle</param>
		private void GenerateForagingBundle(Seasons season, BundleColors color)
		{
			Name = $"{season.ToString()} Foraging";
			List<RequiredItem> potentialItems = RequiredItem.CreateList(ItemList.GetForagables(season));
			int numberOfChoices = Math.Min(potentialItems.Count, 8);
			RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, numberOfChoices);
			MinimumRequiredItems = Range.GetRandomValue(4, numberOfChoices);
			Color = color;
		}

		/// <summary>
		/// Generates the reward for completing a crafting room bundle
		/// </summary>
		/// <returns />
		protected override void GenerateReward()
		{
			var potentialRewards = new List<RequiredItem>
			{
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetResources()), 999),
				new RequiredItem((int)ObjectIndexes.Sprinkler, 2, 5),
				new RequiredItem((int)ObjectIndexes.QualitySprinkler, 1, 4),
				new RequiredItem((int)ObjectIndexes.IridiumSprinkler, 1, 3),
				new RequiredItem((int)ObjectIndexes.BasicFertilizer, 100),
				new RequiredItem((int)ObjectIndexes.QualityFertilizer, 100),
				new RequiredItem((int)ObjectIndexes.BasicRetainingSoil, 100),
				new RequiredItem((int)ObjectIndexes.QualityRetainingSoil, 100),
				new RequiredItem((int)ObjectIndexes.OakResin, 25, 50),
				new RequiredItem((int)ObjectIndexes.MapleSyrup, 25, 50),
				new RequiredItem((int)ObjectIndexes.PineTar, 25, 50),
				new RequiredItem((int)ObjectIndexes.Acorn, 25, 50),
				new RequiredItem((int)ObjectIndexes.MapleSeed, 25, 50),
				new RequiredItem((int)ObjectIndexes.PineCone, 25, 50),
				new RequiredItem((int)ObjectIndexes.SpringSeeds, 25, 50),
				new RequiredItem((int)ObjectIndexes.SummerSeeds, 25, 50),
				new RequiredItem((int)ObjectIndexes.FallSeeds, 25, 50),
				new RequiredItem((int)ObjectIndexes.WinterSeeds, 25, 50),
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetForagables()), 10, 20),
				new RequiredItem((int)ObjectIndexes.SeedMaker)
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
