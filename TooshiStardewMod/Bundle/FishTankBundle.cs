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
						Globals.RNGGetRandomValuesFromList(FishItem.GetForTime(800), 8)
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
					potentialItems = RequiredItem.CreateList(new List<int>
					{
						(int)ObjectIndexes.Spinner,
						(int)ObjectIndexes.DressedSpinner,
						(int)ObjectIndexes.TrapBobber,
						(int)ObjectIndexes.CorkBobber,
						(int)ObjectIndexes.LeadBobber,
						(int)ObjectIndexes.TreasureHunter,
						(int)ObjectIndexes.BarbedHook, //TODO:FINISHME
					});
					break;
				case BundleTypes.FishTankUnique:
					break;
					//TODO: you CAN do location bundle now!@!!
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
			MinimumRequiredItems = Math.Min(Range.GetRandomValue(6, 8), potentialItems.Count);
			Color = color;
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			Reward = new RequiredItem((int)ObjectIndexes.Wood); //TODO: complete this ha
		}
	}
}
