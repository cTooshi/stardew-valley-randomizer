using System.Collections.Generic;

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

		//TODO: crab pot stuff
		//TODO: the rest of the crafting recipes
		//TODO: cooking recipes
		//TODO: geodes
		public static Dictionary<int, Item> Items = new Dictionary<int, Item>
		{
			// Craftable items
			{ (int)ObjectIndexes.Torch, new CraftableItem((int)ObjectIndexes.Torch, "/Field/93/false/l 0", CraftableCategories.Cheap) { DifficultyToObtain = ObtainingDifficulties.SmallTimeRequirements } }, // You can find it in the mines
			{ (int)ObjectIndexes.Chest, new CraftableItem((int)ObjectIndexes.Chest, "/Home/130/true/null", CraftableCategories.Cheap) { DifficultyToObtain = ObtainingDifficulties.Impossible } }, // There's no item ID for this, so you can't exactly require it in a recipe
			{ (int)ObjectIndexes.WoodPath, new CraftableItem((int)ObjectIndexes.WoodPath, "/Field/405/false/l 0", CraftableCategories.CheapAndNeedMany) },
			{ (int)ObjectIndexes.GravelPath, new CraftableItem((int)ObjectIndexes.GravelPath, "/Field/407/false/l 0", CraftableCategories.CheapAndNeedMany) },
			{ (int)ObjectIndexes.CobblestonePath, new CraftableItem((int)ObjectIndexes.CobblestonePath, "/Field/411/false/l 0", CraftableCategories.CheapAndNeedMany) },
			{ (int)ObjectIndexes.SteppingStonePath, new CraftableItem((int)ObjectIndexes.SteppingStonePath, "/Field/415/false/l 0", CraftableCategories.CheapAndNeedMany) },

			// Resources - ObtainingDifficulties.NoRequirements
			{ (int)ObjectIndexes.Wood, new ResourceItem((int)ObjectIndexes.Wood) },
			{ (int)ObjectIndexes.Stone, new ResourceItem((int)ObjectIndexes.Stone) },
			{ (int)ObjectIndexes.Fiber, new ResourceItem((int)ObjectIndexes.Fiber, 3, new Range(1, 5)) },
			{ (int)ObjectIndexes.Clay, new ResourceItem((int)ObjectIndexes.Clay, 1, new Range(1, 15)) },

			// Items you get as a byproduct of collection resources
			{ (int)ObjectIndexes.Sap, new Item((int)ObjectIndexes.Sap, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 15) } },
			{ (int)ObjectIndexes.Acorn, new Item((int)ObjectIndexes.Acorn, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.MapleSeed, new Item((int)ObjectIndexes.MapleSeed, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.PineCone, new Item((int)ObjectIndexes.PineCone, ObtainingDifficulties.NoRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.MixedSeeds, new Item((int)ObjectIndexes.MixedSeeds, ObtainingDifficulties.NoRequirements) },

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

			// Items you can find in the mines
			{ (int)ObjectIndexes.CaveCarrot, new Item((int)ObjectIndexes.CaveCarrot, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.BugMeat, new Item((int)ObjectIndexes.BugMeat, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.Slime, new Item((int)ObjectIndexes.BugMeat, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 10) } },
			{ (int)ObjectIndexes.BatWing, new Item((int)ObjectIndexes.BatWing, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.VoidEssence, new Item((int)ObjectIndexes.VoidEssence, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.SolarEssence, new Item((int)ObjectIndexes.SolarEssence, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },

			{ (int)ObjectIndexes.Coal, new Item((int)ObjectIndexes.Coal, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.CopperOre, new Item((int)ObjectIndexes.CopperOre, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.IronOre, new Item((int)ObjectIndexes.IronOre, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 5) } },
			{ (int)ObjectIndexes.IridiumOre, new Item((int)ObjectIndexes.IridiumOre, ObtainingDifficulties.EndgameItem) { ItemsRequiredForRecipe = new Range(1, 5) } },

			{ (int)ObjectIndexes.Quartz, new Item((int)ObjectIndexes.Quartz, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.FireQuartz, new Item((int)ObjectIndexes.FireQuartz, ObtainingDifficulties.MediumTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.EarthCrystal, new Item((int)ObjectIndexes.EarthCrystal, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },
			{ (int)ObjectIndexes.FrozenTear, new Item((int)ObjectIndexes.FrozenTear, ObtainingDifficulties.SmallTimeRequirements) { ItemsRequiredForRecipe = new Range(1, 3) } },

			{ (int)ObjectIndexes.Aquamarine, new Item((int)ObjectIndexes.Aquamarine, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Amethyst, new Item((int)ObjectIndexes.Amethyst, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Emerald, new Item((int)ObjectIndexes.Emerald, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Ruby, new Item((int)ObjectIndexes.Ruby, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Topaz, new Item((int)ObjectIndexes.Topaz, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Jade, new Item((int)ObjectIndexes.Jade, ObtainingDifficulties.MediumTimeRequirements) },
			{ (int)ObjectIndexes.Diamond, new Item((int)ObjectIndexes.Diamond, ObtainingDifficulties.MediumTimeRequirements) },

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

			// Beach Foragables
			{ (int)ObjectIndexes.NautilusShell, new ForagableItem((int)ObjectIndexes.NautilusShell) },
			{ (int)ObjectIndexes.Coral, new ForagableItem((int)ObjectIndexes.Coral) },
			{ (int)ObjectIndexes.SeaUrchin, new ForagableItem((int)ObjectIndexes.SeaUrchin) },
			{ (int)ObjectIndexes.RainbowShell, new ForagableItem((int)ObjectIndexes.RainbowShell) },
			{ (int)ObjectIndexes.Clam, new ForagableItem((int)ObjectIndexes.Clam) },
			{ (int)ObjectIndexes.Cockle, new ForagableItem((int)ObjectIndexes.Cockle) },
			{ (int)ObjectIndexes.Mussel, new ForagableItem((int)ObjectIndexes.Mussel) },
			{ (int)ObjectIndexes.Oyster, new ForagableItem((int)ObjectIndexes.Oyster) },

			// Desert Forabagles
			{ (int)ObjectIndexes.Coconut, new ForagableItem((int)ObjectIndexes.Coconut) },
			{ (int)ObjectIndexes.CactusFruit, new ForagableItem((int)ObjectIndexes.CactusFruit) },
			// ------ End Foragables -------

			// Smelted Items - ObtainingDifficulties.MediumTimeRequirements
			{ (int)ObjectIndexes.RefinedQuartz, new SmeltedItem((int)ObjectIndexes.RefinedQuartz) },
			{ (int)ObjectIndexes.CopperBar, new SmeltedItem((int)ObjectIndexes.CopperBar) },
			{ (int)ObjectIndexes.IronBar, new SmeltedItem((int)ObjectIndexes.IronBar) },
			{ (int)ObjectIndexes.IridiumBar, new SmeltedItem((int)ObjectIndexes.IridiumBar, ObtainingDifficulties.EndgameItem) },

			// Trash items - ObtainingDifficulties.NoRequirements
			{ (int)ObjectIndexes.BrokenCD, new TrashItem((int)ObjectIndexes.BrokenCD) },
			{ (int)ObjectIndexes.SoggyNewspaper, new TrashItem((int)ObjectIndexes.SoggyNewspaper) },
			{ (int)ObjectIndexes.Driftwood, new TrashItem((int)ObjectIndexes.Driftwood) },
			{ (int)ObjectIndexes.BrokenGlasses, new TrashItem((int)ObjectIndexes.BrokenGlasses) },
			{ (int)ObjectIndexes.JojaCola, new TrashItem((int)ObjectIndexes.JojaCola) },
		};
	}
}
