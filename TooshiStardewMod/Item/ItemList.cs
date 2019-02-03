﻿using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class ItemList
	{
		/// <summary>
		/// Returns the crafting string of the given object index
		/// Intended to only be passed craftable items, or you'll get an error in the console
		/// </summary>
		/// <param name="objectIndex">The object to look up</param>
		/// <returns />
		public static string GetCraftingString(ObjectIndexes objectIndex)
		{
			Item item = Items[(int)objectIndex];
			if (item.IsCraftable)
			{
				return ((CraftableItem)item).GetCraftingString();
			}

			Globals.ConsoleWrite($"ERROR: Attempted to create a crafting recipe for a non-craftable item - {item.Name}");
			return string.Empty;
		}

		/// <summary>
		/// Gets a random craftable item out of the list
		/// </summary>
		/// <param name="possibleDifficulties">The difficulties that can be in the result</param>
		/// <param name="itemBeingCrafted">The item being crafted</param>
		/// <param name="idsToExclude">Any ids to not include in the results</param>
		/// <param name="onlyResources">Whether to only include resource items</param>
		/// <returns>The selected item</returns>
		public static Item GetRandomCraftableItem(
			List<ObtainingDifficulties> possibleDifficulties,
			Item itemBeingCrafted,
			List<int> idsToExclude = null,
			bool onlyResources = false)
		{
			List<Item> items = Items.Values
				.Where(x =>
					// Disallow the item being required to craft itself
					x.Id != itemBeingCrafted.Id &&

					// Don't allow items to require the items that they're used to obtain
					(itemBeingCrafted.Id != (int)ObjectIndexes.Furnace || !x.IsSmelted) &&
					(itemBeingCrafted.Id != (int)ObjectIndexes.MayonnaiseMachine || !x.IsMayonaisse) &&
					(itemBeingCrafted.Id != (int)ObjectIndexes.CrabPot || !x.IsCrabPotItem) &&
					((itemBeingCrafted.Id != (int)ObjectIndexes.BeeHouse) || (x.Id != (int)ObjectIndexes.Honey)) &&
					((itemBeingCrafted.Id != (int)ObjectIndexes.LightningRod) || (x.Id != (int)ObjectIndexes.Battery)) &&

					(possibleDifficulties == null || possibleDifficulties.Contains(x.DifficultyToObtain)) &&
					(idsToExclude == null || !idsToExclude.Contains(x.Id)) &&
					(!onlyResources || x.IsResource)
				).ToList();

			return Globals.RNGGetRandomValueFromList(items);
		}

		public static Dictionary<int, Item> Items = new Dictionary<int, Item>
		{
			// Craftable items - Impossible by default
			{ (int)ObjectIndexes.WoodFence, new CraftableItem((int)ObjectIndexes.WoodFence, "/Field/322/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.StoneFence, new CraftableItem((int)ObjectIndexes.StoneFence, "/Field/323/false/", CraftableCategories.EasyAndNeedMany, "Farming", 2) },
			{ (int)ObjectIndexes.IronFence, new CraftableItem((int)ObjectIndexes.IronFence, "/Field/324 10/false/", CraftableCategories.ModerateAndNeedMany, "Farming", 4) },
			{ (int)ObjectIndexes.HardwoodFence, new CraftableItem((int)ObjectIndexes.HardwoodFence, "/Field/298/false/", CraftableCategories.ModerateAndNeedMany, "Farming", 6) },
			{ (int)ObjectIndexes.Gate, new CraftableItem((int)ObjectIndexes.Gate, "/Home/325/false/l 0", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.Chest, new CraftableItem((int)ObjectIndexes.Chest, "/Home/130/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.Torch, new CraftableItem((int)ObjectIndexes.Torch, "/Field/93/false/l 0", CraftableCategories.Easy) { DifficultyToObtain = ObtainingDifficulties.SmallTimeRequirements } }, // You can find it in the mines
			{ (int)ObjectIndexes.Scarecrow, new CraftableItem((int)ObjectIndexes.Scarecrow, "/Home/8/true/", CraftableCategories.Moderate, "Farming", 1) },
			{ (int)ObjectIndexes.BeeHouse, new CraftableItem((int)ObjectIndexes.BeeHouse, "/Home/10/true/", CraftableCategories.Moderate, "Farming", 3) },
			{ (int)ObjectIndexes.Keg, new CraftableItem((int)ObjectIndexes.Keg, "/Home/12/true/", CraftableCategories.Moderate, "Farming", 8) },
			{ (int)ObjectIndexes.Cask, new CraftableItem((int)ObjectIndexes.Cask, "/Home/163/true/null", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.Furnace, new CraftableItem((int)ObjectIndexes.Furnace, "/Home/13/true/l 2", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.GardenPot, new CraftableItem((int)ObjectIndexes.GardenPot, "/Home/62/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.WoodSign, new CraftableItem((int)ObjectIndexes.WoodSign, "/Home/37/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.StoneSign, new CraftableItem((int)ObjectIndexes.StoneSign, "/Home/38/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.CheesePress, new CraftableItem((int)ObjectIndexes.CheesePress, "/Home/16/true/", CraftableCategories.Moderate, "Farming", 6) },
			{ (int)ObjectIndexes.MayonnaiseMachine, new CraftableItem((int)ObjectIndexes.MayonnaiseMachine, "/Home/24/true/", CraftableCategories.Moderate, "Farming", 2) },
			{ (int)ObjectIndexes.SeedMaker, new CraftableItem((int)ObjectIndexes.SeedMaker, "/Home/25/true/", CraftableCategories.Moderate, "Farming", 9) },
			{ (int)ObjectIndexes.Loom, new CraftableItem((int)ObjectIndexes.Loom, "/Home/17/true/", CraftableCategories.Moderate, "Farming", 7) },
			{ (int)ObjectIndexes.OilMaker, new CraftableItem((int)ObjectIndexes.OilMaker, "/Home/19/true/", CraftableCategories.Moderate, "Farming", 8) },
			{ (int)ObjectIndexes.RecyclingMachine, new CraftableItem((int)ObjectIndexes.RecyclingMachine, "/Home/20/true/", CraftableCategories.Moderate, "Fishing", 4) },
			{ (int)ObjectIndexes.WormBin, new CraftableItem((int)ObjectIndexes.WormBin, "/Home/154/true/", CraftableCategories.Difficult, "Fishing", 8) },
			{ (int)ObjectIndexes.PreservesJar, new CraftableItem((int)ObjectIndexes.PreservesJar, "/Home/15/true/", CraftableCategories.Moderate, "Farming", 4) },
			{ (int)ObjectIndexes.CharcoalKiln, new CraftableItem((int)ObjectIndexes.CharcoalKiln, "/Home/114/true/", CraftableCategories.Easy, "Foraging", 4) },
			{ (int)ObjectIndexes.Tapper, new CraftableItem((int)ObjectIndexes.Tapper, "/Home/105/true/", CraftableCategories.Moderate, "Foraging", 3) },
			{ (int)ObjectIndexes.LightningRod, new CraftableItem((int)ObjectIndexes.LightningRod, "/Home/9/true/", CraftableCategories.Moderate, "Foraging", 6) },
			{ (int)ObjectIndexes.SlimeIncubator, new CraftableItem((int)ObjectIndexes.SlimeIncubator, "/Home/156/true/", CraftableCategories.Difficult, "Combat", 8) },
			{ (int)ObjectIndexes.SlimeEggPress, new CraftableItem((int)ObjectIndexes.SlimeEggPress, "/Home/158/true/", CraftableCategories.DifficultAndNeedMany, "Combat", 6) { OverrideName = "Slime Egg-Press" } },
			{ (int)ObjectIndexes.Crystalarium, new CraftableItem((int)ObjectIndexes.Crystalarium, "/Home/21/true/", CraftableCategories.Moderate, "Mining", 9) },
			{ (int)ObjectIndexes.Sprinkler, new CraftableItem((int)ObjectIndexes.Sprinkler, "/Home/599/false/", CraftableCategories.ModerateAndNeedMany, "Farming", 2) },
			{ (int)ObjectIndexes.QualitySprinkler, new CraftableItem((int)ObjectIndexes.QualitySprinkler, "/Home/621/false/", CraftableCategories.Moderate, "Farming", 6) },
			{ (int)ObjectIndexes.IridiumSprinkler, new CraftableItem((int)ObjectIndexes.IridiumSprinkler, "/Home/645/false/", CraftableCategories.DifficultAndNeedMany, "Farming", 9) },
			{ (int)ObjectIndexes.Staircase, new CraftableItem((int)ObjectIndexes.Staircase, "/Field/71/true/", CraftableCategories.Moderate, "Mining", 2) },
			{ (int)ObjectIndexes.BasicFertilizer, new CraftableItem((int)ObjectIndexes.BasicFertilizer, "/Field/368/false/s ", CraftableCategories.EasyAndNeedMany, "Farming", 1) },
			{ (int)ObjectIndexes.QualityFertilizer, new CraftableItem((int)ObjectIndexes.QualityFertilizer, "/Field/369/false/s ", CraftableCategories.Easy, "Farming", 9) },
			{ (int)ObjectIndexes.BasicRetainingSoil, new CraftableItem((int)ObjectIndexes.BasicRetainingSoil, "/Field/370/false/s ", CraftableCategories.EasyAndNeedMany, "Farming", 4) },
			{ (int)ObjectIndexes.QualityRetainingSoil, new CraftableItem((int)ObjectIndexes.QualityRetainingSoil, "/Field/371 2/false/s ", CraftableCategories.EasyAndNeedMany, "Farming", 7) },
			{ (int)ObjectIndexes.SpeedGro, new CraftableItem((int)ObjectIndexes.SpeedGro, "/Field/465 5/false/s ", CraftableCategories.ModerateAndNeedMany, "Farming", 3) { OverrideName = "Speed-Gro" } },
			{ (int)ObjectIndexes.DeluxeSpeedGro, new CraftableItem((int)ObjectIndexes.DeluxeSpeedGro, "/Field/466 5/false/s ", CraftableCategories.ModerateAndNeedMany, "Farming", 8) { OverrideName = "Deluxe Speed-Gro" } },
			{ (int)ObjectIndexes.CherryBomb, new CraftableItem((int)ObjectIndexes.CherryBomb, "/Field/286/false/", CraftableCategories.Easy, "Mining", 1) },
			{ (int)ObjectIndexes.Bomb, new CraftableItem((int)ObjectIndexes.Bomb, "/Field/287/false/", CraftableCategories.Moderate, "Mining", 6) },
			{ (int)ObjectIndexes.MegaBomb, new CraftableItem((int)ObjectIndexes.MegaBomb, "/Field/288/false/", CraftableCategories.Difficult, "Mining",  8) },
			{ (int)ObjectIndexes.ExplosiveAmmo, new CraftableItem((int)ObjectIndexes.ExplosiveAmmo, "/Home/441 5/false/", CraftableCategories.ModerateAndNeedMany, "Combat",  8) },
			{ (int)ObjectIndexes.TransmuteFe, new CraftableItem((int)ObjectIndexes.TransmuteFe, "/Home/335/false/", CraftableCategories.Moderate, "Mining", 4) { OverrideName = "Transmute (Fe)" } },
			{ (int)ObjectIndexes.TransmuteAu, new CraftableItem((int)ObjectIndexes.TransmuteAu, "/Home/336/false/", CraftableCategories.Moderate, "Mining", 7) { OverrideName = "Transmute (Au)" } },
			// Skipping ancient seeds, as it's just meant to get them from the artifact
			{ (int)ObjectIndexes.WildSeedsSp, new CraftableItem((int)ObjectIndexes.WildSeedsSp, "/Field/495 10/false/", CraftableCategories.DifficultAndNeedMany, "Foraging", 1) { OverrideName = "Wild Seeds (Sp)" } },
			{ (int)ObjectIndexes.WildSeedsSu, new CraftableItem((int)ObjectIndexes.WildSeedsSu, "/Field/496 10/false/", CraftableCategories.DifficultAndNeedMany, "Foraging", 4) { OverrideName = "Wild Seeds (Su)" } },
			{ (int)ObjectIndexes.WildSeedsFa, new CraftableItem((int)ObjectIndexes.WildSeedsFa, "/Field/497 10/false/", CraftableCategories.DifficultAndNeedMany, "Foraging", 6) { OverrideName = "Wild Seeds (Fa)" } },
			{ (int)ObjectIndexes.WildSeedsWi, new CraftableItem((int)ObjectIndexes.WildSeedsWi, "/Field/498 10/false/", CraftableCategories.DifficultAndNeedMany, "Foraging", 7) { OverrideName = "Wild Seeds (Wi)" } },
			{ (int)ObjectIndexes.WarpTotemFarm, new CraftableItem((int)ObjectIndexes.WarpTotemFarm, "/Field/688/false/", CraftableCategories.ModerateAndNeedMany, "Foraging", 8) { OverrideName = "Warp Totem: Farm" } },
			{ (int)ObjectIndexes.WarpTotemMountains, new CraftableItem((int)ObjectIndexes.WarpTotemMountains, "/Field/689/false/", CraftableCategories.ModerateAndNeedMany, "Foraging", 7) { OverrideName = "Warp Totem: Mountains" } },
			{ (int)ObjectIndexes.WarpTotemBeach, new CraftableItem((int)ObjectIndexes.WarpTotemBeach, "/Field/690/false/", CraftableCategories.ModerateAndNeedMany, "Foraging", 6) { OverrideName = "Warp Totem: Beach" } },
			{ (int)ObjectIndexes.RainTotem, new CraftableItem((int)ObjectIndexes.RainTotem, "/Field/681/false/", CraftableCategories.Difficult, "Foraging", 9) },
			{ (int)ObjectIndexes.FieldSnack, new CraftableItem((int)ObjectIndexes.FieldSnack, "/Home/403/false/", CraftableCategories.Easy, "Foraging", 1) },
			{ (int)ObjectIndexes.JackOLantern, new CraftableItem((int)ObjectIndexes.JackOLantern, "/Home/746/false/null", CraftableCategories.DifficultAndNeedMany) { OverrideName = "Jack-O-Lantern" } },
			{ (int)ObjectIndexes.WoodFloor, new CraftableItem((int)ObjectIndexes.WoodFloor, "/Field/328/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.StrawFloor, new CraftableItem((int)ObjectIndexes.StrawFloor, "/Field/401/false/1 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.WeatheredFloor, new CraftableItem((int)ObjectIndexes.WeatheredFloor, "/Field/331/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.CrystalFloor, new CraftableItem((int)ObjectIndexes.CrystalFloor, "/Field/333 5/false/l 0", CraftableCategories.ModerateAndNeedMany) },
			{ (int)ObjectIndexes.StoneFloor, new CraftableItem((int)ObjectIndexes.StoneFloor, "/Field/329/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.WoodPath, new CraftableItem((int)ObjectIndexes.WoodPath, "/Field/405/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.GravelPath, new CraftableItem((int)ObjectIndexes.GravelPath, "/Field/407/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.CobblestonePath, new CraftableItem((int)ObjectIndexes.CobblestonePath, "/Field/411/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.SteppingStonePath, new CraftableItem((int)ObjectIndexes.SteppingStonePath, "/Field/415/false/l 0", CraftableCategories.EasyAndNeedMany) },
			{ (int)ObjectIndexes.CrystalPath, new CraftableItem((int)ObjectIndexes.CrystalPath, "/Field/409 5/false/l 0", CraftableCategories.ModerateAndNeedMany) },
			{ (int)ObjectIndexes.WildBait, new CraftableItem((int)ObjectIndexes.WildBait, "/Home/774 5/false/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.Bait, new CraftableItem((int)ObjectIndexes.Bait, "/Home/685 5/false/", CraftableCategories.EasyAndNeedMany, "Fishing", 2) },
			{ (int)ObjectIndexes.Spinner, new CraftableItem((int)ObjectIndexes.Spinner, "/Home/686/false/", CraftableCategories.ModerateAndNeedMany, "Fishing", 6) },
			{ (int)ObjectIndexes.Magnet, new CraftableItem((int)ObjectIndexes.Magnet, "/Home/703 3/false/", CraftableCategories.ModerateAndNeedMany, "Fishing", 9) },
			{ (int)ObjectIndexes.TrapBobber, new CraftableItem((int)ObjectIndexes.TrapBobber, "/Home/694/false/", CraftableCategories.Moderate, "Fishing", 6) },
			{ (int)ObjectIndexes.CorkBobber, new CraftableItem((int)ObjectIndexes.CorkBobber, "/Home/695/false/", CraftableCategories.Moderate, "Fishing", 7) },
			{ (int)ObjectIndexes.DressedSpinner, new CraftableItem((int)ObjectIndexes.DressedSpinner, "/Home/687/false/", CraftableCategories.Moderate, "Fishing", 8) },
			{ (int)ObjectIndexes.TreasureHunter, new CraftableItem((int)ObjectIndexes.TreasureHunter, "/Home/693/false/", CraftableCategories.Moderate, "Fishing", 7) },
			{ (int)ObjectIndexes.BarbedHook, new CraftableItem((int)ObjectIndexes.BarbedHook, "/Home/691/false/", CraftableCategories.Moderate, "Fishing", 8) },
			{ (int)ObjectIndexes.OilOfGarlic, new CraftableItem((int)ObjectIndexes.OilOfGarlic, "/Home/772 1/false/", CraftableCategories.Difficult, "Fishing", 6) },
			{ (int)ObjectIndexes.LifeElixir, new CraftableItem((int)ObjectIndexes.LifeElixir, "/Home/773 1/false/", CraftableCategories.DifficultAndNeedMany, "Fishing", 2) },
			{ (int)ObjectIndexes.CrabPot, new CraftableItem((int)ObjectIndexes.CrabPot, "/Home/710/false/", CraftableCategories.Moderate, "Fishing", 9) },
			{ (int)ObjectIndexes.IridiumBand, new CraftableItem((int)ObjectIndexes.IridiumBand, "/Home/527/false/", CraftableCategories.Endgame, "Combat", 9) },
			{ (int)ObjectIndexes.WeddingRing, new CraftableItem((int)ObjectIndexes.WeddingRing, "/Home/801/false/null", CraftableCategories.Endgame) },
			{ (int)ObjectIndexes.RingOfYoba, new CraftableItem((int)ObjectIndexes.RingOfYoba, "/Home/524/false/", CraftableCategories.Difficult, "Combat", 7) { OverrideName = "Ring of Yoba" } },
			{ (int)ObjectIndexes.SturdyRing, new CraftableItem((int)ObjectIndexes.SturdyRing, "/Home/525/false/", CraftableCategories.Moderate, "Combat", 1) },
			{ (int)ObjectIndexes.WarriorRing, new CraftableItem((int)ObjectIndexes.WarriorRing, "/Home/521/false/", CraftableCategories.Moderate, "Combat", 4) },
			{ (int)ObjectIndexes.TubOFlowers, new CraftableItem((int)ObjectIndexes.TubOFlowers, "/Home/108/true/null", CraftableCategories.Easy) { OverrideName = "Tub o' Flowers" } },
			{ (int)ObjectIndexes.WoodenBrazier, new CraftableItem((int)ObjectIndexes.WoodenBrazier, "/Home/143/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.WickedStatue, new CraftableItem((int)ObjectIndexes.WickedStatue, "/Home/83/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.StoneBrazier, new CraftableItem((int)ObjectIndexes.StoneBrazier, "/Home/144/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.GoldBrazier, new CraftableItem((int)ObjectIndexes.GoldBrazier, "/Home/145/true/null", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.Campfire, new CraftableItem((int)ObjectIndexes.Campfire, "/Home/146/true/null", CraftableCategories.Easy) },
			{ (int)ObjectIndexes.StumpBrazier, new CraftableItem((int)ObjectIndexes.StumpBrazier, "/Home/147/true/null", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.CarvedBrazier, new CraftableItem((int)ObjectIndexes.CarvedBrazier, "/Home/148/true/null", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.SkullBrazier, new CraftableItem((int)ObjectIndexes.SkullBrazier, "/Home/149/true/null", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.BarrelBrazier, new CraftableItem((int)ObjectIndexes.BarrelBrazier, "/Home/150/true/null", CraftableCategories.Moderate) },
			{ (int)ObjectIndexes.MarbleBrazier, new CraftableItem((int)ObjectIndexes.MarbleBrazier, "/Home/151/true/null", CraftableCategories.Difficult) },
			{ (int)ObjectIndexes.WoodLampPost, new CraftableItem((int)ObjectIndexes.WoodLampPost, "/Home/152/true/null", CraftableCategories.Moderate) { OverrideName = "Wood Lamp-post" } },
			{ (int)ObjectIndexes.IronLampPost, new CraftableItem((int)ObjectIndexes.IronLampPost, "/Home/153/true/null", CraftableCategories.Moderate) { OverrideName = "Iron Lamp-post" } },

			// Resources - ObtainingDifficulties.NoRequirements
			{ (int)ObjectIndexes.Wood, new ResourceItem((int)ObjectIndexes.Wood) },
			{ (int)ObjectIndexes.Hardwood, new ResourceItem((int)ObjectIndexes.Wood, 1, new Range(1, 15)) { DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements } },
			{ (int)ObjectIndexes.Stone, new ResourceItem((int)ObjectIndexes.Stone) },
			{ (int)ObjectIndexes.Fiber, new ResourceItem((int)ObjectIndexes.Fiber, 3, new Range(1, 5)) },
			{ (int)ObjectIndexes.Clay, new ResourceItem((int)ObjectIndexes.Clay, 1, new Range(1, 15)) },

			// Items you get as a byproduct of collection resources
			{ (int)ObjectIndexes.Sap, new Item((int)ObjectIndexes.Sap, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 15) } },
			{ (int)ObjectIndexes.Acorn, new Item((int)ObjectIndexes.Acorn, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.MapleSeed, new Item((int)ObjectIndexes.MapleSeed, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.PineCone, new Item((int)ObjectIndexes.PineCone, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.MixedSeeds, new Item((int)ObjectIndexes.MixedSeeds, ObtainingDifficulties.NoRequirements) },

			// Tapper items
			{ (int)ObjectIndexes.OakResin, new Item((int)ObjectIndexes.OakResin, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.MapleSyrup, new Item((int)ObjectIndexes.MapleSyrup, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.PineTar, new Item((int)ObjectIndexes.PineTar, ObtainingDifficulties.MediumTimeRequirements) },

			// Items you can buy from the shops easily
			{ (int)ObjectIndexes.Hay, new Item((int)ObjectIndexes.Hay, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.Sugar, new Item((int)ObjectIndexes.Sugar, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.Oil, new Item((int)ObjectIndexes.Oil, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.WheatFlour, new Item((int)ObjectIndexes.WheatFlour, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },

			// Items you can find while fishing
			{ (int)ObjectIndexes.Seaweed, new Item((int)ObjectIndexes.Seaweed, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.GreenAlgae, new Item((int)ObjectIndexes.GreenAlgae, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.WhiteAlgae, new Item((int)ObjectIndexes.WhiteAlgae, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },

			// Fish - defaults to ObtainingDifficulties.LargeTimeRequirements
			{ (int)ObjectIndexes.AnyFish, new FishItem((int)ObjectIndexes.AnyFish, ObtainingDifficulties.NoRequirements) },

			{ (int)ObjectIndexes.Pufferfish, new FishItem((int)ObjectIndexes.Pufferfish) },
			{ (int)ObjectIndexes.Anchovy, new FishItem((int)ObjectIndexes.Anchovy) },
			{ (int)ObjectIndexes.Tuna, new FishItem((int)ObjectIndexes.Tuna) },
			{ (int)ObjectIndexes.Sardine, new FishItem((int)ObjectIndexes.Sardine) },
			{ (int)ObjectIndexes.Bream, new FishItem((int)ObjectIndexes.Bream) },
			{ (int)ObjectIndexes.LargemouthBass, new FishItem((int)ObjectIndexes.LargemouthBass) },
			{ (int)ObjectIndexes.SmallmouthBass, new FishItem((int)ObjectIndexes.SmallmouthBass) },
			{ (int)ObjectIndexes.RainbowTrout, new FishItem((int)ObjectIndexes.RainbowTrout) },
			{ (int)ObjectIndexes.Salmon, new FishItem((int)ObjectIndexes.Salmon) },
			{ (int)ObjectIndexes.Walleye, new FishItem((int)ObjectIndexes.Walleye) },
			{ (int)ObjectIndexes.Perch, new FishItem((int)ObjectIndexes.Perch) },
			{ (int)ObjectIndexes.Carp, new FishItem((int)ObjectIndexes.Carp) },
			{ (int)ObjectIndexes.Catfish, new FishItem((int)ObjectIndexes.Catfish) },
			{ (int)ObjectIndexes.Pike, new FishItem((int)ObjectIndexes.Pike) },
			{ (int)ObjectIndexes.Sunfish, new FishItem((int)ObjectIndexes.Sunfish) },
			{ (int)ObjectIndexes.RedMullet, new FishItem((int)ObjectIndexes.RedMullet) },
			{ (int)ObjectIndexes.Herring, new FishItem((int)ObjectIndexes.Herring) },
			{ (int)ObjectIndexes.Eel, new FishItem((int)ObjectIndexes.Eel) },
			{ (int)ObjectIndexes.Octopus, new FishItem((int)ObjectIndexes.Octopus) },
			{ (int)ObjectIndexes.RedSnapper, new FishItem((int)ObjectIndexes.RedSnapper) },
			{ (int)ObjectIndexes.Squid, new FishItem((int)ObjectIndexes.Squid) },
			{ (int)ObjectIndexes.SeaCucumber, new FishItem((int)ObjectIndexes.SeaCucumber) },
			{ (int)ObjectIndexes.SuperCucumber, new FishItem((int)ObjectIndexes.SuperCucumber) },
			{ (int)ObjectIndexes.Ghostfish, new FishItem((int)ObjectIndexes.Ghostfish) },
			{ (int)ObjectIndexes.Stonefish, new FishItem((int)ObjectIndexes.Stonefish) },
			{ (int)ObjectIndexes.IcePip, new FishItem((int)ObjectIndexes.IcePip) },
			{ (int)ObjectIndexes.LavaEel, new FishItem((int)ObjectIndexes.LavaEel) },
			{ (int)ObjectIndexes.Sandfish, new FishItem((int)ObjectIndexes.Sandfish) },
			{ (int)ObjectIndexes.ScorpionCarp, new FishItem((int)ObjectIndexes.ScorpionCarp) },
			{ (int)ObjectIndexes.Sturgeon, new FishItem((int)ObjectIndexes.Sturgeon) },
			{ (int)ObjectIndexes.TigerTrout, new FishItem((int)ObjectIndexes.TigerTrout) },
			{ (int)ObjectIndexes.Bullhead, new FishItem((int)ObjectIndexes.Bullhead) },
			{ (int)ObjectIndexes.Tilapia, new FishItem((int)ObjectIndexes.Tilapia) },
			{ (int)ObjectIndexes.Chub, new FishItem((int)ObjectIndexes.Chub) },
			{ (int)ObjectIndexes.Dorado, new FishItem((int)ObjectIndexes.Dorado) },
			{ (int)ObjectIndexes.Albacore, new FishItem((int)ObjectIndexes.Albacore) },
			{ (int)ObjectIndexes.Shad, new FishItem((int)ObjectIndexes.Shad) },
			{ (int)ObjectIndexes.Lingcod, new FishItem((int)ObjectIndexes.Lingcod) },
			{ (int)ObjectIndexes.Halibut, new FishItem((int)ObjectIndexes.Halibut) },
			{ (int)ObjectIndexes.Woodskip, new FishItem((int)ObjectIndexes.Woodskip) },
			{ (int)ObjectIndexes.VoidSalmon, new FishItem((int)ObjectIndexes.VoidSalmon, ObtainingDifficulties.Impossible) }, // Can only get after completing bundles
			{ (int)ObjectIndexes.Slimejack, new FishItem((int)ObjectIndexes.Slimejack, ObtainingDifficulties.Impossible) }, // Can only get after completing bundles

			{ (int)ObjectIndexes.MidnightSquid, new FishItem((int)ObjectIndexes.MidnightSquid, ObtainingDifficulties.RareItem) }, // These three are only at the night market
			{ (int)ObjectIndexes.SpookFish, new FishItem((int)ObjectIndexes.SpookFish, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.Blobfish, new FishItem((int)ObjectIndexes.Blobfish, ObtainingDifficulties.RareItem) },

			{ (int)ObjectIndexes.Crimsonfish, new FishItem((int)ObjectIndexes.Crimsonfish, ObtainingDifficulties.EndgameItem) },
			{ (int)ObjectIndexes.Angler, new FishItem((int)ObjectIndexes.Angler, ObtainingDifficulties.EndgameItem) },
			{ (int)ObjectIndexes.Legend, new FishItem((int)ObjectIndexes.Legend, ObtainingDifficulties.EndgameItem) },
			{ (int)ObjectIndexes.Glacierfish, new FishItem((int)ObjectIndexes.Glacierfish, ObtainingDifficulties.EndgameItem) },
			{ (int)ObjectIndexes.MutantCarp, new FishItem((int)ObjectIndexes.Blobfish, ObtainingDifficulties.EndgameItem) },

			// Crab pot specific
			{ (int)ObjectIndexes.Lobster, new CrabPotItem((int)ObjectIndexes.Lobster) },
			{ (int)ObjectIndexes.Crab, new CrabPotItem((int)ObjectIndexes.Crab) },
			{ (int)ObjectIndexes.Shrimp, new CrabPotItem((int)ObjectIndexes.Shrimp) },

			// Items you can find in the mines
			{ (int)ObjectIndexes.CaveCarrot, new Item((int)ObjectIndexes.CaveCarrot, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.BugMeat, new Item((int)ObjectIndexes.BugMeat, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.Slime, new Item((int)ObjectIndexes.BugMeat, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 10) } },
			{ (int)ObjectIndexes.BatWing, new Item((int)ObjectIndexes.BatWing, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.VoidEssence, new Item((int)ObjectIndexes.VoidEssence, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.SolarEssence, new Item((int)ObjectIndexes.SolarEssence, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },

			{ (int)ObjectIndexes.Coal, new Item((int)ObjectIndexes.Coal, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.CopperOre, new Item((int)ObjectIndexes.CopperOre, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.IronOre, new Item((int)ObjectIndexes.IronOre, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.IridiumOre, new Item((int)ObjectIndexes.IridiumOre, ObtainingDifficulties.EndgameItem) { ItemsRequiredForRecipe = new Range(1, 5) } },

			{ (int)ObjectIndexes.Quartz, new Item((int)ObjectIndexes.Quartz, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.FireQuartz, new Item((int)ObjectIndexes.FireQuartz, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.EarthCrystal, new Item((int)ObjectIndexes.EarthCrystal, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.FrozenTear, new Item((int)ObjectIndexes.FrozenTear, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },

			{ (int)ObjectIndexes.Geode, new Item((int)ObjectIndexes.Geode, ObtainingDifficulties.SmallTimeRequirements) },
			{ (int)ObjectIndexes.FrozenGeode, new Item((int)ObjectIndexes.FrozenGeode, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.MagmaGeode, new Item((int)ObjectIndexes.MagmaGeode, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 2) } },
			{ (int)ObjectIndexes.OmniGeode, new Item((int)ObjectIndexes.OmniGeode, ObtainingDifficulties.MediumTimeRequirements) },

			{ (int)ObjectIndexes.Aquamarine, new Item((int)ObjectIndexes.Aquamarine, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Amethyst, new Item((int)ObjectIndexes.Amethyst, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Emerald, new Item((int)ObjectIndexes.Emerald, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Ruby, new Item((int)ObjectIndexes.Ruby, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Topaz, new Item((int)ObjectIndexes.Topaz, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Jade, new Item((int)ObjectIndexes.Jade, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Diamond, new Item((int)ObjectIndexes.Diamond, ObtainingDifficulties.MediumTimeRequirements) },

			// Geode mineral items - ObtainingDifficulties.LargeTimeRequirements
			{ (int)ObjectIndexes.Alamite, new Item((int)ObjectIndexes.Alamite) },
			{ (int)ObjectIndexes.Calcite, new Item((int)ObjectIndexes.Calcite) },
			{ (int)ObjectIndexes.Celestine, new Item((int)ObjectIndexes.Celestine) },
			{ (int)ObjectIndexes.Granite, new Item((int)ObjectIndexes.Granite) },
			{ (int)ObjectIndexes.Jagoite, new Item((int)ObjectIndexes.Jagoite) },
			{ (int)ObjectIndexes.Jamborite, new Item((int)ObjectIndexes.Jamborite) },
			{ (int)ObjectIndexes.Limestone, new Item((int)ObjectIndexes.Limestone) },
			{ (int)ObjectIndexes.Malachite, new Item((int)ObjectIndexes.Malachite) },
			{ (int)ObjectIndexes.Mudstone, new Item((int)ObjectIndexes.Mudstone) },
			{ (int)ObjectIndexes.Nekoite, new Item((int)ObjectIndexes.Nekoite) },
			{ (int)ObjectIndexes.Orpiment, new Item((int)ObjectIndexes.Orpiment) },
			{ (int)ObjectIndexes.PetrifiedSlime, new Item((int)ObjectIndexes.PetrifiedSlime) },
			{ (int)ObjectIndexes.Sandstone, new Item((int)ObjectIndexes.Sandstone) },
			{ (int)ObjectIndexes.Slate, new Item((int)ObjectIndexes.Slate) },
			{ (int)ObjectIndexes.ThunderEgg, new Item((int)ObjectIndexes.ThunderEgg) },

			{ (int)ObjectIndexes.Aerinite, new Item((int)ObjectIndexes.Aerinite) },
			{ (int)ObjectIndexes.Esperite, new Item((int)ObjectIndexes.Esperite) },
			{ (int)ObjectIndexes.FairyStone, new Item((int)ObjectIndexes.FairyStone) },
			{ (int)ObjectIndexes.Fluorapatite, new Item((int)ObjectIndexes.Fluorapatite) },
			{ (int)ObjectIndexes.Geminite, new Item((int)ObjectIndexes.Geminite) },
			{ (int)ObjectIndexes.GhostCrystal, new Item((int)ObjectIndexes.GhostCrystal) },
			{ (int)ObjectIndexes.Hematite, new Item((int)ObjectIndexes.Hematite) },
			{ (int)ObjectIndexes.Kyanite, new Item((int)ObjectIndexes.Kyanite) },
			{ (int)ObjectIndexes.Lunarite, new Item((int)ObjectIndexes.Lunarite) },
			{ (int)ObjectIndexes.Marble, new Item((int)ObjectIndexes.Marble) },
			{ (int)ObjectIndexes.OceanStone, new Item((int)ObjectIndexes.OceanStone) },
			{ (int)ObjectIndexes.Opal, new Item((int)ObjectIndexes.Opal) },
			{ (int)ObjectIndexes.Pyrite, new Item((int)ObjectIndexes.Pyrite) },
			{ (int)ObjectIndexes.Soapstone, new Item((int)ObjectIndexes.Soapstone) },

			{ (int)ObjectIndexes.Baryte, new Item((int)ObjectIndexes.Baryte) },
			{ (int)ObjectIndexes.Basalt, new Item((int)ObjectIndexes.Basalt) },
			{ (int)ObjectIndexes.Bixite, new Item((int)ObjectIndexes.Bixite) },
			{ (int)ObjectIndexes.Dolomite, new Item((int)ObjectIndexes.Dolomite) },
			{ (int)ObjectIndexes.FireOpal, new Item((int)ObjectIndexes.FireOpal) },
			{ (int)ObjectIndexes.Helvite, new Item((int)ObjectIndexes.Helvite) },
			{ (int)ObjectIndexes.Jasper, new Item((int)ObjectIndexes.Jasper) },
			{ (int)ObjectIndexes.LemonStone, new Item((int)ObjectIndexes.LemonStone) },
			{ (int)ObjectIndexes.Neptunite, new Item((int)ObjectIndexes.Neptunite) },
			{ (int)ObjectIndexes.Obsidian, new Item((int)ObjectIndexes.Obsidian) },
			{ (int)ObjectIndexes.StarShards, new Item((int)ObjectIndexes.StarShards) },
			{ (int)ObjectIndexes.Tigerseye, new Item((int)ObjectIndexes.Tigerseye) },

			// Animal items - default is ObtainingDifficulties.MediumTimeRequirements, but switches to LargeTimeRequirements if 1 upgrade to the building is required
			{ (int)ObjectIndexes.Honey, new AnimalItem((int)ObjectIndexes.Honey) { RequiresBeehouse = true } },
			{ (int)ObjectIndexes.WhiteEgg, new AnimalItem((int)ObjectIndexes.WhiteEgg) },
			{ (int)ObjectIndexes.LargeWhiteEgg, new AnimalItem((int)ObjectIndexes.LargeWhiteEgg, ObtainingDifficulties.LargeTimeRequirements) },
			{ (int)ObjectIndexes.BrownEgg, new AnimalItem((int)ObjectIndexes.BrownEgg) },
			{ (int)ObjectIndexes.LargeBrownEgg, new AnimalItem((int)ObjectIndexes.LargeBrownEgg, ObtainingDifficulties.LargeTimeRequirements) },
			{ (int)ObjectIndexes.VoidEgg, new AnimalItem((int)ObjectIndexes.VoidEgg, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.Milk, new AnimalItem((int)ObjectIndexes.Milk) },
			{ (int)ObjectIndexes.LargeMilk, new AnimalItem((int)ObjectIndexes.LargeMilk, ObtainingDifficulties.LargeTimeRequirements) },
			{ (int)ObjectIndexes.GoatMilk, new AnimalItem((int)ObjectIndexes.GoatMilk) },
			{ (int)ObjectIndexes.LargeGoatMilk, new AnimalItem((int)ObjectIndexes.LargeGoatMilk, ObtainingDifficulties.LargeTimeRequirements) },
			{ (int)ObjectIndexes.DuckEgg, new AnimalItem((int)ObjectIndexes.DuckEgg, 1) },
			{ (int)ObjectIndexes.DuckFeather, new AnimalItem((int)ObjectIndexes.DuckFeather, 1) },
			{ (int)ObjectIndexes.Wool, new AnimalItem((int)ObjectIndexes.Wool, 2) },
			{ (int)ObjectIndexes.Cloth, new AnimalItem((int)ObjectIndexes.Cloth, 2) },
			{ (int)ObjectIndexes.RabbitsFoot, new AnimalItem((int)ObjectIndexes.RabbitsFoot, 2) },
			{ (int)ObjectIndexes.Truffle, new AnimalItem((int)ObjectIndexes.Truffle, 2) },
			{ (int)ObjectIndexes.TruffleOil, new AnimalItem((int)ObjectIndexes.TruffleOil, 2) { RequiresOilMaker = true } },
			{ (int)ObjectIndexes.Mayonnaise, new AnimalItem((int)ObjectIndexes.Wool) { IsMayonaisse = true } },
			{ (int)ObjectIndexes.DuckMayonnaise, new AnimalItem((int)ObjectIndexes.Wool, 1) { IsMayonaisse = true } },
			{ (int)ObjectIndexes.VoidMayonnaise, new AnimalItem((int)ObjectIndexes.Wool) { IsMayonaisse = true, DifficultyToObtain = ObtainingDifficulties.RareItem } },
			
			// Artifacts and rare items
			{ (int)ObjectIndexes.DwarfScrollI, new ArtifactItem((int)ObjectIndexes.DwarfScrollI) },
			{ (int)ObjectIndexes.DwarfScrollII, new ArtifactItem((int)ObjectIndexes.DwarfScrollII) },
			{ (int)ObjectIndexes.DwarfScrollIII, new ArtifactItem((int)ObjectIndexes.DwarfScrollIII) },
			{ (int)ObjectIndexes.DwarfScrollIV, new ArtifactItem((int)ObjectIndexes.DwarfScrollIV) },
			{ (int)ObjectIndexes.ChippedAmphora, new ArtifactItem((int)ObjectIndexes.ChippedAmphora) },
			{ (int)ObjectIndexes.Arrowhead, new ArtifactItem((int)ObjectIndexes.Arrowhead) },
			{ (int)ObjectIndexes.AncientDoll, new ArtifactItem((int)ObjectIndexes.AncientDoll) },
			{ (int)ObjectIndexes.ElvishJewelry, new ArtifactItem((int)ObjectIndexes.ElvishJewelry) },
			{ (int)ObjectIndexes.ChewingStick, new ArtifactItem((int)ObjectIndexes.ChewingStick) },
			{ (int)ObjectIndexes.OrnamentalFan, new ArtifactItem((int)ObjectIndexes.OrnamentalFan) },
			{ (int)ObjectIndexes.AncientSword, new ArtifactItem((int)ObjectIndexes.AncientSword) },
			{ (int)ObjectIndexes.RustySpoon, new ArtifactItem((int)ObjectIndexes.RustySpoon) },
			{ (int)ObjectIndexes.RustySpur, new ArtifactItem((int)ObjectIndexes.RustySpur) },
			{ (int)ObjectIndexes.RustyCog, new ArtifactItem((int)ObjectIndexes.RustyCog) },
			{ (int)ObjectIndexes.ChickenStatue, new ArtifactItem((int)ObjectIndexes.ChickenStatue) },
			{ (int)ObjectIndexes.PrehistoricTool, new ArtifactItem((int)ObjectIndexes.PrehistoricTool) },
			{ (int)ObjectIndexes.DriedStarfish, new ArtifactItem((int)ObjectIndexes.DriedStarfish) },
			{ (int)ObjectIndexes.Anchor, new ArtifactItem((int)ObjectIndexes.Anchor) },
			{ (int)ObjectIndexes.GlassShards, new ArtifactItem((int)ObjectIndexes.GlassShards) },
			{ (int)ObjectIndexes.BoneFlute, new ArtifactItem((int)ObjectIndexes.BoneFlute) },
			{ (int)ObjectIndexes.PrehistoricHandaxe, new ArtifactItem((int)ObjectIndexes.PrehistoricHandaxe) },
			{ (int)ObjectIndexes.DwarvishHelm, new ArtifactItem((int)ObjectIndexes.DwarvishHelm) },
			{ (int)ObjectIndexes.DwarfGadget, new ArtifactItem((int)ObjectIndexes.DwarfGadget) },
			{ (int)ObjectIndexes.AncientDrum, new ArtifactItem((int)ObjectIndexes.AncientDrum) },
			{ (int)ObjectIndexes.PrehistoricScapula, new ArtifactItem((int)ObjectIndexes.PrehistoricScapula) },
			{ (int)ObjectIndexes.PrehistoricTibia, new ArtifactItem((int)ObjectIndexes.PrehistoricTibia) },
			{ (int)ObjectIndexes.PrehistoricSkull, new ArtifactItem((int)ObjectIndexes.PrehistoricSkull) },
			{ (int)ObjectIndexes.SkeletalHand, new ArtifactItem((int)ObjectIndexes.SkeletalHand) },
			{ (int)ObjectIndexes.PrehistoricRib, new ArtifactItem((int)ObjectIndexes.PrehistoricRib) },
			{ (int)ObjectIndexes.PrehistoricVertebra, new ArtifactItem((int)ObjectIndexes.PrehistoricVertebra) },
			{ (int)ObjectIndexes.SkeletalTail, new ArtifactItem((int)ObjectIndexes.SkeletalTail) },
			{ (int)ObjectIndexes.NautilusFossil, new ArtifactItem((int)ObjectIndexes.NautilusFossil) },
			{ (int)ObjectIndexes.AmphibianFossil, new ArtifactItem((int)ObjectIndexes.AmphibianFossil) },
			{ (int)ObjectIndexes.PalmFossil, new ArtifactItem((int)ObjectIndexes.PalmFossil) },
			{ (int)ObjectIndexes.Trilobite, new ArtifactItem((int)ObjectIndexes.Trilobite) },

			{ (int)ObjectIndexes.PrismaticShard, new Item((int)ObjectIndexes.PrismaticShard, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.DinosaurEgg, new ArtifactItem((int)ObjectIndexes.DinosaurEgg, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.RareDisc, new ArtifactItem((int)ObjectIndexes.RareDisc, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.GoldenMask, new ArtifactItem((int)ObjectIndexes.GoldenMask, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.GoldenRelic, new ArtifactItem((int)ObjectIndexes.GoldenRelic, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.GoldenPumpkin, new ArtifactItem((int)ObjectIndexes.GoldenPumpkin, ObtainingDifficulties.RareItem) },
			{ (int)ObjectIndexes.AncientSeed, new ArtifactItem((int)ObjectIndexes.AncientSeed, ObtainingDifficulties.RareItem) },

			// Misc
			{ (int)ObjectIndexes.Battery, new Item((int)ObjectIndexes.Battery, ObtainingDifficulties.LargeTimeRequirements) },

			// All cooking recipes - ObtainingDifficulties.LargeTimeRequirements
			{ (int)ObjectIndexes.FriedEgg, new CookedItem((int)ObjectIndexes.FriedEgg) },
			{ (int)ObjectIndexes.Omelet, new CookedItem((int)ObjectIndexes.Omelet) },
			{ (int)ObjectIndexes.Salad, new CookedItem((int)ObjectIndexes.Salad) },
			{ (int)ObjectIndexes.CheeseCauliflower, new CookedItem((int)ObjectIndexes.CheeseCauliflower) },
			{ (int)ObjectIndexes.BakedFish, new CookedItem((int)ObjectIndexes.BakedFish) },
			{ (int)ObjectIndexes.ParsnipSoup, new CookedItem((int)ObjectIndexes.ParsnipSoup) },
			{ (int)ObjectIndexes.VegetableMedley, new CookedItem((int)ObjectIndexes.VegetableMedley) },
			{ (int)ObjectIndexes.CompleteBreakfast, new CookedItem((int)ObjectIndexes.CompleteBreakfast) },
			{ (int)ObjectIndexes.FriedCalamari, new CookedItem((int)ObjectIndexes.FriedCalamari) },
			{ (int)ObjectIndexes.StrangeBun, new CookedItem((int)ObjectIndexes.StrangeBun) },

			{ (int)ObjectIndexes.LuckyLunch, new CookedItem((int)ObjectIndexes.LuckyLunch) },
			{ (int)ObjectIndexes.FriedMushroom, new CookedItem((int)ObjectIndexes.FriedMushroom) },
			{ (int)ObjectIndexes.Pizza, new CookedItem((int)ObjectIndexes.Pizza) },
			{ (int)ObjectIndexes.BeanHotpot, new CookedItem((int)ObjectIndexes.BeanHotpot) },
			{ (int)ObjectIndexes.GlazedYams, new CookedItem((int)ObjectIndexes.GlazedYams) },
			{ (int)ObjectIndexes.CarpSurprise, new CookedItem((int)ObjectIndexes.CarpSurprise) },
			{ (int)ObjectIndexes.Hashbrowns, new CookedItem((int)ObjectIndexes.Hashbrowns) },
			{ (int)ObjectIndexes.Pancakes, new CookedItem((int)ObjectIndexes.Pancakes) },
			{ (int)ObjectIndexes.SalmonDinner, new CookedItem((int)ObjectIndexes.SalmonDinner) },
			{ (int)ObjectIndexes.FishTaco, new CookedItem((int)ObjectIndexes.FishTaco) },

			{ (int)ObjectIndexes.CrispyBass, new CookedItem((int)ObjectIndexes.CrispyBass) },
			{ (int)ObjectIndexes.PepperPoppers, new CookedItem((int)ObjectIndexes.PepperPoppers) },
			{ (int)ObjectIndexes.Bread, new CookedItem((int)ObjectIndexes.Bread) },
			{ (int)ObjectIndexes.TomKhaSoup, new CookedItem((int)ObjectIndexes.TomKhaSoup) },
			{ (int)ObjectIndexes.TroutSoup, new CookedItem((int)ObjectIndexes.TroutSoup) },
			{ (int)ObjectIndexes.ChocolateCake, new CookedItem((int)ObjectIndexes.ChocolateCake) },
			{ (int)ObjectIndexes.PinkCake, new CookedItem((int)ObjectIndexes.PinkCake) },
			{ (int)ObjectIndexes.RhubarbPie, new CookedItem((int)ObjectIndexes.RhubarbPie) },
			{ (int)ObjectIndexes.Cookie, new CookedItem((int)ObjectIndexes.Cookie) },
			{ (int)ObjectIndexes.Spaghetti, new CookedItem((int)ObjectIndexes.Spaghetti) },

			{ (int)ObjectIndexes.FriedEel, new CookedItem((int)ObjectIndexes.FriedEel) },
			{ (int)ObjectIndexes.SpicyEel, new CookedItem((int)ObjectIndexes.SpicyEel) },
			{ (int)ObjectIndexes.Sashimi, new CookedItem((int)ObjectIndexes.Sashimi) },
			{ (int)ObjectIndexes.MakiRoll, new CookedItem((int)ObjectIndexes.MakiRoll) },
			{ (int)ObjectIndexes.Tortilla, new CookedItem((int)ObjectIndexes.Tortilla) },
			{ (int)ObjectIndexes.RedPlate, new CookedItem((int)ObjectIndexes.RedPlate) },
			{ (int)ObjectIndexes.EggplantParmesan, new CookedItem((int)ObjectIndexes.EggplantParmesan) },
			{ (int)ObjectIndexes.RicePudding, new CookedItem((int)ObjectIndexes.RicePudding) },
			{ (int)ObjectIndexes.IceCream, new CookedItem((int)ObjectIndexes.IceCream) },
			{ (int)ObjectIndexes.BlueberryTart, new CookedItem((int)ObjectIndexes.BlueberryTart) },

			{ (int)ObjectIndexes.AutumnsBounty, new CookedItem((int)ObjectIndexes.AutumnsBounty) },
			{ (int)ObjectIndexes.PumpkinSoup, new CookedItem((int)ObjectIndexes.PumpkinSoup) },
			{ (int)ObjectIndexes.SuperMeal, new CookedItem((int)ObjectIndexes.SuperMeal) },
			{ (int)ObjectIndexes.CranberrySauce, new CookedItem((int)ObjectIndexes.CranberrySauce) },
			{ (int)ObjectIndexes.Stuffing, new CookedItem((int)ObjectIndexes.Stuffing) },
			{ (int)ObjectIndexes.FarmersLunch, new CookedItem((int)ObjectIndexes.FarmersLunch) },
			{ (int)ObjectIndexes.SurvivalBurger, new CookedItem((int)ObjectIndexes.SurvivalBurger) },
			{ (int)ObjectIndexes.DishOTheSea, new CookedItem((int)ObjectIndexes.DishOTheSea) },
			{ (int)ObjectIndexes.MinersTreat, new CookedItem((int)ObjectIndexes.MinersTreat) },
			{ (int)ObjectIndexes.RootsPlatter, new CookedItem((int)ObjectIndexes.RootsPlatter) },

			{ (int)ObjectIndexes.AlgaeSoup, new CookedItem((int)ObjectIndexes.AlgaeSoup) },
			{ (int)ObjectIndexes.PaleBroth, new CookedItem((int)ObjectIndexes.PaleBroth) },
			{ (int)ObjectIndexes.PlumPudding, new CookedItem((int)ObjectIndexes.PlumPudding) },
			{ (int)ObjectIndexes.ArtichokeDip, new CookedItem((int)ObjectIndexes.ArtichokeDip) },
			{ (int)ObjectIndexes.StirFry, new CookedItem((int)ObjectIndexes.StirFry) },
			{ (int)ObjectIndexes.RoastedHazelnuts, new CookedItem((int)ObjectIndexes.RoastedHazelnuts) },
			{ (int)ObjectIndexes.PumpkinPie, new CookedItem((int)ObjectIndexes.PumpkinPie) },
			{ (int)ObjectIndexes.RadishSalad, new CookedItem((int)ObjectIndexes.RadishSalad) },
			{ (int)ObjectIndexes.FruitSalad, new CookedItem((int)ObjectIndexes.FruitSalad) },
			{ (int)ObjectIndexes.BlackberryCobbler, new CookedItem((int)ObjectIndexes.BlackberryCobbler) },

			{ (int)ObjectIndexes.CranberryCandy, new CookedItem((int)ObjectIndexes.CranberryCandy) },
			{ (int)ObjectIndexes.Bruschetta, new CookedItem((int)ObjectIndexes.Bruschetta) },
			{ (int)ObjectIndexes.Coleslaw, new CookedItem((int)ObjectIndexes.Coleslaw) },
			{ (int)ObjectIndexes.FiddleheadRisotto, new CookedItem((int)ObjectIndexes.FiddleheadRisotto) },
			{ (int)ObjectIndexes.PoppyseedMuffin, new CookedItem((int)ObjectIndexes.PoppyseedMuffin) },
			{ (int)ObjectIndexes.Chowder, new CookedItem((int)ObjectIndexes.Chowder) },
			{ (int)ObjectIndexes.LobsterBisque, new CookedItem((int)ObjectIndexes.LobsterBisque) },
			{ (int)ObjectIndexes.Escargot, new CookedItem((int)ObjectIndexes.Escargot) },
			{ (int)ObjectIndexes.FishStew, new CookedItem((int)ObjectIndexes.FishStew) },
			{ (int)ObjectIndexes.MapleBar, new CookedItem((int)ObjectIndexes.MapleBar) },

			{ (int)ObjectIndexes.CrabCakes, new CookedItem((int)ObjectIndexes.CrabCakes) },
			
			// ------ All Foragables - ObtainingDifficulties.LargeTimeRequirements -------
			// Spring Foragables - TODO: look into Salmonberries and Spring Onions
			{ (int)ObjectIndexes.WildHorseradish, new ForagableItem((int)ObjectIndexes.WildHorseradish) },
			{ (int)ObjectIndexes.Daffodil, new ForagableItem((int)ObjectIndexes.Daffodil) },
			{ (int)ObjectIndexes.Leek, new ForagableItem((int)ObjectIndexes.Leek) },
			{ (int)ObjectIndexes.Dandelion, new ForagableItem((int)ObjectIndexes.Dandelion) },
			{ (int)ObjectIndexes.Morel, new ForagableItem((int)ObjectIndexes.Morel) },
			{ (int)ObjectIndexes.CommonMushroom, new ForagableItem((int)ObjectIndexes.CommonMushroom) }, // Also fall

			// Summer Foragables
			{ (int)ObjectIndexes.SpiceBerry, new ForagableItem((int)ObjectIndexes.SpiceBerry) },
			{ (int)ObjectIndexes.Grape, new ForagableItem((int)ObjectIndexes.Grape) },
			{ (int)ObjectIndexes.SweetPea, new ForagableItem((int)ObjectIndexes.SweetPea) },
			{ (int)ObjectIndexes.RedMushroom, new ForagableItem((int)ObjectIndexes.RedMushroom) }, // Also fall
			{ (int)ObjectIndexes.FiddleheadFern, new ForagableItem((int)ObjectIndexes.FiddleheadFern) },

			// Fall Foragables
			{ (int)ObjectIndexes.WildPlum, new ForagableItem((int)ObjectIndexes.WildPlum) },
			{ (int)ObjectIndexes.Hazelnut, new ForagableItem((int)ObjectIndexes.Hazelnut) },
			{ (int)ObjectIndexes.Blackberry, new ForagableItem((int)ObjectIndexes.Blackberry) },
			{ (int)ObjectIndexes.Chanterelle, new ForagableItem((int)ObjectIndexes.Chanterelle) },

			// Winter Foragables
			{ (int)ObjectIndexes.WinterRoot, new ForagableItem((int)ObjectIndexes.WinterRoot) },
			{ (int)ObjectIndexes.CrystalFruit, new ForagableItem((int)ObjectIndexes.CrystalFruit) },
			{ (int)ObjectIndexes.SnowYam, new ForagableItem((int)ObjectIndexes.SnowYam) },
			{ (int)ObjectIndexes.Crocus, new ForagableItem((int)ObjectIndexes.Crocus) },
			{ (int)ObjectIndexes.Holly, new ForagableItem((int)ObjectIndexes.Holly) },

			// Beach Foragables - the medium ones can also be obtained from crab pots
			{ (int)ObjectIndexes.NautilusShell, new ForagableItem((int)ObjectIndexes.NautilusShell) },
			{ (int)ObjectIndexes.Coral, new ForagableItem((int)ObjectIndexes.Coral) },
			{ (int)ObjectIndexes.SeaUrchin, new ForagableItem((int)ObjectIndexes.SeaUrchin) },
			{ (int)ObjectIndexes.RainbowShell, new ForagableItem((int)ObjectIndexes.RainbowShell) },
			{ (int)ObjectIndexes.Clam, new ForagableItem((int)ObjectIndexes.Clam) { DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements, IsCrabPotItem = true } },
			{ (int)ObjectIndexes.Cockle, new ForagableItem((int)ObjectIndexes.Cockle) { DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements, IsCrabPotItem = true } },
			{ (int)ObjectIndexes.Mussel, new ForagableItem((int)ObjectIndexes.Mussel) { DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements, IsCrabPotItem = true } },
			{ (int)ObjectIndexes.Oyster, new ForagableItem((int)ObjectIndexes.Oyster) { DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements, IsCrabPotItem = true } },

			// Desert Foragables
			{ (int)ObjectIndexes.Coconut, new ForagableItem((int)ObjectIndexes.Coconut) },
			{ (int)ObjectIndexes.CactusFruit, new ForagableItem((int)ObjectIndexes.CactusFruit) },

			// Fruit - since trees are randomized, we're making them foragable
			{ (int)ObjectIndexes.Cherry, new ForagableItem((int)ObjectIndexes.Cherry) },
			{ (int)ObjectIndexes.Apricot, new ForagableItem((int)ObjectIndexes.Apricot) },
			{ (int)ObjectIndexes.Orange, new ForagableItem((int)ObjectIndexes.Orange) },
			{ (int)ObjectIndexes.Peach, new ForagableItem((int)ObjectIndexes.Peach) },
			{ (int)ObjectIndexes.Pomegranate, new ForagableItem((int)ObjectIndexes.Pomegranate) },
			{ (int)ObjectIndexes.Apple, new ForagableItem((int)ObjectIndexes.Apple) },
			// ------ End Foragables -------

			// Smelted Items - ObtainingDifficulties.MediumTimeRequirements
			{ (int)ObjectIndexes.RefinedQuartz, new SmeltedItem((int)ObjectIndexes.RefinedQuartz) },
			{ (int)ObjectIndexes.CopperBar, new SmeltedItem((int)ObjectIndexes.CopperBar) },
			{ (int)ObjectIndexes.IronBar, new SmeltedItem((int)ObjectIndexes.IronBar) },
			{ (int)ObjectIndexes.GoldBar, new SmeltedItem((int)ObjectIndexes.GoldBar) },
			{ (int)ObjectIndexes.IridiumBar, new SmeltedItem((int)ObjectIndexes.IridiumBar, ObtainingDifficulties.EndgameItem) },

			// Trash items - ObtainingDifficulties.NoRequirements
			{ (int)ObjectIndexes.BrokenCD, new TrashItem((int)ObjectIndexes.BrokenCD) { OverrideName = "Broken CD" } },
			{ (int)ObjectIndexes.SoggyNewspaper, new TrashItem((int)ObjectIndexes.SoggyNewspaper) },
			{ (int)ObjectIndexes.Driftwood, new TrashItem((int)ObjectIndexes.Driftwood) },
			{ (int)ObjectIndexes.BrokenGlasses, new TrashItem((int)ObjectIndexes.BrokenGlasses) },
			{ (int)ObjectIndexes.JojaCola, new TrashItem((int)ObjectIndexes.JojaCola) },
		};
	}
}
