using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class VaultBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Populates the bundle with the name, required items, minimum required, and color
		/// </summary>
		protected override void Populate()
		{
			int moneyAmount = 0;
			BundleType = Globals.RNGGetAndRemoveRandomValueFromList(RoomBundleTypes);
			string bundleNameFlavor = "";

			switch (BundleType)
			{
				case BundleTypes.Vault2500:
					bundleNameFlavor = Globals.RNGGetRandomValueFromList(new List<string>
					{
						"Lunch Money",
						"Bus Fare",
						"Cheapskate",
						"Tree Fiddy",
						"Cheapo",
						"Gas Station Food",
						"Piggy Bank"
					});
					moneyAmount = Range.GetRandomValue(500, 3500);
					break;
				case BundleTypes.Vault5000:
					bundleNameFlavor = Globals.RNGGetRandomValueFromList(new List<string>
					{
						"Chili's Lunch",
						"Express Mail",
						"New Blender",
						"New Shoes",
						"Amusement Park Pass",
						"Stardew Valley - The Game"
					});
					moneyAmount = Range.GetRandomValue(4000, 7000);
					break;
				case BundleTypes.Vault10000:
					bundleNameFlavor = Globals.RNGGetRandomValueFromList(new List<string>
					{
						"Nintendo Switch",
						"Laptop",
						"MLM Starter Kit",
						"4k TV",
						"Viking Fridge",
						"New Glasses"
					});
					moneyAmount = Range.GetRandomValue(7500, 12500);
					break;
				case BundleTypes.Vault25000:
					bundleNameFlavor = Globals.RNGGetRandomValueFromList(new List<string>
					{
						"Down Payment",
						"College Tuition",
						"Korean Air Ticket",
						"Fort Knox",
						"Mustang",
						"Lexus",
						"US Hospital Bill"
					});
					moneyAmount = Range.GetRandomValue(20000, 30000);
					break;
				default:
					return;
			}

			RequiredItems = new List<RequiredItem> { new RequiredItem() { MoneyAmount = moneyAmount } };
			Name = $"{bundleNameFlavor}: {moneyAmount:N0}G";
			Color = Globals.RNGGetRandomValueFromList(
				Enum.GetValues(typeof(BundleColors)).Cast<BundleColors>().ToList());
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			List<RequiredItem> potentialRewards = new List<RequiredItem>
			{
				new RequiredItem((int)ObjectIndexes.GoldBar, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.IridiumBar, Range.GetRandomValue(1, 5)),
				new RequiredItem((int)ObjectIndexes.SolidGoldLewis),
				new RequiredItem((int)ObjectIndexes.HMTGF),
				new RequiredItem((int)ObjectIndexes.PinkyLemon),
				new RequiredItem((int)ObjectIndexes.Foroguemon),
				new RequiredItem((int)ObjectIndexes.GoldenPumpkin),
				new RequiredItem((int)ObjectIndexes.GoldenMask),
				new RequiredItem((int)ObjectIndexes.GoldenRelic),
				new RequiredItem((int)ObjectIndexes.GoldBrazier),
				new RequiredItem((int)ObjectIndexes.TreasureChest),
				new RequiredItem((int)ObjectIndexes.Lobster, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.LobsterBisque, Range.GetRandomValue(5, 25))
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
