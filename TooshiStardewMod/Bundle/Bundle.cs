using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Randomizer
{
	/// <summary>
	/// Represents a bundle
	/// </summary>
	public abstract class Bundle
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

		private static bool _isInitialized { get; set; }
		private static List<BundleTypes> _randomBundleTypes { get; set; }

		/// <summary>
		/// Re-set up the static properties so that if this is ran again
		/// </summary>
		public static void InitializeAllBundleTypes()
		{
			_randomBundleTypes = GetBundleTypeList("All");
			CraftingRoomBundle.RoomBundleTypes = GetBundleTypeList("Crafting");
			PantryBundle.RoomBundleTypes = GetBundleTypeList("Pantry");
			FishTankBundle.RoomBundleTypes = GetBundleTypeList("FishTank");
			BulletinBoardBundle.RoomBundleTypes = GetBundleTypeList("BulletinBoard");
			BoilerRoomBundle.RoomBundleTypes = GetBundleTypeList("Boiler");
			VaultBundle.RoomBundleTypes = GetBundleTypeList("Vault");
		}

		/// <summary>
		/// Gets the bundle type list that starts with the given string
		/// </summary>
		/// <param name="BundleTypeString">The string to match</param>
		/// <returns>The list of all matching bundle types</returns>
		private static List<BundleTypes> GetBundleTypeList(string BundleTypeString)
		{
			return Enum.GetValues(typeof(BundleTypes))
				.Cast<BundleTypes>()
				.Where(x => x.ToString().StartsWith(BundleTypeString))
				.ToList();
		}

		/// <summary>
		/// The factory call for this bundle - creates one of the appropriate type
		/// If there's no bundle types left, will default to a random bundle
		/// Has a 10% chance of generating a random bundle, and a 10% chance of a random reward
		/// </summary>
		/// <param name="room">The room the bundle is in</param>
		/// <param name="id">The id of the bundle</param>
		public static Bundle Create(CommunityCenterRooms room, int id)
		{
			Bundle createdBundle = null;
			switch (room)
			{
				case CommunityCenterRooms.CraftsRoom:
					createdBundle = new CraftingRoomBundle();
					break;
				case CommunityCenterRooms.Pantry:
					createdBundle = new PantryBundle();
					break;
				case CommunityCenterRooms.FishTank:
					createdBundle = new FishTankBundle();
					break;
				case CommunityCenterRooms.BoilerRoom:
					createdBundle = new BoilerRoomBundle();
					break;
				case CommunityCenterRooms.Vault:
					createdBundle = new VaultBundle();
					break;
				case CommunityCenterRooms.BulletinBoard:
					createdBundle = new BulletinBoardBundle();
					break;
				default:
					Globals.ConsoleWrite($"ERROR: Cannot create bundle for room: {room.ToString()}");
					return null;
			}

			createdBundle.Room = room;
			createdBundle.Id = id;

			if (!createdBundle.TryGenerateRandomBundle()) { createdBundle.Populate(); }
			if (!createdBundle.TryGenerateRandomReward()) { createdBundle.GenerateReward(); }

			if (createdBundle.RequiredItems == null || createdBundle.RequiredItems.Count == 0)
			{
				createdBundle.GenerateRandomBundleFailsafe();
			}

			return createdBundle;
		}

		/// <summary>
		/// Populate the bundle's name, required items, number required, color
		/// </summary>
		protected abstract void Populate();

		/// <summary>
		/// Populate the bundle's reward
		/// </summary>
		protected abstract void GenerateReward();

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
		/// Attempt to generate a random bundle - 10% chance
		/// </summary>
		/// <returns>True if successful, false otherwise</returns>
		protected bool TryGenerateRandomBundle()
		{
			if (Globals.RNGGetNextBoolean(10))
			{
				PopulateRandomBundle();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Attempt to generate a random reward - 10% chance
		/// </summary>
		/// /// <returns>True if successful, false otherwise</returns>
		protected bool TryGenerateRandomReward()
		{
			if (Globals.RNGGetNextBoolean(10))
			{
				GenerateRandomReward();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Force generate a random bundle
		/// Failsafe in case we run out of bundles
		/// </summary>
		protected void GenerateRandomBundleFailsafe()
		{
			Globals.ConsoleWrite("WARNING: Had to generate random bundle due to lack of bundles!");
			PopulateRandomBundle();
			GenerateRandomReward();
		}

		/// <summary>
		/// Creates a bundle with random items
		/// </summary>
		protected void PopulateRandomBundle()
		{
			BundleType = Globals.RNGGetRandomValueFromList(_randomBundleTypes);
			List<RequiredItem> potentialItems = new List<RequiredItem>();
			switch (BundleType)
			{
				case BundleTypes.AllRandom:
					Name = "Random";
					potentialItems = RequiredItem.CreateList(ItemList.Items.Values.Where(x =>
						x.DifficultyToObtain < ObtainingDifficulties.Impossible &&
						x.Id > -4)
					.ToList());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = 4;
					break;
				case BundleTypes.AllLetter:
					string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
					string randomLetter;
					do
					{
						randomLetter = letters[Range.GetRandomValue(0, letters.Length - 1)].ToString();
						letters.Replace(randomLetter, "");
						potentialItems = RequiredItem.CreateList(
							ItemList.Items.Values.Where(x =>
								x.Name.StartsWith(randomLetter, StringComparison.InvariantCultureIgnoreCase) &&
								x.Id > -4
							).ToList()
						);
					} while (potentialItems.Count < 4);
					Name = $"\"{randomLetter}\"";
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = 3;
					break;
			}

			Color = Globals.RNGGetRandomValueFromList(
				Enum.GetValues(typeof(BundleColors)).Cast<BundleColors>().ToList());
		}

		/// <summary>
		/// Generates a random reward out of all of the items
		/// </summary>
		protected void GenerateRandomReward()
		{
			Item reward = Globals.RNGGetRandomValueFromList(ItemList.Items.Values.Where(x =>
				x.Id != (int)ObjectIndexes.TransmuteAu && x.Id != (int)ObjectIndexes.TransmuteFe).ToList());
			int numberToGive = Range.GetRandomValue(1, 25);
			if (!reward.CanStack) { numberToGive = 1; }

			Reward = new RequiredItem(reward.Id, numberToGive);
		}
	}
}
