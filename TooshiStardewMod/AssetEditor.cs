using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class AssetEditor : IAssetEditor
	{
		private readonly ModEntry _mod;
		private readonly Dictionary<string, string> _recipeReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _bundleReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _blueprintReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _stringReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _farmEventReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _mailReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _fishReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _questReplacements = new Dictionary<int, string>();
		private Dictionary<string, string> _locationsReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _objectInformationReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _fruitTreeReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _cropReplacements = new Dictionary<int, string>();
		public Dictionary<string, string> MusicReplacements = new Dictionary<string, string>();

		public AssetEditor(ModEntry mod)
		{
			this._mod = mod;
		}

		public bool CanEdit<T>(IAssetInfo asset)
		{
			if (asset.AssetNameEquals("Data/CraftingRecipes")) { return ModEntry.configDict.ContainsKey("crafting recipes") ? ModEntry.configDict["crafting recipes"] : true; }
			if (asset.AssetNameEquals("Data/Bundles")) { return ModEntry.configDict.ContainsKey("bundles") ? ModEntry.configDict["bundles"] : true; }
			if (asset.AssetNameEquals("Data/Blueprints")) { return ModEntry.configDict.ContainsKey("building prices/mats") ? ModEntry.configDict["building prices/mats"] : true; }
			if (asset.AssetNameEquals("Strings/StringsFromCSFiles")) { return ModEntry.configDict.ContainsKey("intro cutscene madlib") ? ModEntry.configDict["intro cutscene madlib"] : true; }
			if (asset.AssetNameEquals("Data/ObjectInformation")) { return ModEntry.configDict.ContainsKey("crops prices") ? ModEntry.configDict["crop prices"] : true; }
			if (asset.AssetNameEquals("Data/Events/Farm")) { return true; }
			if (asset.AssetNameEquals("Data/Mail")) { return true; }
			if (asset.AssetNameEquals("Data/Fish")) { return ModEntry.configDict.ContainsKey("fishing difficulty") ? ModEntry.configDict["fishing difficulty"] : true; }
			if (asset.AssetNameEquals("Data/Quests")) { return true; }
			if (asset.AssetNameEquals("Data/Locations")) { return true; } //TODO: add a setting for this
			if (asset.AssetNameEquals("Data/fruitTrees")) { return true; } //TODO: add a setting for this
			if (asset.AssetNameEquals("Data/Crops")) { return true; } //TODO: add a setting for this
			return false;
		}

		private void ApplyEdits<TKey, TValue>(IAssetData asset, IDictionary<TKey, TValue> edits)
		{
			IAssetDataForDictionary<TKey, TValue> assetDict = asset.AsDictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> edit in edits)
			{
				assetDict.Set(edit.Key, edit.Value);
			}
		}

		public void Edit<T>(IAssetData asset)
		{
			if (asset.AssetNameEquals("Data/CraftingRecipes"))
			{
				this.ApplyEdits(asset, this._recipeReplacements);
			}
			else if (asset.AssetNameEquals("Data/Bundles"))
			{
				this.ApplyEdits(asset, this._bundleReplacements);
			}
			else if (asset.AssetNameEquals("Data/Blueprints"))
			{
				this.ApplyEdits(asset, this._blueprintReplacements);
			}
			else if (asset.AssetNameEquals("Strings/StringsFromCSFiles"))
			{
				this.ApplyEdits(asset, this._stringReplacements);
			}
			else if (asset.AssetNameEquals("Data/ObjectInformation"))
			{
				this.ApplyEdits(asset, this._objectInformationReplacements);
			}
			else if (asset.AssetNameEquals("Data/Events/Farm"))
			{
				this.ApplyEdits(asset, this._farmEventReplacements);
			}
			else if (asset.AssetNameEquals("Data/Mail"))
			{
				this.ApplyEdits(asset, this._mailReplacements);
			}
			else if (asset.AssetNameEquals("Data/Fish"))
			{
				this.ApplyEdits(asset, this._fishReplacements);
			}
			else if (asset.AssetNameEquals("Data/Quests"))
			{
				this.ApplyEdits(asset, this._questReplacements);
			}
			else if (asset.AssetNameEquals("Data/Locations"))
			{
				this.ApplyEdits(asset, this._locationsReplacements);
			}
			else if (asset.AssetNameEquals("Data/fruitTrees"))
			{
				this.ApplyEdits(asset, this._fruitTreeReplacements);
			}
			else if (asset.AssetNameEquals("Data/Crops"))
			{
				this.ApplyEdits(asset, this._cropReplacements);
			}
		}

		public void InvalidateCache()
		{
			this._mod.Helper.Content.InvalidateCache("Data/CraftingRecipes");
			this._mod.Helper.Content.InvalidateCache("Data/Bundles");
			this._mod.Helper.Content.InvalidateCache("Data/Blueprints");
			this._mod.Helper.Content.InvalidateCache("Strings/StringsFromCSFiles");
			this._mod.Helper.Content.InvalidateCache("Data/ObjectInformation");
			this._mod.Helper.Content.InvalidateCache("Data/Events/Farm");
			this._mod.Helper.Content.InvalidateCache("Data/Mail");
			this._mod.Helper.Content.InvalidateCache("Data/Fish");
			this._mod.Helper.Content.InvalidateCache("Data/Quests");
			this._mod.Helper.Content.InvalidateCache("Data/Locations");
			this._mod.Helper.Content.InvalidateCache("Data/fruitTrees");
			this._mod.Helper.Content.InvalidateCache("Data/Crops");
		}

		//Too change before save is loaded/created
		public void CalculateEditsBeforeLoad(Random placeHolderNumber)
		{
			this.CalculateStringEdits(placeHolderNumber);
		}

		public void CalculateEdits()
		{
			ValidateItemList();

			EditedObjectInformation editedObjectInfo = new EditedObjectInformation();
			CropRandomizer.Randomize(editedObjectInfo);
			_fruitTreeReplacements = editedObjectInfo.FruitTreeReplacements;
			_cropReplacements = editedObjectInfo.CropsReplacements;

			FishRandomizer.Randomize(editedObjectInfo);
			_fishReplacements = editedObjectInfo.FishReplacements;

			_objectInformationReplacements = editedObjectInfo.ObjectInformationReplacements;

			this.CalculateRecipeEdits();
			this.CalculateBlueprintEdits();
			//this.CalculateFarmEventEdits();
			//this.CalculateMailEdits();

			_locationsReplacements = LocationRandomizer.Randomize();
			_bundleReplacements = BundleRandomizer.Randomize(); // This needs to happen after the location AND the crop replacements
			MusicReplacements = MusicRandomizer.Randomize();
			_questReplacements = QuestRandomizer.Randomize();
		}

		/// <summary>
		/// Validates that all the items in the ObjectIndexes exist in the main item list
		/// </summary>
		private void ValidateItemList()
		{
			bool foundIssue = false;
			foreach (ObjectIndexes index in Enum.GetValues(typeof(ObjectIndexes)).Cast<ObjectIndexes>())
			{
				if (!ItemList.Items.ContainsKey((int)index))
				{
					foundIssue = true;
					Globals.ConsoleWrite($"Missing item: {(int)index}: {index.ToString()}");
				}
			}
			if (!foundIssue) { Globals.ConsoleWrite("No issues found with the item index!"); }

			//TODO: move me
			//foreach (string cropItem in CropGrowthInformation.DefaultStringData.Values)
			//{
			//	Globals.ConsoleWrite($"Crop string: {CropGrowthInformation.ParseString(cropItem).ToString()}");
			//}
		}

		private void CalculateBlueprintEdits()
		{
			this._blueprintReplacements.Clear();
			Random rng = Globals.RNG;

			string[] siloValues = new string[6];
			siloValues[0] = $"{ObjectIndexes.Stone:D} {rng.Next(50, 200)} {ObjectIndexes.Clay:D} {rng.Next(5, 20)} {ObjectIndexes.CopperBar:D} {rng.Next(2, 10)}";
			siloValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(25, 100)} {ObjectIndexes.Wood:D} {rng.Next(50, 100)} {ObjectIndexes.IronBar:D} {rng.Next(2, 6)}";
			siloValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 200)} {ObjectIndexes.Fiber:D} {rng.Next(5, 20)} {ObjectIndexes.Clay:D} {rng.Next(2, 10)}";
			siloValues[3] = $"{ObjectIndexes.Stone:D} {rng.Next(50, 100)} {ObjectIndexes.Sap:D} {rng.Next(20, 40)} {ObjectIndexes.CopperOre:D} {rng.Next(10, 40)}";
			siloValues[4] = $"{ObjectIndexes.Wood:D} {rng.Next(25, 150)} {ObjectIndexes.EarthCrystal:D} {rng.Next(1, 4)} {ObjectIndexes.CopperBar:D} {rng.Next(2, 11)}";
			siloValues[5] = $"{ObjectIndexes.Stone:D} {rng.Next(25, 75)} {ObjectIndexes.Hardwood:D} {rng.Next(5, 10)} {ObjectIndexes.Clay:D} {rng.Next(2, 10)}";

			string[] millValues = new string[4];
			millValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(25, 300)} {ObjectIndexes.Stone:D} {rng.Next(20, 120)} {ObjectIndexes.Cloth:D} {rng.Next(2, 7)}";
			millValues[1] = $"{ObjectIndexes.Hardwood:D} {rng.Next(5, 25)} {ObjectIndexes.Clay:D} {rng.Next(2, 10)} {ObjectIndexes.Cloth:D} {rng.Next(2, 7)}";
			millValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 300)} {ObjectIndexes.Hay:D} {rng.Next(25, 50)} {ObjectIndexes.CopperBar:D} {rng.Next(2, 11)}";
			millValues[3] = $"{ObjectIndexes.Stone:D} {rng.Next(25, 200)} {ObjectIndexes.IronBar:D} {rng.Next(1, 11)} {ObjectIndexes.GoldBar:D} {rng.Next(1, 6)}";

			string[] coopValues = new string[6];
			coopValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(100, 500)} {ObjectIndexes.Stone:D} {rng.Next(25, 175)} {ObjectIndexes.Fiber:D} {rng.Next(10, 30)}";
			coopValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(50, 350)} {ObjectIndexes.Fiber:D} {rng.Next(20, 60)} {ObjectIndexes.CopperBar:D} {rng.Next(2, 6)}";
			coopValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 600)} {ObjectIndexes.Fiber:D} {rng.Next(5, 60)} {ObjectIndexes.Clay:D} {rng.Next(2, 11)}";
			coopValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(100, 400)} {ObjectIndexes.Sap:D} {rng.Next(10, 30)} {ObjectIndexes.CopperOre:D} {rng.Next(10, 60)}";
			coopValues[4] = $"{ObjectIndexes.Wood:D} {rng.Next(100, 500)} {ObjectIndexes.EarthCrystal:D} {rng.Next(1, 4)} {ObjectIndexes.CopperBar:D} {rng.Next(2, 6)}";
			coopValues[5] = $"{ObjectIndexes.Stone:D} {rng.Next(100, 500)} {ObjectIndexes.LargemouthBass:D} 1 {ObjectIndexes.Clay:D} {rng.Next(2, 11)}";

			string[] stableValues = new string[4];
			stableValues[0] = $"{ObjectIndexes.Hardwood:D} {rng.Next(50, 150)} {ObjectIndexes.IronBar:D} {rng.Next(2, 11)}";
			stableValues[1] = $"{ObjectIndexes.Hardwood:D} {rng.Next(50, 150)} {ObjectIndexes.GoldBar:D} {rng.Next(2, 11)} {ObjectIndexes.Stone:D} {rng.Next(50, 150)}";
			stableValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(300, 600)} {ObjectIndexes.Hay:D} {rng.Next(100, 200)} {ObjectIndexes.GoldBar:D} {rng.Next(5, 21)}";
			stableValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(200, 400)} {ObjectIndexes.IronBar:D} {rng.Next(5, 11)} {ObjectIndexes.IridiumBar:D} {rng.Next(2, 11)}";

			string[] shippingBinValues = new string[4];
			shippingBinValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 250)} {ObjectIndexes.Stone:D} {rng.Next(20, 60)}";
			shippingBinValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(50, 200)} {ObjectIndexes.Clay:D} {rng.Next(1, 15)}";
			shippingBinValues[2] = $"{ObjectIndexes.Hardwood:D} {rng.Next(5, 30)} {ObjectIndexes.Fiber:D} {rng.Next(5, 20)} {ObjectIndexes.CopperBar:D} {rng.Next(1, 3)}";
			shippingBinValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 350)} {ObjectIndexes.Sap:D} {rng.Next(10, 50)} {ObjectIndexes.RefinedQuartz:D} {rng.Next(1, 3)}";

			string[] bigCoopValues = new string[4];
			bigCoopValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(200, 600)} {ObjectIndexes.Stone:D} {rng.Next(50, 350)} {ObjectIndexes.IronBar:D} {rng.Next(1, 6)}";
			bigCoopValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(200, 400)} {ObjectIndexes.Hay:D} {rng.Next(20, 80)} {ObjectIndexes.EarthCrystal:D} 1";
			bigCoopValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 600)} {ObjectIndexes.FireQuartz:D} 1 {ObjectIndexes.Clay:D} {rng.Next(2, 11)}";
			bigCoopValues[3] = $"{ObjectIndexes.Hardwood:D} {rng.Next(30, 60)} {ObjectIndexes.Wood:D} {rng.Next(100, 200)} {ObjectIndexes.GoldBar:D} {rng.Next(1, 3)}";

			string[] deluxeCoopValues = new string[4];
			deluxeCoopValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(250, 750)} {ObjectIndexes.Stone:D} {rng.Next(50, 350)} {ObjectIndexes.IronBar:D} {rng.Next(1, 6)}";
			deluxeCoopValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(200, 600)} {ObjectIndexes.Jade:D} 1 {ObjectIndexes.RefinedQuartz:D} {rng.Next(2, 6)}";
			deluxeCoopValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 700)} {ObjectIndexes.Aquamarine:D} 1 {ObjectIndexes.Clay:D} {rng.Next(2, 11)}";
			deluxeCoopValues[3] = $"{ObjectIndexes.Hardwood:D} {rng.Next(30, 70)} {ObjectIndexes.Wood:D} {rng.Next(100, 200)} {ObjectIndexes.GoldBar:D} {rng.Next(1, 6)}";

			string[] barnValues = new string[4];
			barnValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 450)} {ObjectIndexes.Stone:D} {rng.Next(100, 300)} {ObjectIndexes.IronBar:D} {rng.Next(1, 6)}";
			barnValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(100, 300)} {ObjectIndexes.Hay:D} {rng.Next(10, 50)} {ObjectIndexes.Clay:D} {rng.Next(5, 11)}";
			barnValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 350)} {ObjectIndexes.Fiber:D} {rng.Next(5, 60)} {ObjectIndexes.RefinedQuartz:D} {rng.Next(1, 6)}";
			barnValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 500)} {ObjectIndexes.Stone:D} {rng.Next(50, 150)} {ObjectIndexes.LargemouthBass:D} 1";

			string[] bigBarnValues = new string[4];
			bigBarnValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 450)} {ObjectIndexes.Stone:D} {rng.Next(100, 300)} {ObjectIndexes.IronBar:D} {rng.Next(1, 6)}";
			bigBarnValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(100, 300)} {ObjectIndexes.FrozenTear:D} 1 {ObjectIndexes.CopperBar:D} {rng.Next(2, 6)}";
			bigBarnValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 700)} {ObjectIndexes.EarthCrystal:D} {rng.Next(1, 6)} {ObjectIndexes.Clay:D} {rng.Next(2, 11)}";
			bigBarnValues[3] = $"{ObjectIndexes.Hardwood:D} {rng.Next(5, 25)} {ObjectIndexes.GoldBar:D} {rng.Next(2, 7)} {ObjectIndexes.OmniGeode:D} {rng.Next(1, 15)}";

			string[] deluxeBarnValues = new string[4];
			deluxeBarnValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 450)} {ObjectIndexes.Stone:D} {rng.Next(100, 300)} {ObjectIndexes.IronBar:D} {rng.Next(1, 6)}";
			deluxeBarnValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(100, 300)} {ObjectIndexes.RefinedQuartz:D} 1 {ObjectIndexes.IridiumBar:D} {rng.Next(2, 6)}";
			deluxeBarnValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(150, 700)} {ObjectIndexes.Cloth:D} {rng.Next(1, 8)} {ObjectIndexes.GoldBar:D} {rng.Next(2, 10)}";
			deluxeBarnValues[3] = $"{ObjectIndexes.Hardwood:D} {rng.Next(30, 70)} {ObjectIndexes.Wood:D} {rng.Next(100, 200)} {ObjectIndexes.GoldBar:D} {rng.Next(1, 6)}";

			string[] slimeHutchValues = new string[4];
			slimeHutchValues[0] = $"{ObjectIndexes.Stone:D} {rng.Next(250, 750)} {ObjectIndexes.RefinedQuartz:D} {rng.Next(1, 20)} {ObjectIndexes.IridiumBar:D} {rng.Next(1, 3)}";
			slimeHutchValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(250, 750)} {ObjectIndexes.GoldBar:D} {rng.Next(1, 10)} {ObjectIndexes.Cloth:D} {rng.Next(2, 15)}";
			slimeHutchValues[2] = $"{ObjectIndexes.Stone:D} {rng.Next(250, 750)} {ObjectIndexes.IridiumBar:D} {rng.Next(1, 6)} {ObjectIndexes.TruffleOil:D} {rng.Next(1, 3)}";
			slimeHutchValues[3] = $"{ObjectIndexes.Stone:D} {rng.Next(250, 750)} {ObjectIndexes.Quartz:D} {rng.Next(1, 20)} {ObjectIndexes.IridiumBar:D} {rng.Next(1, 3)}";

			string[] shedValues = new string[4];
			shedValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(100, 500)} {ObjectIndexes.Stone:D} {rng.Next(25, 75)}";
			shedValues[1] = $"{ObjectIndexes.Hardwood:D} {rng.Next(25, 100)} {ObjectIndexes.IronBar:D} {rng.Next(1, 7)}";
			shedValues[2] = $"{ObjectIndexes.Stone:D} {rng.Next(75, 350)} {ObjectIndexes.Topaz:D} {rng.Next(1, 4)} {ObjectIndexes.Fiber:D} {rng.Next(5, 50)}";
			shedValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(50, 250)} {ObjectIndexes.Quartz:D} {rng.Next(1, 4)} {ObjectIndexes.Jade:D} 1";

			string[] stoneCabinValues = new string[4];
			stoneCabinValues[0] = $"{ObjectIndexes.Stone:D} {rng.Next(1, 50)} {ObjectIndexes.Sap:D} {rng.Next(1, 10)}";
			stoneCabinValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(1, 25)} {ObjectIndexes.Torch:D} {rng.Next(1, 10)}";
			stoneCabinValues[2] = $"{ObjectIndexes.Stone:D} {rng.Next(5, 25)} {ObjectIndexes.Wood:D} {rng.Next(10, 25)} {ObjectIndexes.Fiber:D} {rng.Next(1, 6)}";
			stoneCabinValues[3] = $"{ObjectIndexes.Stone:D} {rng.Next(1, 25)} {ObjectIndexes.Clay:D} {rng.Next(1, 3)}";

			string[] plankCabinValues = new string[4];
			plankCabinValues[0] = $"{ObjectIndexes.Wood:D} 5 {ObjectIndexes.Fiber:D} 10";
			plankCabinValues[1] = $"{ObjectIndexes.Wood:D} {rng.Next(1, 50)} {ObjectIndexes.PineCone:D} {rng.Next(1, 3)}";
			plankCabinValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(5, 25)} {ObjectIndexes.Stone:D} {rng.Next(1, 10)} {ObjectIndexes.Sap:D} {rng.Next(1, 3)}";
			plankCabinValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(1, 25)} {ObjectIndexes.MixedSeeds:D} {rng.Next(1, 3)}";

			string[] logCabinValues = new string[4];
			logCabinValues[0] = $"{ObjectIndexes.Wood:D} {rng.Next(1, 50)} {ObjectIndexes.Clay:D} {rng.Next(1, 3)}";
			logCabinValues[1] = $"{ObjectIndexes.Wood:D} {rng.Next(5, 25)} {ObjectIndexes.Stone:D} {rng.Next(5, 25)} {ObjectIndexes.Fiber:D} 1";
			logCabinValues[2] = $"{ObjectIndexes.Wood:D} {rng.Next(1, 10)} {ObjectIndexes.Stone:D} {rng.Next(1, 10)}";
			logCabinValues[3] = $"{ObjectIndexes.Wood:D} {rng.Next(10, 50)} {ObjectIndexes.Sap:D} {rng.Next(1, 5)}";

			string[] wellValues = new string[4];
			wellValues[0] = $"{ObjectIndexes.Stone:D} {rng.Next(25, 140)} {ObjectIndexes.Sap:D} 5";
			wellValues[1] = $"{ObjectIndexes.Stone:D} {rng.Next(20, 100)} {ObjectIndexes.IronBar:D} {rng.Next(1, 3)} {ObjectIndexes.BasicRetainingSoil:D} {rng.Next(1, 20)}";
			wellValues[2] = $"{ObjectIndexes.Stone:D} {rng.Next(5, 50)} {ObjectIndexes.Wood:D} {rng.Next(5, 25)} {ObjectIndexes.FrozenTear:D} 1";
			wellValues[3] = $"{ObjectIndexes.Stone:D} {rng.Next(5, 25)} {ObjectIndexes.BasicRetainingSoil:D} {rng.Next(1, 20)}";

			this._blueprintReplacements["Silo"] = $"{siloValues[rng.Next(0, 6)]}/3/3/-1/-1/-2/-1/null/Silo/Allows you to cut and store grass for feed. Market price of 100g/Buildings/none/48/128/-1/null/Farm/{rng.Next(1, 6) * 100}/false";
			this._blueprintReplacements["Mill"] = $"{millValues[rng.Next(0, 4)]}/4/2/-1/-1/-2/-1/null/Mill/Allows you to create flour from wheat and sugar from beets. Market price of 2,500g/Buildings/none/64/128/-1/null/Farm/{rng.Next(10, 61) * 100}/false";
			this._blueprintReplacements["Coop"] = $"{coopValues[rng.Next(0, 6)]}/6/3/1/2/2/2/Coop/Coop/Houses 4 coop-dwelling animals. Market price of 4,000g/Buildings/none/64/96/4/null/Farm/{rng.Next(20, 55) * 100}/false";
			this._blueprintReplacements["Stable"] = $"{stableValues[rng.Next(0, 4)]}/4/2/-1/-1/-2/-1/null/Stable/Allows you to keep and ride a horse. Horse included. Market price of 10,000g/Buildings/none/64/96/-1/null/Farm/{rng.Next(50, 151) * 100}/false";
			this._blueprintReplacements["Shipping Bin"] = $"{shippingBinValues[rng.Next(0, 4)]}/2/1/-1/-1/-1/-1/null/Shipping Bin/Throw items inside to sell them overnight. Market price of 250g/Buildings/none/48/80/-1/null/Farm/{rng.Next(1, 7) * 100}/false/0";
			this._blueprintReplacements["Big Coop"] = $"{bigCoopValues[rng.Next(0, 4)]}/6/3/1/2/2/2/Coop2/Big Coop/Houses 8 coop-dwelling animals. Comes with an incubator. Unlocks ducks. Market price of 10,000g/Upgrades/Coop/64/96/8/null/Farm/{rng.Next(50, 151) * 100}/false";
			this._blueprintReplacements["Deluxe Coop"] = $"{deluxeCoopValues[rng.Next(0, 4)]}/6/3/1/2/2/2/Coop3/Deluxe Coop/Houses 12 coop-dwelling animals. Comes with an auto-feed system. Unlocks rabbits. Market price of 20,000g/Upgrades/Big Coop/64/96/12/null/Farm/{rng.Next(100, 301) * 100}/false";
			this._blueprintReplacements["Barn"] = $"{barnValues[rng.Next(0, 4)]}/7/4/1/3/3/3/Barn/Barn/Houses 4 barn-dwelling animals. Market price of 6,000g/Buildings/none/96/96/4/null/Farm/{rng.Next(3000, 8000)}/false";
			this._blueprintReplacements["Big Barn"] = $"{bigBarnValues[rng.Next(0, 4)]}/7/4/1/3/4/3/Barn2/Big Barn/Houses 8 barn-dwelling animals. Allows animals to give birth. Unlocks goats. Market price of 12,000g/Upgrades/Barn/96/96/8/null/Farm/{rng.Next(80, 151) * 100}/false";
			this._blueprintReplacements["Deluxe Barn"] = $"{deluxeBarnValues[rng.Next(0, 4)]}/7/4/1/3/4/3/Barn3/Deluxe Barn/Houses 12 barn-dwelling animals. Comes with an auto-feed system. Unlocks sheep and pigs. Market price of 25,000g/Upgrades/Big Barn/96/96/12/null/Farm/{rng.Next(180, 301) * 100}/false";
			this._blueprintReplacements["Slime Hutch"] = $"{slimeHutchValues[rng.Next(0, 4)]}/11/6/5/5/-1/-1/SlimeHutch/Slime Hutch/Raise up to 20 slimes. Fill water troughs and slimes will create slime balls. Market price of 10,000g/Buildings/none/96/96/20/null/Farm/{rng.Next(50, 101) * 100}/false";
			this._blueprintReplacements["Shed"] = $"{shedValues[rng.Next(0, 4)]}/7/3/3/2/-1/-1/Shed/Shed/An empty building. Fill it with whatever you like! The interior can be decorated. Market price of 15,000g/Buildings/none/96/96/20/null/Farm/{rng.Next(35, 151) * 100}/false";
			this._blueprintReplacements["Stone Cabin"] = $"{stoneCabinValues[rng.Next(0, 4)]}/5/3/2/2/-1/-1/Cabin/Cabin/A home for a friend! Subsidized by the town agricultural fund./Buildings/none/96/96/20/null/Farm/100/false/0";
			this._blueprintReplacements["Plank Cabin"] = $"{plankCabinValues[rng.Next(0, 4)]}/5/3/2/2/-1/-1/Cabin/Cabin/A home for a friend! Subsidized by the town agricultural fund./Buildings/none/96/96/20/null/Farm/100/false/0";
			this._blueprintReplacements["Log Cabin"] = $"{logCabinValues[rng.Next(0, 4)]}/5/3/2/2/-1/-1/Cabin/Cabin/A home for a friend! Subsidized by the town agricultural fund./Buildings/none/96/96/20/null/Farm/100/false/0";
			this._blueprintReplacements["Well"] = $"{wellValues[rng.Next(0, 4)]}/3/3/-1/-1/-1/-1/null/Well/Provides a place for you to refill your watering can. Market price of 1,000g/Buildings/none/32/32/-1/null/Farm/{rng.Next(2, 18) * 100}/false";
		}

		private void CalculateRecipeEdits()
		{
			_recipeReplacements.Clear();
			foreach (CraftableItem item in ItemList.Items.Values.Where(x => x.IsCraftable))
			{
				_recipeReplacements[item.Name] = item.GetCraftingString(); //TODO: keep an eye out for the Name being wrong at some point
			}
		}

		private void CalculateStringEdits(Random rng)
		{

			this._stringReplacements.Clear();

			string[] Adjective = new string[30];
			Adjective[0] = $"angry"; Adjective[1] = $"arrogrant"; Adjective[2] = $"bored"; Adjective[3] = $"clumsy"; Adjective[4] = $"confused"; Adjective[5] = $"creepy"; Adjective[6] = $"cruel"; Adjective[7] = $"fierce";
			Adjective[8] = $"mysterious"; Adjective[9] = $"adorable"; Adjective[10] = $"handsome"; Adjective[11] = $"confident"; Adjective[12] = $"glamorous"; Adjective[13] = $"kind"; Adjective[14] = $"pretty"; Adjective[15] = $"calm";
			Adjective[16] = $"peaceful"; Adjective[17] = $"tranquil"; Adjective[18] = $"fat"; Adjective[19] = $"gigantic"; Adjective[20] = $"immense"; Adjective[21] = $"miniature"; Adjective[22] = $"gigantic";
			Adjective[23] = $"petite"; Adjective[24] = $"tiny"; Adjective[25] = $"brave"; Adjective[26] = $"charming"; Adjective[27] = $"energetic"; Adjective[28] = $"proud"; Adjective[29] = $"lazy";

			string[] Verb = new string[30];
			Verb[0] = $"bite"; Verb[1] = $"break"; Verb[2] = $"burn"; Verb[3] = $"dig"; Verb[4] = $"dream"; Verb[5] = $"drink"; Verb[6] = $"fight"; Verb[7] = $"freeze";
			Verb[8] = $"hide"; Verb[9] = $"hurt"; Verb[10] = $"lose"; Verb[11] = $"read"; Verb[12] = $"sell"; Verb[13] = $"swim"; Verb[14] = $"throw"; Verb[15] = $"understand";
			Verb[16] = $"write"; Verb[17] = $"lead"; Verb[18] = $"fly"; Verb[19] = $"forget"; Verb[20] = $"dive"; Verb[21] = $"choose"; Verb[22] = $"catch";
			Verb[23] = $"buy"; Verb[24] = $"bend"; Verb[25] = $"stab"; Verb[26] = $"make"; Verb[27] = $"run"; Verb[28] = $"see"; Verb[29] = $"shred";

			string[] PastVerb = new string[20];
			PastVerb[0] = $"beat"; PastVerb[1] = $"broke"; PastVerb[2] = $"burned"; PastVerb[3] = $"cut"; PastVerb[4] = $"dug"; PastVerb[5] = $"dove"; PastVerb[6] = $"dreamed"; PastVerb[7] = $"fell";
			PastVerb[8] = $"fought"; PastVerb[9] = $"froze"; PastVerb[10] = $"grew"; PastVerb[11] = $"hurt"; PastVerb[12] = $"laid"; PastVerb[13] = $"paid"; PastVerb[14] = $"sold"; PastVerb[15] = $"showed";
			PastVerb[16] = $"threw"; PastVerb[17] = $"woke"; PastVerb[18] = $"swam"; PastVerb[19] = $"tore";

			string[] Noun = new string[30];
			Noun[0] = $"oven mitt"; Noun[1] = $"canadian"; Noun[2] = $"dank weed"; Noun[3] = $"american"; Noun[4] = $"concerned ape"; Noun[5] = $"dragon"; Noun[6] = $"cold-hearted eskimo"; Noun[7] = $"doge";
			Noun[8] = $"kappa"; Noun[9] = $"twitch chat"; Noun[10] = $"spaceship"; Noun[11] = $"gift"; Noun[12] = $"cowbell"; Noun[13] = $"shark"; Noun[14] = $"Spiderweb"; Noun[15] = $"canoe";
			Noun[16] = $"cardigan"; Noun[17] = $"tornado"; Noun[18] = $"underwear"; Noun[19] = $"airplane"; Noun[20] = $"toenail"; Noun[21] = $"pathoschild"; Noun[22] = $"mosquito"; Noun[23] = $"missile";
			Noun[24] = $"landmine"; Noun[25] = $"hamburger"; Noun[26] = $"gorilla"; Noun[27] = $"noob"; Noun[28] = $"dinosaur"; Noun[29] = "particle accelerator";

			string farmerNameTemp = "{0}";
			string farmNameTemp = "{1}";


			this._stringReplacements["GrandpaStory.cs.12026"] = $"...and for my very {Adjective[rng.Next(0, 30)]} grandson:";
			this._stringReplacements["GrandpaStory.cs.12028"] = $"...and for my very {Adjective[rng.Next(0, 30)]} granddaughter:";
			this._stringReplacements["GrandpaStory.cs.12029"] = $"I want you to have this {PastVerb[rng.Next(0, 20)]} envelope.";
			this._stringReplacements["GrandpaStory.cs.12030"] = $"No, no, don't {Verb[rng.Next(0, 30)]} it yet... have patience.";
			this._stringReplacements["GrandpaStory.cs.12034"] = $"There will come a day when you feel {PastVerb[rng.Next(0, 20)]} by the burden of modern life...";
			this._stringReplacements["GrandpaStory.cs.12035"] = $"...and your {Adjective[rng.Next(0, 30)]} spirit will fade before a growing emptiness.";
			this._stringReplacements["GrandpaStory.cs.12036"] = $"When that happens, my boy, you'll be ready for this {Noun[rng.Next(0, 30)]}.";
			this._stringReplacements["GrandpaStory.cs.12038"] = $"When that happens, my dear, you'll be ready for this {Noun[rng.Next(0, 30)]}.";
			this._stringReplacements["GrandpaStory.cs.12040"] = $"Now, let Grandpa {Verb[rng.Next(0, 30)]}...";
			this._stringReplacements["GrandpaStory.cs.12051"] = $"Dear {farmerNameTemp},^^If you're reading this, you must be in dire need of a {Noun[rng.Next(0, 30)]}.^^The same thing happened to me, long ago. I'd lost sight of what mattered most in life... {Noun[rng.Next(0, 30)]}s. So I {PastVerb[rng.Next(0, 20)]} everything and moved to the place I truly belong.^^^I've enclosed the deed to that place... my pride and joy: {farmNameTemp} Farm. It's located in Stardew Valley, on the {Adjective[rng.Next(0, 30)]} coast. It's the {Adjective[rng.Next(0, 30)]} place to start your new life.^^This was my most precious gift of all, and now it's yours. I know you'll honor the family name, my boy. Good luck.^^Love, Grandpa^^P.S. If Lewis is still alive say hi to the {Adjective[rng.Next(0, 30)]} guy for me, will ya?";
			this._stringReplacements["GrandpaStory.cs.12055"] = $"Dear {farmerNameTemp},^^If you're reading this, you must be in dire need of a {Noun[rng.Next(0, 30)]}.^^The same thing happened to me, long ago. I'd lost sight of what mattered most in life... {Noun[rng.Next(0, 30)]}s. So I {PastVerb[rng.Next(0, 20)]} everything and moved to the place I truly belong.^^^I've enclosed the deed to that place... my pride and joy: {farmNameTemp} Farm. It's located in Stardew Valley, on the {Adjective[rng.Next(0, 30)]} coast. It's the {Adjective[rng.Next(0, 30)]} place to start your new life.^^This was my most precious gift of all, and now it's yours. I know you'll honor the family name, my boy. Good luck.^^Love, Grandpa^^P.S. If Lewis is still alive say hi to the {Adjective[rng.Next(0, 30)]} guy for me, will ya?";

		}

		private void CalculateFarmEventEdits()
		{
			//TODO: replace this code
			throw new NotImplementedException();
		}

		private void CalculateMailEdits()
		{
			//TODO: replace this code
			throw new NotImplementedException();
		}
	}
}