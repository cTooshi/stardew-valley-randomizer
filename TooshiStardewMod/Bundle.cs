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
			if (RequiredItems.Where(x => x.MoneyAmount >= 0).ToList().Count == 0 &&
				MinimumRequiredItems != null &&
				MinimumRequiredItems > 0)
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
			List<RequiredItem> moneyRequiredItems = RequiredItems.Where(x => x.MoneyAmount >= 0).ToList();
			if (moneyRequiredItems.Count > 0)
			{
				return moneyRequiredItems.First().GetStringForBundles();
			}

			string output = "";
			foreach (RequiredItem item in RequiredItems)
			{
				output += $"{item.GetStringForBundles()} ";
			}

			if (string.IsNullOrEmpty(output))
			{
				Globals.ConsoleWrite($"ERROR: No items defined for bundle {Name}");
			}
			return output.Trim();
		}

		private void Initialize(List<BundleTypes> bundleTypesToExclude)
		{
			//NOTE: only the vault can have money requirements
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

				// THe vault CANNOT require items!
				case CommunityCenterRooms.Vault:
					break;
				case CommunityCenterRooms.BulletinBoard:
					break;
			}
		}

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

			Name = "?"; // temp
			switch (BundleType)
			{
				case BundleTypes.CraftingResource:
					Name = "Resource";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Wood, 100, 250),
						new RequiredItem((int)ObjectIndexes.Stone, 100, 250),
						new RequiredItem((int)ObjectIndexes.Fiber, 10, 50),
						new RequiredItem((int)ObjectIndexes.Clay, 10, 50)
					};
					Color = BundleColors.Orange;
					Reward = new RequiredItem(ItemList.Items[(int)ObjectIndexes.AlgaeSoup], 1); // Temporary
					break;
				case BundleTypes.CraftingSprinklers:
					Name = "Crop Production";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Sprinkler, 1, 5),
						new RequiredItem((int)ObjectIndexes.QualitySprinkler, 1, 5),
						new RequiredItem((int)ObjectIndexes.IridiumSprinkler, 1, 5),
						new RequiredItem((int)ObjectIndexes.BasicFertilizer, 10, 20),
						new RequiredItem((int)ObjectIndexes.QualityFertilizer, 5, 20),
						new RequiredItem((int)ObjectIndexes.BasicRetainingSoil, 10, 20),
						new RequiredItem((int)ObjectIndexes.QualityRetainingSoil, 5, 20),
					};
					Color = BundleColors.Orange;
					Reward = new RequiredItem(ItemList.Items[(int)ObjectIndexes.AlgaeSoup], 1); // Temporary
					break;
				case BundleTypes.CraftingTreeProducts:
					break;
				case BundleTypes.CraftingTotems:
					break;
				case BundleTypes.CraftingBindle:
					break;
			}

			//TEMPORARY
			if (Name == "?")
			{
				Name = "Hello";
				RequiredItems = new List<RequiredItem>
						{
							new RequiredItem((int)ObjectIndexes.Wood, 100, 250),
							new RequiredItem((int)ObjectIndexes.Stone, 100, 250),
							new RequiredItem((int)ObjectIndexes.Fiber, 10, 50),
							new RequiredItem((int)ObjectIndexes.Clay, 10, 50)
						};
				Color = BundleColors.Orange;
				Reward = new RequiredItem(ItemList.Items[(int)ObjectIndexes.AlgaeSoup], 1); // Temporary
			}
		}

		//private List<RequiredItem> CreateRequiredItemsList(List<RequiredItem> items, int listLength)
		//{

		//}
	}
}
