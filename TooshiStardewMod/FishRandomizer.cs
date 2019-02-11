using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Randomizes fish
	/// </summary>
	public class FishRandomizer
	{
		public static void Randomize(EditedObjectInformation editedObjectInfo)
		{
			List<FishItem> legendaryFish = FishItem.GetLegendaries().Cast<FishItem>().ToList();
			List<FishItem> normalFish = FishItem.Get().Cast<FishItem>().ToList();
			List<FishItem> normalFishCopy = new List<FishItem>();
			foreach (FishItem fish in normalFish)
			{
				FishItem fishInfo = new FishItem(fish.Id, true);
				CopyFishInfo(fish, fishInfo);
				normalFishCopy.Add(fishInfo);
			}

			List<string> fishNames = NameAndDescriptionRandomizer.GenerateFishNames(normalFish.Count + legendaryFish.Count);
			foreach (FishItem fish in normalFish)
			{
				CopyFishInfo(Globals.RNGGetAndRemoveRandomValueFromList(normalFishCopy), fish);
				fish.DartChance = GenerateRandomFishDifficulty();
				fish.BehaviorType = Globals.RNGGetRandomValueFromList(
					Enum.GetValues(typeof(FishBehaviorType)).Cast<FishBehaviorType>().ToList());
				fish.OverrideName = Globals.RNGGetAndRemoveRandomValueFromList(fishNames);

				editedObjectInfo.FishReplacements.Add(fish.Id, fish.ToString());
				editedObjectInfo.ObjectInformationReplacements.Add(fish.Id, GetFishObjectInformation(fish));
			}

			foreach (FishItem fish in legendaryFish)
			{
				fish.BehaviorType = Globals.RNGGetRandomValueFromList(
					Enum.GetValues(typeof(FishBehaviorType)).Cast<FishBehaviorType>().ToList());
				fish.OverrideName = Globals.RNGGetAndRemoveRandomValueFromList(fishNames);

				editedObjectInfo.FishReplacements.Add(fish.Id, fish.ToString());
				editedObjectInfo.ObjectInformationReplacements.Add(fish.Id, GetFishObjectInformation(fish));
			}
		}

		/// <summary>
		/// Copies a select set of info from one fish to another
		/// </summary>
		/// <param name="fromFish">The fish to copy from</param>
		/// <param name="toFish">The fish to copy to</param>
		private static void CopyFishInfo(FishItem fromFish, FishItem toFish)
		{
			toFish.Times = new Range(fromFish.Times.MinValue, fromFish.Times.MaxValue);
			toFish.ExcludedTimes = new Range(fromFish.ExcludedTimes.MinValue, fromFish.ExcludedTimes.MaxValue);
			toFish.Weathers = new List<Weather>(fromFish.Weathers);
			toFish.AvailableLocations = new List<Locations>(fromFish.AvailableLocations);
			toFish.AvailableSeasons = new List<Seasons>(fromFish.AvailableSeasons);
		}

		/// <summary>
		/// Gets a random fish difficulty value
		/// </summary>
		/// <returns>
		/// 25% Easy: 15 - 30
		/// 45% Moderate: 31 - 50
		/// 25% Difficult: 51 - 75
		/// 5% WTF: 76 - 95
		/// 0% Legendary: 96 - 110
		/// </returns>
		private static int GenerateRandomFishDifficulty()
		{
			int difficultyRange = Range.GetRandomValue(1, 100);
			if (difficultyRange < 26)
			{
				return Range.GetRandomValue(15, 30);
			}
			else if (difficultyRange < 71)
			{
				return Range.GetRandomValue(31, 50);
			}
			else if (difficultyRange < 96)
			{
				return Range.GetRandomValue(51, 75);
			}
			else
			{
				return Range.GetRandomValue(76, 95);
			}
		}

		/// <summary>
		/// Gets the fish object info string
		/// </summary>
		/// <param name="fish">The fish</param>
		/// <returns />
		private static string GetFishObjectInformation(FishItem fish)
		{
			string defaultObjectInfo = FishData.DefaultObjectInformation[fish.Id];
			string[] objectInfoParts = defaultObjectInfo.Split('/');

			objectInfoParts[0] = fish.OverrideName;
			objectInfoParts[4] = fish.OverrideName;
			objectInfoParts[5] = fish.Description;
			objectInfoParts[6] = fish.ObjectInformationSuffix;

			return string.Join("/", objectInfoParts);
		}
	}
}
