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

			BundleType = BundleTypes.FishTankLegendary; //TODO: removeme
			switch (BundleType)
			{
				case BundleTypes.FishTankSpringFish:
					break;
				case BundleTypes.FishTankSummerFish:
					break;
				case BundleTypes.FishTankFallFish:
					break;
				case BundleTypes.FishTankWinterFish:
					break;
				case BundleTypes.FishTankOceanFood:
					break;
				case BundleTypes.FishTankLegendary:
					Name = "Legendary";
					RequiredItems = RequiredItem.CreateList(FishItem.GetLegendaries().Cast<Item>().ToList());
					MinimumRequiredItems = Range.GetRandomValue(3, 4);
					Color = BundleColors.Red;
					break;
				case BundleTypes.FishTankRainFish:
					break;
				case BundleTypes.FishTankNightFish:
					break;
				case BundleTypes.FishTankQualityFish:
					break;
				case BundleTypes.FishTankBeachForagables:
					break;
				case BundleTypes.FishTankFishingTools:
					break;
				case BundleTypes.FishTankUnique:
					break;
					//TODO: you CAN do location bundle now!@!!
			}
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
