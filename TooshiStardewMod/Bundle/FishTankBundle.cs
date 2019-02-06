using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class FishTankBundle : Bundle
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
				case BundleTypes.FishTankSpringFish:
					GenerateSeasonBundle(Seasons.Spring, BundleColors.Green);
					break;
				case BundleTypes.FishTankSummerFish:
					GenerateSeasonBundle(Seasons.Summer, BundleColors.Red);
					break;
				case BundleTypes.FishTankFallFish:
					GenerateSeasonBundle(Seasons.Fall, BundleColors.Orange);
					break;
				case BundleTypes.FishTankWinterFish:
					GenerateSeasonBundle(Seasons.Winter, BundleColors.Cyan);
					break;
				case BundleTypes.FishTankOceanFood:
					Name = "Fish Food";
					potentialItems = RequiredItem.CreateList(new List<int>
					{
						(int)ObjectIndexes.CrispyBass,
						(int)ObjectIndexes.FriedEel,
						(int)ObjectIndexes.AlgaeSoup,
						(int)ObjectIndexes.CrabCakes,
						(int)ObjectIndexes.SpicyEel,
						(int)ObjectIndexes.PaleBroth,
						(int)ObjectIndexes.Sashimi,
						(int)ObjectIndexes.MakiRoll,
						(int)ObjectIndexes.TomKhaSoup,
						(int)ObjectIndexes.BakedFish,
						(int)ObjectIndexes.TroutSoup,
						(int)ObjectIndexes.Chowder,
						(int)ObjectIndexes.LobsterBisque,
						(int)ObjectIndexes.DishOTheSea,
						(int)ObjectIndexes.FishStew,
						(int)ObjectIndexes.FriedCalamari,
						(int)ObjectIndexes.SalmonDinner,
						(int)ObjectIndexes.FishTaco
					});
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = 4;
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.FishTankLegendary:
					Name = "Legendary";
					RequiredItems = RequiredItem.CreateList(FishItem.GetLegendaries().Cast<Item>().ToList());
					MinimumRequiredItems = Range.GetRandomValue(3, 4);
					Color = BundleColors.Red;
					break;
				case BundleTypes.FishTankLocation:
					List<Locations> locations = new List<Locations>
					{
						Locations.Town,
						Locations.Mountain,
						Locations.Desert,
						Locations.Woods,
						Locations.Forest,
						Locations.NightMarket,
						Locations.Beach
					};
					Locations location = Globals.RNGGetRandomValueFromList(locations);

					Name = location.ToString();
					RequiredItems = RequiredItem.CreateList(Globals.RNGGetRandomValuesFromList(FishItem.Get(location), 8));
					MinimumRequiredItems = Math.Min(RequiredItems.Count, Range.GetRandomValue(2, 4));
					Color = BundleColors.Blue;
					break;
				case BundleTypes.FishTankRainFish:
					Name = "Rain Fish";
					RequiredItems = RequiredItem.CreateList(
						Globals.RNGGetRandomValuesFromList(FishItem.Get(Weather.Rain), 8)
					);
					MinimumRequiredItems = Math.Min(RequiredItems.Count, 4);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.FishTankNightFish:
					Name = "Night Fish";
					RequiredItems = RequiredItem.CreateList(
						Globals.RNGGetRandomValuesFromList(FishItem.GetNightFish(), 8)
					);
					MinimumRequiredItems = Math.Min(RequiredItems.Count, 4);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.FishTankQualityFish:
					Name = "Quality Fish";
					potentialItems = RequiredItem.CreateList(
						Globals.RNGGetRandomValuesFromList(FishItem.Get(), 8)
					);
					potentialItems.ForEach(x => x.MinimumQuality = ItemQualities.Gold);
					RequiredItems = potentialItems;
					MinimumRequiredItems = 4;
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.FishTankBeachForagables:
					Name = "Beach";
					RequiredItems = RequiredItem.CreateList(
						Globals.RNGGetRandomValuesFromList(ItemList.GetUniqueBeachForagables(), 6)
					);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.FishTankFishingTools:
					Name = "Fishing Tools";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Spinner, 1),
						new RequiredItem((int)ObjectIndexes.DressedSpinner, 1),
						new RequiredItem((int)ObjectIndexes.TrapBobber, 1),
						new RequiredItem((int)ObjectIndexes.CorkBobber, 1),
						new RequiredItem((int)ObjectIndexes.LeadBobber, 1),
						new RequiredItem((int)ObjectIndexes.TreasureHunter, 1),
						new RequiredItem((int)ObjectIndexes.Bait, 25, 50),
						new RequiredItem((int)ObjectIndexes.WildBait, 10, 20)
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 4);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.FishTankUnique:
					Name = "Unique Fish";

					List<Item> nightFish = FishItem.Get(Locations.NightMarket);
					List<Item> minesFish = FishItem.Get(Locations.UndergroundMine);
					List<Item> desertFish = FishItem.Get(Locations.Desert);
					List<Item> woodsFish = FishItem.Get(Locations.Woods);

					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem(Globals.RNGGetRandomValueFromList(nightFish)),
						new RequiredItem(Globals.RNGGetRandomValueFromList(minesFish)),
						new RequiredItem(Globals.RNGGetRandomValueFromList(desertFish)),
						new RequiredItem(Globals.RNGGetRandomValueFromList(woodsFish))
					};
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 1, RequiredItems.Count);
					Color = BundleColors.Cyan;
					break;
			}
		}

		/// <summary>
		/// Generates the bundle for the given season
		/// </summary>
		/// <param name="season">The season</param>
		/// <param name="color">The color to use</param>
		private void GenerateSeasonBundle(Seasons season, BundleColors color)
		{
			Name = $"{season.ToString()} Fish";
			List<RequiredItem> potentialItems = RequiredItem.CreateList(FishItem.Get(season));
			RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
			MinimumRequiredItems = Math.Min(Range.GetRandomValue(6, 8), RequiredItems.Count);
			Color = color;
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			var tackles = new List<RequiredItem>
			{
				new RequiredItem((int)ObjectIndexes.Spinner, 1),
				new RequiredItem((int)ObjectIndexes.DressedSpinner),
				new RequiredItem((int)ObjectIndexes.TrapBobber),
				new RequiredItem((int)ObjectIndexes.CorkBobber),
				new RequiredItem((int)ObjectIndexes.LeadBobber),
				new RequiredItem((int)ObjectIndexes.TreasureHunter)
			};

			var potentialRewards = new List<RequiredItem>
			{
				new RequiredItem((int)ObjectIndexes.RecyclingMachine),
				new RequiredItem((int)ObjectIndexes.Bait, 500),
				new RequiredItem((int)ObjectIndexes.WildBait, 500),
				Globals.RNGGetRandomValueFromList(tackles),
				Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(FishItem.Get(), 25, 50)),
				Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(ItemList.GetUniqueBeachForagables(), 25, 50)),
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
