using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Randomizes fruit trees
	/// </summary>
	public class CropRandomizer
	{
		public static EditedObjectInformation Randomize()
		{
			EditedObjectInformation editedObjectInfo = new EditedObjectInformation();
			RandomizeCrops(editedObjectInfo);
			RandomizeFruitTrees(editedObjectInfo);
			return editedObjectInfo;
		}

		/// <summary>
		/// Randomize fruit tree information
		/// </summary>
		/// <param name="editedObjectInfo">The edited object information</param>
		private static void RandomizeFruitTrees(EditedObjectInformation editedObjectInfo)
		{
			List<Item> allPotentialTrees = ItemList.Items.Values.Where(x =>
				x.DifficultyToObtain < ObtainingDifficulties.Impossible
			).ToList();

			Item tree1 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree2 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree3 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree4 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree5 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree6 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);

			string[] seasons = { "spring", "spring", "summer", "summer", "fall", "fall" };
			seasons[Globals.RNG.Next(0, 6)] = "winter";

			// TODO: These prices don't actually seem to work for fruit trees
			int[] prices =
			{
				tree1.GetPriceForObtainingDifficulty(0.2),
				tree2.GetPriceForObtainingDifficulty(0.2),
				tree3.GetPriceForObtainingDifficulty(0.2),
				tree4.GetPriceForObtainingDifficulty(0.2),
				tree5.GetPriceForObtainingDifficulty(0.2),
				tree6.GetPriceForObtainingDifficulty(0.2)
			};

			// Fruit tree asset replacements
			var fruitTreeReplacements = new Dictionary<int, string>
			{
				{ (int)ObjectIndexes.CherrySapling, $"0/{seasons[0]}/{tree1.Id}/{prices[0]}" },
				{ (int)ObjectIndexes.ApricotSapling, $"1/{seasons[1]}/{tree2.Id}/{prices[1]}"},
				{ (int)ObjectIndexes.OrangeSapling, $"2/{seasons[2]}/{tree3.Id}/{prices[2]}"},
				{ (int)ObjectIndexes.PeachSapling, $"3/{seasons[3]}/{tree4.Id}/{prices[3]}"},
				{ (int)ObjectIndexes.PomegranateSapling, $"4/{seasons[4]}/{tree5.Id}/{prices[4]}"},
				{ (int)ObjectIndexes.AppleSapling, $"5/{seasons[5]}/{tree6.Id}/{prices[5]}"},
			};

			foreach (KeyValuePair<int, string> pair in fruitTreeReplacements)
			{
				editedObjectInfo.FruitTreeReplacements[pair.Key] = pair.Value;
			}

			// Fruit tree item/shop info replacements
			Random rng = Globals.RNG;
			var objectReplacements = new Dictionary<int, string>
			{
				{ (int)ObjectIndexes.CherrySapling, $"{tree1.Name} Sapling/{prices[0] / 2}/-300/Basic -74/{tree1.Name} Sapling/Takes 28 days to produce a mature {tree1.Name} tree. Bears item in the {seasons[0]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.ApricotSapling, $"{tree2.Name} Sapling/{prices[1] / 2}/-300/Basic -74/{tree2.Name} Sapling/Takes 28 days to produce a mature {tree2.Name} tree. Bears item in the {seasons[1]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.OrangeSapling, $"{tree3.Name} Sapling/{prices[2] / 2}/-300/Basic -74/{tree3.Name} Sapling/Takes 28 days to produce a mature {tree3.Name} tree. Bears item in the {seasons[2]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.PeachSapling, $"{tree4.Name} Sapling/{prices[3] / 2}/-300/Basic -74/{tree4.Name} Sapling/Takes 28 days to produce a mature {tree4.Name} tree. Bears item in the {seasons[3]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.AppleSapling, $"{tree5.Name} Sapling/{prices[4] / 2}/-300/Basic -74/{tree5.Name} Sapling/Takes 28 days to produce a mature {tree5.Name} tree. Bears item in the {seasons[4]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.PomegranateSapling, $"{tree6.Name} Sapling/{prices[5] / 2}/-300/Basic -74/{tree6.Name} Sapling/Takes 28 days to produce a mature {tree6.Name} tree. Bears item in the {seasons[5]}. Only grows if the 8 surrounding \"tiles\" are empty."},
			};

			foreach (KeyValuePair<int, string> pair in objectReplacements)
			{
				editedObjectInfo.ObjectInformationReplacements[pair.Key] = pair.Value;
			}
		}

		/// <summary>
		/// Randomizes the crops - currently only does prices, and only for seasonal crops
		/// </summary>
		/// <param name="editedObjectInfo">The edited object information</param>
		/// crop format: name/price/-300/Seeds -74/name/tooltip
		private static void RandomizeCrops(EditedObjectInformation editedObjectInfo)
		{
			List<SeedItem> seedsToRandomize = ItemList.GetSeeds().Cast<SeedItem>()
				.Where(x => x.Randomize)
				.ToList();

			List<int> seedIdsToRandomize = seedsToRandomize.Select(x => x.Id).ToList();
			List<int> seedIdsToRandomizeCopy = new List<int>(seedIdsToRandomize);

			// Fill up a dictionary to remap the seed values
			Dictionary<int, int> seedMappings = new Dictionary<int, int>(); // Original value, new value
			foreach (int originalSeedId in CropGrowthInformation.CropIdsToInfo.Keys)
			{
				if (seedIdsToRandomize.Contains(originalSeedId))
				{
					seedMappings.Add(originalSeedId, Globals.RNGGetAndRemoveRandomValueFromList(seedIdsToRandomizeCopy));
				}
			}

			// Loop through the dictionary and reassign the values, keeping the seasons the same as before
			foreach (KeyValuePair<int, int> seedMapping in seedMappings)
			{
				int originalValue = seedMapping.Key;
				int newValue = seedMapping.Value;

				CropGrowthInformation cropInfoToAdd = CropGrowthInformation.ParseString(CropGrowthInformation.DefaultStringData[newValue]);
				cropInfoToAdd.GrowingSeasons = CropGrowthInformation.ParseString(CropGrowthInformation.DefaultStringData[originalValue]).GrowingSeasons;

				//TODO: modify the growth cycles, scythe stuff, regrowth, etc.
				CropGrowthInformation.CropIdsToInfo[originalValue] = cropInfoToAdd;
			}

			// Set the object info
			List<CropItem> randomizedCrops = ItemList.GetCrops(true).Cast<CropItem>()
				.Where(x => seedIdsToRandomize.Contains(x.MatchingSeedItem.Id))
				.ToList();

			List<string> vegetableNames = CropNameRandomizer.GenerateVegetableNames(randomizedCrops.Count + 1);
			SetCropAndSeedInformation(
				editedObjectInfo,
				randomizedCrops.Where(x => !x.IsFlower).ToList(),
				vegetableNames);

			SetUpCoffee(editedObjectInfo, vegetableNames[vegetableNames.Count - 1]);

			SetCropAndSeedInformation(
				editedObjectInfo,
				randomizedCrops.Where(x => x.IsFlower).ToList(),
				CropNameRandomizer.GenerateFlowerNames(randomizedCrops.Count));
		}

		/// <summary>
		/// Sets the ToString information for the given crops
		/// </summary>
		/// <param name="editedObjectInfo">The object info containing changes to apply</param>
		/// <param name="crops">The crops to set</param>
		/// <param name="randomNames">The random names to give the crops</param>
		private static void SetCropAndSeedInformation(
			EditedObjectInformation editedObjectInfo,
			List<CropItem> crops,
			List<string> randomNames)
		{
			for (int i = 0; i < crops.Count; i++)
			{
				CropItem crop = crops[i];
				string name = randomNames[i];
				crop.OverrideName = name;

				SeedItem seed = ItemList.GetSeedFromCrop(crop);
				seed.OverrideName = $"{name} {(seed.CropGrowthInfo.IsTrellisCrop ? "Starter" : "Seeds")}";

				editedObjectInfo.ObjectInformationReplacements[crop.Id] = crop.ToString();
				editedObjectInfo.ObjectInformationReplacements[seed.Id] = seed.ToString();
			}
		}

		/// <summary>
		/// Sets up the coffee beans and coffee objects
		/// </summary>
		/// <param name="editedObjectInfo">The object info containing changes to apply</param>
		/// <param name="coffeeName">The name of the coffee item</param>
		private static void SetUpCoffee(EditedObjectInformation editedObjectInfo, string coffeeName)
		{
			Item coffeeBean = ItemList.Items[(int)ObjectIndexes.CoffeeBean];
			coffeeBean.OverrideName = $"{coffeeName} Bean";
			editedObjectInfo.ObjectInformationReplacements[(int)ObjectIndexes.CoffeeBean] = coffeeBean.ToString();

			Item coffee = ItemList.Items[(int)ObjectIndexes.Coffee];
			coffee.OverrideName = $"Hot {coffeeName}";
			editedObjectInfo.ObjectInformationReplacements[(int)ObjectIndexes.Coffee] = coffee.ToString();
		}
	}
}
