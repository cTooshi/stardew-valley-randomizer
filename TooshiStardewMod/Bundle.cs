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

		private static List<BundleTypes> _randomBundleTypes { get; set; }
		private static List<BundleTypes> _craftingRoomBundleTypes { get; set; }
		private static List<BundleTypes> _pantryBundleTypes { get; set; }

		/// <summary>
		/// Re-set up the static properties so that if this is ran again, everything works out
		/// </summary>
		public static void Reinitialize()
		{
			_randomBundleTypes =
				Enum.GetValues(typeof(BundleTypes))
					.Cast<BundleTypes>()
					.Where(x => x.ToString().StartsWith("All"))
					.ToList();

			_craftingRoomBundleTypes =
				Enum.GetValues(typeof(BundleTypes))
					.Cast<BundleTypes>()
					.Where(x => x.ToString().StartsWith("Crafting"))
					.ToList();

			_pantryBundleTypes =
				Enum.GetValues(typeof(BundleTypes))
					.Cast<BundleTypes>()
					.Where(x => x.ToString().StartsWith("Pantry"))
					.ToList();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="room">The room the bundle is in</param>
		/// <param name="id">The id of the bundle</param>
		public Bundle(CommunityCenterRooms room, int id)
		{
			Room = room;
			Id = id;
			Initialize();
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
		private void Initialize()
		{
			if (Room != CommunityCenterRooms.Vault && Range.GetRandomValue(1, 100) <= 10) //TODO: use the other API
			{
				PopulateRandomBundle();
				return;
			}

			//NOTE: only the vault can have money requirements, and it also cannot require items
			switch (Room)
			{
				case CommunityCenterRooms.CraftsRoom:
					PopulateForCraftsRoom();
					break;
				case CommunityCenterRooms.Pantry:
					PopulateForPantry();
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

			// Failsafe in case we run out of bundles
			if (RequiredItems == null)
			{
				Globals.ConsoleWrite("WARNING: Had to generate random bundle due to lack of bundles!");
				PopulateRandomBundle();
				return;
			}

			if (Range.GetRandomValue(1, 100) <= 10) //TODO: use the other API
			{
				Reward = GenerateRandomReward();
				return;
			}
		}

		/// <summary>
		/// Creates a bundle with random items
		/// </summary>
		private void PopulateRandomBundle()
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
			Reward = GenerateRandomReward();
		}

		/// <summary>
		/// Generates a random reward out of all of the items
		/// </summary>
		/// <returns />
		private RequiredItem GenerateRandomReward()
		{
			Item reward = Globals.RNGGetRandomValueFromList(ItemList.Items.Values.Where(x =>
				x.Id != (int)ObjectIndexes.TransmuteAu && x.Id != (int)ObjectIndexes.TransmuteFe).ToList());
			int numberToGive = Range.GetRandomValue(1, 25);
			if (reward.Id < -4 || reward.IsRing) { numberToGive = 1; }

			return new RequiredItem(reward.Id, numberToGive);
		}

		/// <summary>
		/// Creates a bundle for the crafts room
		/// </summary>
		private void PopulateForCraftsRoom()
		{
			// Force one resource bundle so that there's one possible bundle to complete
			if (!_craftingRoomBundleTypes.Contains(BundleTypes.CraftingResource))
			{
				BundleType = Globals.RNGGetAndRemoveRandomValueFromList(_craftingRoomBundleTypes);
			}
			else
			{
				_craftingRoomBundleTypes.Remove(BundleTypes.CraftingResource);
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
					//TODO: make the colored bundles here instead of in all
			}

			Reward = GenerateRewardForCraftingRoom();
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
		/// <returns />
		private RequiredItem GenerateRewardForCraftingRoom()
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

			return Globals.RNGGetRandomValueFromList(potentialRewards);
		}

		/// <summary>
		/// Creates a bundle for the pantry
		/// </summary>
		private void PopulateForPantry()
		{
			BundleType = Globals.RNGGetAndRemoveRandomValueFromList(_pantryBundleTypes);
			List<RequiredItem> potentialItems = new List<RequiredItem>();
			switch (BundleType)
			{
				case BundleTypes.PantryAnimal:
					Name = "Animal";
					potentialItems = RequiredItem.CreateList(ItemList.GetAnimalProducts());
					potentialItems.Add(new RequiredItem((int)ObjectIndexes.Hay, 25, 50));
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 2, RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				//case BundleTypes.PantryQualityCrops: //TODO
				//	break;
				case BundleTypes.PantryQualityForagables:
					Name = "Quality Foragables";
					potentialItems = RequiredItem.CreateList(ItemList.GetForagables());
					potentialItems.ForEach(x => x.MinimumQuality = ItemQualities.Gold);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Green;
					break;
				case BundleTypes.PantryCooked:
					Name = "Cooked";
					potentialItems = RequiredItem.CreateList(ItemList.GetCookeditems());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = Range.GetRandomValue(3, 4);
					Color = BundleColors.Green;
					break;
				//case BundleTypes.PantryFlower: //TODO ALL OF THESE
				//	break;
				//case BundleTypes.PantrySpringCrops:
				//	break;
				//case BundleTypes.PantrySummerCrops:
				//	break;
				//case BundleTypes.PantryFallCrops:
				//	break;
				//case BundleTypes.PantryWinterCrops:
				//	break;
				case BundleTypes.PantryEgg:
					Name = "Egg";
					potentialItems = RequiredItem.CreateList(
						ItemList.Items.Values.Where(x => x.Name.Contains("Egg")).ToList());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 10);
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 5, RequiredItems.Count - 3);
					Color = BundleColors.Yellow;
					break;
				//case BundleTypes.PantryRareFoods: //TODO - need those foods to exist!
				//	Name = "Rare Foods";
				//	RequiredItems = new List<RequiredItem>
				//	{
				//		new RequiredItem((int)ObjectIndexes.AncientFruit),
				//		new RequiredItem((int)ObjectIndexes.Starfruit),
				//		new RequiredItem((int)ObjectIndexes.SweetGemBerry)
				//	};
				//	Color = BundleColors.Blue;
				//	break;
				case BundleTypes.PantryDesert:
					Name = "Desert";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.IridiumOre, 5),
						Globals.RNGGetRandomValueFromList(new List<RequiredItem>
						{
							new RequiredItem((int)ObjectIndexes.GoldenMask),
							new RequiredItem((int)ObjectIndexes.GoldenRelic),
						}),
						new RequiredItem((int)ObjectIndexes.Sandfish), //TODO: change for the fish shuffle
						Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(ItemList.GetUniqueDesertForagables(), 1, 3))
						//new RequiredItem((int)ObjectIndexes.StarfruitSeeds, 5) //TODO: need this item to exist
					};
					MinimumRequiredItems = 4;
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.PantryDessert:
					Name = "Dessert";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.CranberryCandy),
						new RequiredItem((int)ObjectIndexes.PlumPudding),
						new RequiredItem((int)ObjectIndexes.PinkCake),
						new RequiredItem((int)ObjectIndexes.PumpkinPie),
						new RequiredItem((int)ObjectIndexes.RhubarbPie),
						new RequiredItem((int)ObjectIndexes.Cookie),
						new RequiredItem((int)ObjectIndexes.IceCream),
						new RequiredItem((int)ObjectIndexes.MinersTreat),
						new RequiredItem((int)ObjectIndexes.BlueberryTart),
						new RequiredItem((int)ObjectIndexes.BlackberryCobbler),
						new RequiredItem((int)ObjectIndexes.MapleBar),
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = 4;
					Color = BundleColors.Cyan;
					break;
				case BundleTypes.PantryMexicanFood:
					Name = "Mexican Food";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Tortilla),
						new RequiredItem((int)ObjectIndexes.Corn, 1, 5),
						new RequiredItem((int)ObjectIndexes.Tomato, 1, 5),
						new RequiredItem((int)ObjectIndexes.HotPepper, 1, 5),
						new RequiredItem((int)ObjectIndexes.FishTaco),
						new RequiredItem((int)ObjectIndexes.Rice),
						new RequiredItem((int)ObjectIndexes.Cheese),
					};
					MinimumRequiredItems = Range.GetRandomValue(4, 5);
					Color = BundleColors.Red;
					break;
			}

			Reward = GenerateRewardForPantry();
		}

		/// <summary>
		/// Generates the reward for completing a crafting room bundle
		/// </summary>
		/// <returns />
		private RequiredItem GenerateRewardForPantry()
		{
			//TODO: odds of this pulling from the any random reward pool (and make that pool!)
			var potentialRewards = new List<RequiredItem>
			{
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetResources()), 999),
				new RequiredItem((int)ObjectIndexes.Loom),
				new RequiredItem((int)ObjectIndexes.MayonnaiseMachine),
				new RequiredItem((int)ObjectIndexes.Heater),
				new RequiredItem((int)ObjectIndexes.AutoGrabber),
				new RequiredItem((int)ObjectIndexes.SeedMaker),
				//new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetCrops()), 25, 50), //TODO: uncomment when crops are done
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetCookeditems())),
				//new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetSeeds()), 50 100), // TODO: uncomment when crops are done
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetAnimalProducts()), 25, 50),
			};

			return Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
