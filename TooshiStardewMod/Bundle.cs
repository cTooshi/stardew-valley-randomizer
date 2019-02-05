using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Randomizer
{
	/// <summary>
	/// Represents a bundle
	/// </summary>
	public class Bundle
	{
		public CommunityCenterRooms Room { get; set; }
		public int Id { get; set; }
		public string Key
		{
			get
			{
				string roomName = Regex.Replace(Room.ToString(), @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
				return $"{roomName}/{Id}";
			}
		}
		public string Name { get; set; }
		public RequiredItem Reward { get; set; }
		public List<RequiredItem> RequiredItems { get; set; }
		public BundleColors Color { get; set; }
		public int? MinimumRequiredItems { get; set; }
		public BundleTypes BundleType { get; set; } = BundleTypes.None;

		private static List<BundleTypes> _allBundleTypes =
			Enum.GetValues(typeof(BundleTypes))
				.Cast<BundleTypes>()
				.Where(x => x.ToString().StartsWith("All"))
				.ToList();

		private static List<BundleTypes> _craftingRoomBundleTypes =
			Enum.GetValues(typeof(BundleTypes))
				.Cast<BundleTypes>()
				.Where(x => x.ToString().StartsWith("Crafting"))
				.ToList();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="room">The room the bundle is in</param>
		/// <param name="id">The id of the bundle</param>
		public Bundle(CommunityCenterRooms room, int id, List<BundleTypes> bundleTypesToExclude)
		{
			Room = room;
			Id = id;
			Initialize(bundleTypesToExclude);
		}

		/// <summary>
		/// Gets the string to be used in the bundle dictionary
		/// </summary>
		/// <returns>
		/// bundle name/reward/possible items required/color/min items needed (optional)
		/// </returns>
		public override string ToString()
		{
			string rewardStringPrefix = GetRewardStringPrefix();
			int itemId = Reward.Item.Id;
			if (rewardStringPrefix == "BO")
			{
				itemId = ItemList.BigCraftableItems[itemId];
			}
			string rewardString = $"{rewardStringPrefix} {itemId} {Reward.NumberOfItems}";
			string minRequiredItemsString = "";
			if (Room != CommunityCenterRooms.Vault && MinimumRequiredItems != null && MinimumRequiredItems > 0)
			{
				minRequiredItemsString = $"/{MinimumRequiredItems.ToString()}";
			}

			return $"{Name}/{rewardString}/{GetRewardStringForRequiredItems()}/{Color:D}{minRequiredItemsString}";
		}

		/// <summary>
		/// Gets the prefix used before the reward string
		/// </summary>
		/// <returns>R if a ring; BO if a BigCraftableObject; O otherwise</returns>
		private string GetRewardStringPrefix()
		{
			if (Reward?.Item == null)
			{
				Globals.ConsoleWrite($"ERROR: No reward item defined for bundle: {Name}");
				return "O 388 1";
			}

			if (Reward.Item.IsRing)
			{
				return "R";
			}

			if (Reward.Item.Id <= -100)
			{
				return "BO";
			}

			return "O";
		}

		/// <summary>
		/// Gets the reward string for all the required items
		/// </summary>
		/// <returns>The reward string</returns>
		private string GetRewardStringForRequiredItems()
		{
			if (RequiredItems.Count == 0)
			{
				Globals.ConsoleWrite($"ERROR: No items defined for bundle {Name}");
				return "";
			}

			if (Room == CommunityCenterRooms.Vault)
			{
				return RequiredItems.First().GetStringForBundles(true);
			}

			string output = "";
			foreach (RequiredItem item in RequiredItems)
			{
				output += $"{item.GetStringForBundles(false)} ";
			}
			return output.Trim();
		}

		/// <summary>
		/// Used to create the bundle information
		/// </summary>
		/// <param name="bundleTypesToExclude">All bundle types that should not be considered</param>
		private void Initialize(List<BundleTypes> bundleTypesToExclude)
		{
			//NOTE: only the vault can have money requirements, and it also cannot require items
			switch (Room)
			{
				case CommunityCenterRooms.CraftsRoom:
					PopulateForCraftsRoom(bundleTypesToExclude);
					break;
				case CommunityCenterRooms.Pantry:
					break;
				case CommunityCenterRooms.FishTank:
					break;
				case CommunityCenterRooms.BoilerRoom:
					break;
				case CommunityCenterRooms.Vault:
					break;
				case CommunityCenterRooms.BulletinBoard:
					break;
			}
		}

		/// <summary>
		/// Creates a bundle for the crafts room
		/// </summary>
		/// <param name="bundleTypesToExclude">All bundle types that should not be considered</param>
		private void PopulateForCraftsRoom(List<BundleTypes> bundleTypesToExclude)
		{
			// Force one resource bundle so that there's one possible bundle to complete
			if (bundleTypesToExclude.Contains(BundleTypes.CraftingResource))
			{
				BundleType = Globals.RNGGetRandomValueFromList
					(_craftingRoomBundleTypes.Where(x => !bundleTypesToExclude.Contains(x)).ToList());
			}
			else
			{
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
			}

			Reward = new RequiredItem(ItemList.Items[(int)ObjectIndexes.AlgaeSoup], 1); // Temporary
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
		private void GenerateRewardForCraftingRoom()
		{
			//TODO: odds of this pulling from the any random reward pool (and make that pool!)
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
