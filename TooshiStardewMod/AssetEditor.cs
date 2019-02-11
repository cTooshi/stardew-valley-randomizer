using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class AssetEditor : IAssetEditor
	{
		private readonly ModEntry _mod;
		private Dictionary<string, string> _recipeReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _bundleReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _blueprintReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _grandpaStringReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _stringReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _farmEventReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _mailReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _fishReplacements = new Dictionary<int, string>();
		private readonly Dictionary<int, string> _questReplacements = new Dictionary<int, string>();
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
			if (asset.AssetNameEquals("Strings/StringsFromCSFiles")) { return true; }
			if (asset.AssetNameEquals("Data/ObjectInformation")) { return true; }
			if (asset.AssetNameEquals("Data/Events/Farm")) { return true; }
			if (asset.AssetNameEquals("Data/Mail")) { return true; }
			if (asset.AssetNameEquals("Data/Fish")) { return ModEntry.configDict.ContainsKey("fish") ? ModEntry.configDict["fish"] : true; }
			if (asset.AssetNameEquals("Data/Quests")) { return true; }
			if (asset.AssetNameEquals("Data/Locations")) { return ModEntry.configDict.ContainsKey("foragable and fish locations") ? ModEntry.configDict["foragable and fish locations"] : true; ; }
			if (asset.AssetNameEquals("Data/fruitTrees")) { return ModEntry.configDict.ContainsKey("fruit trees") ? ModEntry.configDict["fruit trees"] : true; }
			if (asset.AssetNameEquals("Data/Crops")) { return ModEntry.configDict.ContainsKey("crops prices") ? ModEntry.configDict["crop prices"] : true; }
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
				this.ApplyEdits(asset, this._grandpaStringReplacements);
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
			else if (asset.AssetNameEquals("Data/Quest"))
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
			this._mod.Helper.Content.InvalidateCache("Data/Quest");
			this._mod.Helper.Content.InvalidateCache("Data/Locations");
			this._mod.Helper.Content.InvalidateCache("Data/fruitTrees");
			this._mod.Helper.Content.InvalidateCache("Data/Crops");
		}

		public void CalculateEditsBeforeLoad()
		{
			_grandpaStringReplacements = StringsRandomizer.RandomizeGrandpasStory();
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


			this.CalculateBlueprintEdits();
			//this.CalculateFarmEventEdits();
			//this.CalculateMailEdits();
			this.CalculateQuestEdits();

			_recipeReplacements = CraftingRecipeRandomizer.Randomize();
			_stringReplacements = StringsRandomizer.Randomize();
			_locationsReplacements = LocationRandomizer.Randomize();
			_bundleReplacements = BundleRandomizer.Randomize();
			MusicReplacements = MusicRandomizer.Randomize();
		}

		/// <summary>
		/// Validates that all the items in the ObjectIndexes exist in the main item list
		/// </summary>
		private void ValidateItemList()
		{
			foreach (ObjectIndexes index in Enum.GetValues(typeof(ObjectIndexes)).Cast<ObjectIndexes>())
			{
				if (!ItemList.Items.ContainsKey((int)index))
				{
					Globals.ConsoleWrite($"Missing item: {(int)index}: {index.ToString()}");
				}
			}
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

		private void CalculateQuestEdits()
		{
			this._questReplacements.Clear();
			Random rng = Globals.RNG;

			string[] Quest101Values = new string[4];
			Quest101Values[0] = "ItemDelivery/Jodi's Request/Jodi needs fresh kale for a recipe she's making. She's asking you to bring her one./Bring Jodi kale./Jodi 250/-1/250/-1/true/Oh, that looks so delicious! Thank you, this is just what I wanted. It's going to be perfect for my yellow curry.";
			Quest101Values[1] = "ItemDelivery/Jodi's Request/Hey @,^ I heard you've been exploring the abandoned mines, have you seen any frozen tears? I'd love to have one if you find any! /Bring Jodi a frozen tear./Jodi 84/-1/300/-1/true/Wow, you actually found a frozen tear! This will make a great decorative piece.";
			Quest101Values[2] = "ItemDelivery/Jodi's Request/Hey @,^ Willy told me that you've started fishing, can you catch a halibut for me? I'd like to try a new recipe. /Bring Jodi a halibut./Jodi 708/-1/300/-1/true/Hey the halibut I asked for! I cant wait to make this new dish, here is payment for your trouble.";
			Quest101Values[3] = "ItemDelivery/Jodi's Request/Jodi needs a fresh cauliflower for a recipe she's making. She's asking you to bring her one./Bring Jodi a cauliflower./Jodi 190/-1/350/-1/true/Oh, that looks so delicious! Thank you, this is just what I wanted. It's going to be perfect for my yellow curry.";


			this._questReplacements[101] = Quest101Values[rng.Next(0, 4)];
		}
	}
}