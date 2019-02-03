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

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="room">The room the bundle is in</param>
		/// <param name="id">The id of the bundle</param>
		public Bundle(CommunityCenterRooms room, int id)
		{
			Room = room;
			Id = id;
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
	}
}
