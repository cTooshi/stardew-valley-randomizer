using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class PantryBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Creates a bundle for the pantry
		/// </summary>
		protected override void Populate()
		{
			BundleType = Globals.RNGGetAndRemoveRandomValueFromList(RoomBundleTypes);
			List<RequiredItem> potentialItems = new List<RequiredItem>();

			switch (BundleType)
			{
				case BundleTypes.PantryAnimal:
					Name = "Animal";
					potentialItems = RequiredItem.CreateList(ItemList.GetAnimalProducts());
					potentialItems.Add(new RequiredItem((int)ObjectIndexes.Hay, 25, 50));
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 2, RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.PantryQualityCrops:
					Name = "Quality Crops";
					potentialItems = RequiredItem.CreateList(ItemList.GetCrops());
					potentialItems.ForEach(x => x.MinimumQuality = ItemQualities.Gold);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Green;
					break;
				case BundleTypes.PantryQualityForagables:
					Name = "Quality Foragables";
					potentialItems = RequiredItem.CreateList(ItemList.GetForagables());
					potentialItems.ForEach(x => x.MinimumQuality = ItemQualities.Gold);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Green;
					break;
				case BundleTypes.PantryCooked:
					Name = "Cooked";
					potentialItems = RequiredItem.CreateList(ItemList.GetCookeditems());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = Range.GetRandomValue(3, 4);
					Color = BundleColors.Green;
					break;
				case BundleTypes.PantryFlower:
					Name = "Flower";
					potentialItems = RequiredItem.CreateList(ItemList.GetFlowers());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = RequiredItems.Count - 2;
					Color = BundleColors.Green;
					break;
				case BundleTypes.PantrySpringCrops:
					GenerateBundleForSeasonCrops(Seasons.Spring, BundleColors.Green);
					break;
				case BundleTypes.PantrySummerCrops:
					GenerateBundleForSeasonCrops(Seasons.Summer, BundleColors.Red);
					break;
				case BundleTypes.PantryFallCrops:
					GenerateBundleForSeasonCrops(Seasons.Fall, BundleColors.Orange);
					break;
				case BundleTypes.PantryWinterCrops:
					GenerateBundleForSeasonCrops(Seasons.Winter, BundleColors.Cyan);
					break;
				case BundleTypes.PantryEgg:
					Name = "Egg";
					potentialItems = RequiredItem.CreateList(
						ItemList.Items.Values.Where(x => x.Name.Contains("Egg") && x.Id > -4).ToList());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 3, RequiredItems.Count - 2);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.PantryRareFoods:
					Name = "Rare Foods";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.AncientFruit),
						new RequiredItem((int)ObjectIndexes.Starfruit),
						new RequiredItem((int)ObjectIndexes.SweetGemBerry)
					};
					Color = BundleColors.Blue;
					break;
				case BundleTypes.PantryDesert:
					Name = "Desert";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.IridiumOre, 5),
						Globals.RNGGetRandomValueFromList(new List<RequiredItem>
						{
							new RequiredItem((int)ObjectIndexes.GoldenMask),
							new RequiredItem((int)ObjectIndexes.GoldenRelic),
						}),
						new RequiredItem((int)ObjectIndexes.Sandfish), //TODO: change for the fish shuffle
						Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(ItemList.GetUniqueDesertForagables(), 1, 3)),
						new RequiredItem((int)ObjectIndexes.StarfruitSeeds, 5)
					};
					MinimumRequiredItems = 4;
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.PantryDessert:
					Name = "Dessert";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.CranberryCandy),
						new RequiredItem((int)ObjectIndexes.PlumPudding),
						new RequiredItem((int)ObjectIndexes.PinkCake),
						new RequiredItem((int)ObjectIndexes.PumpkinPie),
						new RequiredItem((int)ObjectIndexes.RhubarbPie),
						new RequiredItem((int)ObjectIndexes.Cookie),
						new RequiredItem((int)ObjectIndexes.IceCream),
						new RequiredItem((int)ObjectIndexes.MinersTreat),
						new RequiredItem((int)ObjectIndexes.BlueberryTart),
						new RequiredItem((int)ObjectIndexes.BlackberryCobbler),
						new RequiredItem((int)ObjectIndexes.MapleBar),
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = 4;
					Color = BundleColors.Cyan;
					break;
				case BundleTypes.PantryMexicanFood:
					Name = "Mexican Food";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Tortilla),
						new RequiredItem((int)ObjectIndexes.Corn, 1, 5),
						new RequiredItem((int)ObjectIndexes.Tomato, 1, 5),
						new RequiredItem((int)ObjectIndexes.HotPepper, 1, 5),
						new RequiredItem((int)ObjectIndexes.FishTaco),
						new RequiredItem((int)ObjectIndexes.Rice),
						new RequiredItem((int)ObjectIndexes.Cheese),
					};
					MinimumRequiredItems = Range.GetRandomValue(4, 5);
					Color = BundleColors.Red;
					break;
			}
		}

		/// <summary>
		/// Generates a bundle of crops belonging to the given season
		/// </summary>
		/// <param name="season">The season</param>
		/// <param name="color">The color of the bundle</param>
		private void GenerateBundleForSeasonCrops(Seasons season, BundleColors color)
		{
			Name = $"{season.ToString()} Crops";
			List<RequiredItem> potentialItems = RequiredItem.CreateList(ItemList.GetCrops(season));
			RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
			MinimumRequiredItems = 6;
			Color = color;
		}

		/// <summary>
		/// Generates the reward for completing a crafting room bundle
		/// </summary>
		protected override void GenerateReward()
		{
			var potentialRewards = new List<RequiredItem>
			{
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetResources()), 999),
				new RequiredItem((int)ObjectIndexes.Loom),
				new RequiredItem((int)ObjectIndexes.MayonnaiseMachine),
				new RequiredItem((int)ObjectIndexes.Heater),
				new RequiredItem((int)ObjectIndexes.AutoGrabber),
				new RequiredItem((int)ObjectIndexes.SeedMaker),
				//new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetCrops()), 25, 50), //TODO: uncomment when crops are done
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetCookeditems())),
				//new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetSeeds()), 50 100), // TODO: uncomment when crops are done
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetAnimalProducts()), 25, 50),
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
