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
		private readonly Dictionary<string, string> _bundleReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _blueprintReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _stringReplacements = new Dictionary<string, string>();
		private readonly Dictionary<int, string> _objectInformationReplacements = new Dictionary<int, string>();
		private readonly Dictionary<string, string> _farmEventReplacements = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _mailReplacements = new Dictionary<string, string>();
		private readonly Dictionary<int, string> _fishReplacements = new Dictionary<int, string>();
		private readonly Dictionary<int, string> _questReplacements = new Dictionary<int, string>();
		private Dictionary<string, string> _locationsReplacements = new Dictionary<string, string>();
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
			if (asset.AssetNameEquals("Data/Locations")) { return true; }
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
			else if (asset.AssetNameEquals("Data/Quest"))
			{
				this.ApplyEdits(asset, this._questReplacements);
			}
			else if (asset.AssetNameEquals("Data/Locations"))
			{
				this.ApplyEdits(asset, this._locationsReplacements);
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
		}

		//Too change before save is loaded/created
		public void CalculateEditsBeforeLoad(Random placeHolderNumber)
		{
			this.CalculateStringEdits(placeHolderNumber);
		}

		public void CalculateEdits()
		{
			//Order matters here because it's using a rng
			this.CalculateRecipeEdits();
			this.CalculateBundleEdits();
			this.CalculateBlueprintEdits();
			this.CalculateObjectInformationEdits();
			//this.CalculateFarmEventEdits();
			//this.CalculateMailEdits();
			this.CalculateFishEdits();
			this.CalculateQuestEdits();
			_locationsReplacements = LocationRandomizer.Randomize();
			MusicReplacements = MusicRandomizer.Randomize();
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

		private void CalculateBundleEdits()
		{
			this._bundleReplacements.Clear();
			Random rng = Globals.RNG;

			string[] Pantry0Values = new string[6];
			Pantry0Values[0] = $"Spring Crops/O {ObjectIndexes.SpeedGro:D} {rng.Next(10, 50)}/{ObjectIndexes.Parsnip:D} {rng.Next(1, 6)} 0 {ObjectIndexes.GreenBean:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Cauliflower:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Potato:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Strawberry:D} {rng.Next(1, 4)} 0/0";
			Pantry0Values[1] = $"Spring Crops/O {ObjectIndexes.SpeedGro:D} {rng.Next(10, 100)}/{ObjectIndexes.Salmonberry:D} {rng.Next(1, 25)} 0 {ObjectIndexes.Kale:D} {rng.Next(1, 6)} 0 {ObjectIndexes.BlueJazz:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Tulip:D} {rng.Next(1, 6)} 0/0";
			Pantry0Values[2] = $"Flowers/O {ObjectIndexes.WarpTotemFarm:D} {rng.Next(10, 20)}/{ObjectIndexes.Tulip:D} {rng.Next(1, 4)} 0 {ObjectIndexes.SummerSpangle:D} {rng.Next(1, 4)} 0 {ObjectIndexes.FairyRose:D} {rng.Next(1, 4)} 0 {ObjectIndexes.BlueJazz:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Sunflower:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Poppy:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Crocus:D} {rng.Next(1, 4)} 0 {ObjectIndexes.SweetPea:D} {rng.Next(1, 4)} 0/4";
			Pantry0Values[3] = $"Spring Crops/O {ObjectIndexes.DeluxeSpeedGro:D} {rng.Next(10, 50)}/{ObjectIndexes.Parsnip:D} {rng.Next(1, 3)} 0 {ObjectIndexes.GreenBean:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Cauliflower:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Potato:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Kale:D} {rng.Next(1, 4)} 0 {ObjectIndexes.CoffeeBean:D} {rng.Next(2, 6)} 0/0";
			Pantry0Values[4] = $"Quality Spring Crops/BO 25 1/{ObjectIndexes.Parsnip:D} {rng.Next(1, 6)} 2 {ObjectIndexes.GreenBean:D} {rng.Next(1, 6)} 2 {ObjectIndexes.Cauliflower:D} {rng.Next(1, 6)} 2 {ObjectIndexes.Potato:D} {rng.Next(1, 6)} 2/3";
			Pantry0Values[5] = $"Mayonnaise Stockpile/O {ObjectIndexes.Truffle:D} {rng.Next(2, 8)}/{ObjectIndexes.Mayonnaise:D} {rng.Next(1, 6)} 0 {ObjectIndexes.DuckMayonnaise:D} {rng.Next(1, 6)} 0 {ObjectIndexes.VoidMayonnaise:D} {rng.Next(1, 6)} 0/6";


			string[] Pantry1Values = new string[6];
			Pantry1Values[0] = $"Summer Crops/O {ObjectIndexes.WarpTotemMountains:D} {rng.Next(5, 15)}/{ObjectIndexes.Tomato:D} {rng.Next(1, 6)} 0 {ObjectIndexes.HotPepper:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Blueberry:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Melon:D} {rng.Next(1, 6)} 0/2";
			Pantry1Values[1] = $"Summer Crops/O {ObjectIndexes.WarpTotemFarm:D} {rng.Next(5, 15)}/{ObjectIndexes.Tomato:D} {rng.Next(1, 6)} 0 {ObjectIndexes.HotPepper:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Blueberry:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Melon:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Wheat:D} {rng.Next(1, 8)} 0 {ObjectIndexes.Radish:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Corn:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Hops:D} {rng.Next(1, 21)} 0/3";
			Pantry1Values[2] = $"Summer Crops/O {ObjectIndexes.WarpTotemBeach:D} {rng.Next(5, 15)}/{ObjectIndexes.Wheat:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Radish:D} {rng.Next(1, 6)} 0 {ObjectIndexes.FiddleheadFern:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Corn:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Hops:D} {rng.Next(1, 21)} 0/3";
			Pantry1Values[3] = $"Eggs Stockpile/BO 24 1/{ObjectIndexes.WhiteEgg:D} {rng.Next(1, 5)} 0 {ObjectIndexes.BrownEgg:D} {rng.Next(1, 6)} 0 {ObjectIndexes.LargeWhiteEgg:D} {rng.Next(1, 3)} 0 {ObjectIndexes.LargeBrownEgg:D} {rng.Next(1, 3)} 0 {ObjectIndexes.VoidEgg:D} 1 0/6";
			Pantry1Values[4] = $"Quality Summer Crops/O {ObjectIndexes.QualitySprinkler:D} {rng.Next(2, 8)}/{ObjectIndexes.Tomato:D} {rng.Next(1, 4)} 2 {ObjectIndexes.HotPepper:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Blueberry:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Melon:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Hops:D} {rng.Next(1, 10)} 2 {ObjectIndexes.Radish:D} {rng.Next(1, 4)} 2/3";
			Pantry1Values[5] = $"Summer Fruits/O {ObjectIndexes.QualitySprinkler:D} {rng.Next(1, 11)}/{ObjectIndexes.Melon:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Tomato:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Blueberry:D} {rng.Next(1, 4)} 0 {ObjectIndexes.HotPepper:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Starfruit:D} 1 0/4";

			string[] Pantry2Values = new string[6];
			Pantry2Values[0] = $"Fall Crops/BO 10 1/{ObjectIndexes.Corn:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Eggplant:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Pumpkin:D} {rng.Next(1, 3)} 0 {ObjectIndexes.BokChoy:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Yam:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Cranberries:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Beet:D} {rng.Next(1, 3)} 0/2/{rng.Next(4, 8)}";
			Pantry2Values[1] = $"Fall Crops/BO 12 1/{ObjectIndexes.Amaranth:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Grape:D} {rng.Next(1, 5)} 0 {ObjectIndexes.SweetGemBerry:D} 1 0 {ObjectIndexes.Yam:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Cranberries:D} {rng.Next(1, 5)} 0/2/{rng.Next(4, 6)}";
			Pantry2Values[2] = $"Fall Quality Crops/O {ObjectIndexes.QualitySprinkler:D} {rng.Next(2, 9)}/{ObjectIndexes.Corn:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Eggplant:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Pumpkin:D} {rng.Next(1, 4)} 2 {ObjectIndexes.BokChoy:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Yam:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Cranberries:D} {rng.Next(1, 4)} 2 {ObjectIndexes.Hops:D} {rng.Next(1, 4)} 2/2/{rng.Next(4, 8)}";
			Pantry2Values[3] = $"Fruit Basket/BO 15 1/{ObjectIndexes.Apple:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Apricot:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Orange:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Peach:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Pomegranate:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Cherry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.CactusFruit:D} {rng.Next(1, 3)} 0/4/{rng.Next(4, 8)}";
			Pantry2Values[4] = $"Pantry Stock/BO 17 1/{ObjectIndexes.Honey:D} {rng.Next(1, 5)} 0 {ObjectIndexes.MapleSyrup:D} {rng.Next(1, 5)} 0 {ObjectIndexes.WheatFlour:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Sugar:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Tortilla:D} {rng.Next(4, 8)} 0 {ObjectIndexes.WhiteEgg:D} {rng.Next(1, 5)} 0 {ObjectIndexes.CoffeeBean:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Vinegar:D} {rng.Next(1, 2)} 0/5/{rng.Next(5, 9)}";
			Pantry2Values[5] = $"Pantry Stock/BO 25 1/{ObjectIndexes.Milk:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Cheese:D} {rng.Next(1, 4)} 0 {ObjectIndexes.WheatFlour:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Rice:D} {rng.Next(1, 7)} 0 {ObjectIndexes.Bread:D} {rng.Next(2, 5)} 0 {ObjectIndexes.CaveCarrot:D} {rng.Next(1, 6)} 0 {ObjectIndexes.CoffeeBean:D} {rng.Next(1, 6)} 0 {ObjectIndexes.BrownEgg:D} {rng.Next(1, 4)} 0/5/{rng.Next(5, 9)}";

			string[] Pantry3Values = new string[6];
			Pantry3Values[0] = $"Mushroom/BO 15 1/{ObjectIndexes.Morel:D} {rng.Next(1, 4)} 0 {ObjectIndexes.CommonMushroom:D} {rng.Next(1, 4)} 0 {ObjectIndexes.RedMushroom:D} {rng.Next(1, 4)} 0 {ObjectIndexes.PurpleMushroom:D} 1 0 {ObjectIndexes.Chanterelle:D} {rng.Next(1, 4)} 0/1";
			Pantry3Values[1] = $"Dairy/BO 10 1/{ObjectIndexes.Milk:D} {rng.Next(2, 6)} 0 {ObjectIndexes.GoatMilk:D} {rng.Next(2, 6)} 0 {ObjectIndexes.LargeMilk:D} 1 0 {ObjectIndexes.LargeGoatMilk:D} 1 0 {ObjectIndexes.Cheese:D} {rng.Next(1, 4)} 0 {ObjectIndexes.GoatCheese:D} {rng.Next(1, 4)} 0/2";
			Pantry3Values[2] = $"Animal/O {ObjectIndexes.Hay:D} {rng.Next(100, 300)}/{ObjectIndexes.Milk:D} 1 0 {ObjectIndexes.GoatMilk:D} 1 0 {ObjectIndexes.Wool:D} 1 0 {ObjectIndexes.DuckEgg:D} 1 0 {ObjectIndexes.BrownEgg:D} {rng.Next(1, 4)} 0 {ObjectIndexes.WhiteEgg:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Truffle:D} 1 0 {ObjectIndexes.DuckEgg:D} 1 0/5/{rng.Next(5, 9)}";
			Pantry3Values[3] = $"Animal/BO 16 1/{ObjectIndexes.Cloth:D} 1 0 {ObjectIndexes.DuckMayonnaise:D} {rng.Next(2, 5)} 0 {ObjectIndexes.LargeMilk:D} 1 0 {ObjectIndexes.LargeGoatMilk:D} 1 0 {ObjectIndexes.Cheese:D} {rng.Next(1, 4)} 0 {ObjectIndexes.GoatCheese:D} {rng.Next(1, 4)} 0/3/6";
			Pantry3Values[4] = $"Fine Foods/BO 141 1/{ObjectIndexes.Wine:D} 1 0 {ObjectIndexes.LargeGoatMilk:D} {rng.Next(2, 5)} 0 {ObjectIndexes.TruffleOil:D} 1 0 {ObjectIndexes.Jelly:D} 1 0 {ObjectIndexes.Cheese:D} {rng.Next(1, 4)} 0 {ObjectIndexes.GoatCheese:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Juice:D} 1 0 {ObjectIndexes.OilOfGarlic:D} 1 0/1/{rng.Next(5, 9)}";
			Pantry3Values[5] = $"Elite/BO 155 1/{ObjectIndexes.Starfruit:D} {rng.Next(1, 5)} 0 {ObjectIndexes.AncientFruit:D} {rng.Next(1, 5)} 0 {ObjectIndexes.SweetGemBerry:D} 1 0 {ObjectIndexes.Wine:D} 1 0/1";

			string[] Pantry4Values = new string[6];
			Pantry4Values[0] = $"Quality Crops/O {ObjectIndexes.QualitySprinkler:D} {rng.Next(2, 6)}/{ObjectIndexes.Parsnip:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Melon:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Corn:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Pumpkin:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Yam:D} {rng.Next(1, 3)} 2/0/{rng.Next(4, 6)}";
			Pantry4Values[1] = $"Quality Crops/O {ObjectIndexes.IridiumSprinkler:D} {rng.Next(1, 3)}/{ObjectIndexes.Potato:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Radish:D} {rng.Next(1, 3)} 2 {ObjectIndexes.BokChoy:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Eggplant:D} {rng.Next(1, 3)} 2 {ObjectIndexes.Cranberries:D} {rng.Next(1, 3)} 2/0/{rng.Next(4, 6)}";
			Pantry4Values[2] = $"Odd Crops/O {ObjectIndexes.LifeElixir:D} {rng.Next(1, 3)}/{ObjectIndexes.Kale:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Amaranth:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Beet:D} {rng.Next(1, 5)} 0 {ObjectIndexes.BokChoy:D} {rng.Next(1, 5)} 0/4";
			Pantry4Values[3] = $"Foragables/BO 10 1/{ObjectIndexes.Salmonberry:D} {rng.Next(1, 10)} 0 {ObjectIndexes.WildHorseradish:D} {rng.Next(1, 3)} 0 {ObjectIndexes.FiddleheadFern:D} {rng.Next(1, 3)} 0 {ObjectIndexes.WildPlum:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SpiceBerry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SnowYam:D} {rng.Next(1, 3)} 0/2/{rng.Next(4, 7)}";
			Pantry4Values[4] = $"Food/O {ObjectIndexes.MuscleRemedy:D} 1/{ObjectIndexes.FriedEgg:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Hashbrowns:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Tortilla:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Bread:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Omelet:D} {rng.Next(1, 3)} 0/3/{rng.Next(4, 6)}";
			Pantry4Values[5] = $"Food/BO 161 1/{ObjectIndexes.Lobster:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Crab:D} {rng.Next(1, 3)} 0 {ObjectIndexes.MapleSyrup:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Apple:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Omelet:D} {rng.Next(1, 3)} 0/3/{rng.Next(4, 6)}";

			string[] Pantry5Values = new string[7];
			Pantry5Values[0] = $"Brew/O {ObjectIndexes.RareSeed:D} {rng.Next(1, 6)}/{ObjectIndexes.Wine:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Beer:D} {rng.Next(1, 5)} 0 {ObjectIndexes.PaleAle:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Mead:D} {rng.Next(1, 3)} 0/1";
			Pantry5Values[1] = $"Red Crops/O {ObjectIndexes.AppleSapling:D} {rng.Next(1, 4)}/{ObjectIndexes.HotPepper:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Strawberry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Tomato:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Cranberries:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Radish:D} {rng.Next(1, 3)} 0 {ObjectIndexes.RedCabbage:D} 1 0 {ObjectIndexes.Beet:D} 1 0 {ObjectIndexes.Rhubarb:D} 1 0/4/4";
			Pantry5Values[2] = $"Green Crops/O {ObjectIndexes.PeachSapling:D} {rng.Next(1, 4)}/{ObjectIndexes.Kale:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Hops:D} {rng.Next(1, 3)} 0 {ObjectIndexes.BokChoy:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Artichoke:D} 1 0 {ObjectIndexes.GreenBean:D} {rng.Next(1, 3)} 0/0/4";
			Pantry5Values[3] = $"Yellow Crops/O {ObjectIndexes.OrangeSapling:D} {rng.Next(1, 4)}/{ObjectIndexes.Parsnip:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Pumpkin:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Yam:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Corn:D} 1 0 {ObjectIndexes.Starfruit:D} 1 0/3/4";
			Pantry5Values[4] = $"Breakfast/O {ObjectIndexes.CompleteBreakfast:D} {rng.Next(5, 11)}/{ObjectIndexes.Omelet:D} 1 0 {ObjectIndexes.FriedEgg:D} 1 0 {ObjectIndexes.Hashbrowns:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Pancakes:D} 1 0 {ObjectIndexes.MapleBar:D} 1 0 {ObjectIndexes.Bread:D} 1 0 {ObjectIndexes.Jelly:D} 1 0 {ObjectIndexes.Coffee:D} 1 0/3/{rng.Next(7, 9)}";
			Pantry5Values[5] = $"Dessert/O {ObjectIndexes.LuckyLunch:D} {rng.Next(1, 5)}/{ObjectIndexes.ChocolateCake:D} 1 0 {ObjectIndexes.IceCream:D} 1 0 {ObjectIndexes.Cookie:D} 1 0 {ObjectIndexes.PumpkinPie:D} 1 0 {ObjectIndexes.BlueberryTart:D} 1 0 {ObjectIndexes.MinersTreat:D} 1 0 {ObjectIndexes.StrangeBun:D} 1 0/5/{rng.Next(5, 8)}";
			Pantry5Values[6] = $"Four Seasons/O {ObjectIndexes.LuckyLunch:D} {rng.Next(1, 5)}/{ObjectIndexes.AutumnsBounty:D} 1 0 {ObjectIndexes.BeanHotpot:D} 1 0 {ObjectIndexes.RedPlate:D} 1 0 {ObjectIndexes.RootsPlatter:D} 1 0/5/4";

			string[] CraftsRoom13Values = new string[6];
			CraftsRoom13Values[0] = $"Spring Foraging/O {ObjectIndexes.SpringSeeds:D} {rng.Next(20, 50)}/{ObjectIndexes.WildHorseradish:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Daffodil:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Leek:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Dandelion:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SpringOnion:D} {rng.Next(1, 3)} 0/0";
			CraftsRoom13Values[1] = $"Quality Spring Foraging/BO 25/{ObjectIndexes.WildHorseradish:D} 1 2 {ObjectIndexes.Daffodil:D} 1 2 {ObjectIndexes.Leek:D} 1 2 {ObjectIndexes.Dandelion:D} 1 2 {ObjectIndexes.SpringOnion:D} 1 2/0";
			CraftsRoom13Values[2] = $"Spring Bulk Foraging/O {ObjectIndexes.Diamond:D} {rng.Next(2, 4)}/{ObjectIndexes.WildHorseradish:D} {rng.Next(3, 5)} 0 {ObjectIndexes.Daffodil:D} {rng.Next(3, 5)} 0 {ObjectIndexes.Leek:D} {rng.Next(3, 5)} 0 {ObjectIndexes.Dandelion:D} {rng.Next(3, 5)} 0 {ObjectIndexes.SpringOnion:D} {rng.Next(3, 5)} 0/0";
			CraftsRoom13Values[3] = $"Pathing/O {ObjectIndexes.QualityFertilizer:D} 30/{ObjectIndexes.WoodPath:D} {rng.Next(10, 30)} 0 {ObjectIndexes.SteppingStonePath:D} {rng.Next(10, 30)} 0 {ObjectIndexes.CrystalPath:D} {rng.Next(10, 30)} 0 {ObjectIndexes.WoodFloor:D} {rng.Next(10, 30)} 0 {ObjectIndexes.CobblestonePath:D} {rng.Next(10, 30)} 0 {ObjectIndexes.GravelPath:D} {rng.Next(10, 30)} 0/0/{rng.Next(4, 7)}";
			CraftsRoom13Values[4] = $"Pathing/O {ObjectIndexes.QualitySprinkler:D} 3/{ObjectIndexes.WoodPath:D} {rng.Next(10, 30)} 0 {ObjectIndexes.StoneFloor:D} {rng.Next(10, 30)} 0 {ObjectIndexes.CrystalFloor:D} {rng.Next(10, 30)} 0 {ObjectIndexes.StrawFloor:D} {rng.Next(10, 30)} 0 {ObjectIndexes.WeatheredFloor:D} {rng.Next(10, 30)} 0 {ObjectIndexes.WoodFloor:D} {rng.Next(10, 30)} 0/0/{rng.Next(4, 7)}";
			CraftsRoom13Values[5] = $"Totem/O {ObjectIndexes.RainTotem:D} {rng.Next(4, 10)}/{ObjectIndexes.WarpTotemFarm:D} 1 0 {ObjectIndexes.WarpTotemBeach:D} 1 0 {ObjectIndexes.WarpTotemMountains:D} 1 0 {ObjectIndexes.RainTotem:D} 1 0/3/3";

			string[] CraftsRoom14Values = new string[6];
			CraftsRoom14Values[0] = $"Summer Foraging/O {ObjectIndexes.SummerSeeds:D} {rng.Next(20, 50)}/{ObjectIndexes.Grape:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SpiceBerry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SweetPea:D} {rng.Next(1, 3)} 0 {ObjectIndexes.FiddleheadFern:D} {rng.Next(1, 3)} 0/3";
			CraftsRoom14Values[1] = $"Berry Basket/O {ObjectIndexes.FruitSalad:D} {rng.Next(2, 6)}/{ObjectIndexes.Salmonberry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Blackberry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Blueberry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Cranberries:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SpiceBerry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Strawberry:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SweetGemBerry:D} {rng.Next(1, 3)} 0/3/{rng.Next(5, 7)}";
			CraftsRoom14Values[2] = $"Quality Summer Foraging/O {ObjectIndexes.SummerSeeds:D} 40/{ObjectIndexes.Grape:D} 1 2 {ObjectIndexes.SweetPea:D} 1 2 {ObjectIndexes.SpiceBerry:D} 1 2/3";
			CraftsRoom14Values[3] = $"Summer Bulk Foraging/O {ObjectIndexes.OmniGeode:D} {rng.Next(10, 20)}/{ObjectIndexes.Grape:D} {rng.Next(3, 6)} 0 {ObjectIndexes.SpiceBerry:D} {rng.Next(3, 6)} 0 {ObjectIndexes.SweetPea:D} {rng.Next(3, 6)} 0 {ObjectIndexes.FiddleheadFern:D} {rng.Next(3, 6)} 0/3";
			CraftsRoom14Values[4] = $"Crop Enhancer/O {ObjectIndexes.QualitySprinkler:D} 5/{ObjectIndexes.BasicFertilizer:D} {rng.Next(5, 20)} 0 {ObjectIndexes.SpeedGro:D} {rng.Next(10, 20)} 0 {ObjectIndexes.BasicRetainingSoil:D} {rng.Next(10, 20)} 0/3";
			CraftsRoom14Values[5] = $"Trinkets/O {ObjectIndexes.OmniGeode:D} {rng.Next(2, 11)}/{ObjectIndexes.CherryBomb:D} {rng.Next(1, 6)} 0 {ObjectIndexes.Spinner:D} 1 0 {ObjectIndexes.FieldSnack:D} {rng.Next(2, 6)} 0/3";

			string[] CraftsRoom15Values = new string[4];
			CraftsRoom15Values[0] = $"Fall Foraging/O {ObjectIndexes.FallSeeds:D} {rng.Next(20, 40)}/{ObjectIndexes.CommonMushroom:D} {rng.Next(1, 3)} 0 {ObjectIndexes.WildPlum:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Hazelnut:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Blackberry:D} {rng.Next(1, 3)} 0/2";
			CraftsRoom15Values[1] = $"Quality Fall Foraging/BO 15 1/{ObjectIndexes.CommonMushroom:D} 1 2 {ObjectIndexes.WildPlum:D} 1 2 {ObjectIndexes.Hazelnut:D} 1 2 {ObjectIndexes.Blackberry:D} 1 2/2";
			CraftsRoom15Values[2] = $"Fall Bulk Foraging/BO 68 1/{ObjectIndexes.CommonMushroom:D} {rng.Next(3, 6)} 0 {ObjectIndexes.WildPlum:D} {rng.Next(3, 6)} 0 {ObjectIndexes.Hazelnut:D} {rng.Next(3, 6)} 0 {ObjectIndexes.Blackberry:D} {rng.Next(3, 6)} 0/2";
			CraftsRoom15Values[3] = $"Wood/BO 15 1/{ObjectIndexes.Wood:D} 99 0 {ObjectIndexes.Hardwood:D} 20 0 {ObjectIndexes.WoodFence:D} 10 0 {ObjectIndexes.WoodFloor:D} 10 0/3";

			string[] CraftsRoom16Values = new string[6];
			CraftsRoom16Values[0] = $"Winter Foraging/O {ObjectIndexes.WinterSeeds:D} {rng.Next(20, 40)}/{ObjectIndexes.WinterRoot:D} {rng.Next(1, 3)} 0 {ObjectIndexes.CrystalFruit:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SnowYam:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Crocus:D} {rng.Next(1, 3)} 0/6";
			CraftsRoom16Values[1] = $"Winter Foraging/O {ObjectIndexes.WinterSeeds:D} {rng.Next(20, 40)}/{ObjectIndexes.WinterRoot:D} 1 0 {ObjectIndexes.CrystalFruit:D} 1 0 {ObjectIndexes.SnowYam:D} 1 0 {ObjectIndexes.Crocus:D} 1 0 {ObjectIndexes.Holly:D} 1 0/6";
			CraftsRoom16Values[2] = $"Quality Winter Foraging/O {ObjectIndexes.WinterSeeds:D} {rng.Next(20, 40)}/{ObjectIndexes.WinterRoot:D} 1 2 {ObjectIndexes.CrystalFruit:D} 1 2 {ObjectIndexes.SnowYam:D} 1 2 {ObjectIndexes.Crocus:D} 1 2/6";
			CraftsRoom16Values[3] = $"Winter Bulk Foraging/BO 19 1/{ObjectIndexes.WinterRoot:D} {rng.Next(3, 5)} 0 {ObjectIndexes.CrystalFruit:D} {rng.Next(3, 5)} 0 {ObjectIndexes.SnowYam:D} {rng.Next(3, 5)} 0 {ObjectIndexes.Crocus:D} {rng.Next(3, 5)} 0/6";
			CraftsRoom16Values[4] = $"Sprinkler/O {ObjectIndexes.AncientSeeds:D} {rng.Next(5, 20)}/{ObjectIndexes.Sprinkler:D} {rng.Next(3, 5)} 0 {ObjectIndexes.QualitySprinkler:D} {rng.Next(2, 5)} 0 {ObjectIndexes.IridiumSprinkler:D} 1 0/6";
			CraftsRoom16Values[5] = $"Geode/R {ObjectIndexes.GlowRing:D} 1/{ObjectIndexes.Geode:D} {rng.Next(1, 5)} 0 {ObjectIndexes.FrozenGeode:D} {rng.Next(1, 5)} 0 {ObjectIndexes.MagmaGeode:D} {rng.Next(1, 5)} 0 {ObjectIndexes.OmniGeode:D} {rng.Next(1, 5)} 0/3";

			string[] CraftsRoom17Values = new string[6];
			CraftsRoom17Values[0] = $"Construction/BO 114 1/{ObjectIndexes.Wood:D} {rng.Next(50, 200)} 0 {ObjectIndexes.Wood:D} {rng.Next(50, 200)} 0 {ObjectIndexes.Stone:D} {rng.Next(50, 200)} 0 {ObjectIndexes.Hardwood:D} {rng.Next(5, 20)} 0/4";
			CraftsRoom17Values[1] = $"Construction/BO 128 1/{ObjectIndexes.Hardwood:D} {rng.Next(10, 20)} 0 {ObjectIndexes.Clay:D} {rng.Next(10, 20)} 0 {ObjectIndexes.WoodFence:D} {rng.Next(20, 50)} 0 {ObjectIndexes.StoneFence:D} {rng.Next(20, 50)} 0/4";
			CraftsRoom17Values[2] = $"Construction/BO 69 1/{ObjectIndexes.HardwoodFence:D} {rng.Next(10, 20)} 0 {ObjectIndexes.Clay:D} {rng.Next(10, 20)} 0 {ObjectIndexes.Wood:D} {rng.Next(50, 150)} 0 {ObjectIndexes.Stone:D} {rng.Next(50, 150)} 0/4"; ;
			CraftsRoom17Values[3] = $"Trash/R {ObjectIndexes.MagnetRing:D} 1/{ObjectIndexes.Trash:D} {rng.Next(1, 3)} 0 {ObjectIndexes.JojaCola:D} 1 0 {ObjectIndexes.Driftwood:D} {rng.Next(2, 5)} 0/3";
			CraftsRoom17Values[4] = $"Reclaimer's/R {ObjectIndexes.MagnetRing:D} 1/{ObjectIndexes.BrokenCD:D} {rng.Next(1, 6)} 0 {ObjectIndexes.BrokenGlasses:D} 1 0 {ObjectIndexes.SoggyNewspaper:D} {rng.Next(2, 6)} 0 {ObjectIndexes.Driftwood:D} {rng.Next(2, 6)} 0 {ObjectIndexes.JojaCola:D} {rng.Next(2, 6)} 0/3";
			CraftsRoom17Values[5] = $"Trash/R {ObjectIndexes.MagnetRing:D} 1/{ObjectIndexes.SoggyNewspaper:D} {rng.Next(2, 5)} 0 {ObjectIndexes.Bait:D} {rng.Next(10, 20)} 0 {ObjectIndexes.BrokenCD:D} 1 0 {ObjectIndexes.BrokenGlasses:D} 1 0/3";

			string[] CraftsRoom19Values = new string[6];
			CraftsRoom19Values[0] = $"Exotic Foraging/O {ObjectIndexes.AutumnsBounty:D} 5/{ObjectIndexes.Coconut:D} 1 0 {ObjectIndexes.CactusFruit:D} 1 0 {ObjectIndexes.CaveCarrot:D} 1 0 {ObjectIndexes.RedMushroom:D} 1 0 {ObjectIndexes.PurpleMushroom:D} 1 0 {ObjectIndexes.MapleSyrup:D} 1 0 {ObjectIndexes.OakResin:D} 1 0 {ObjectIndexes.PineTar:D} 1 0 {ObjectIndexes.Morel:D} 1 0/1/5";
			CraftsRoom19Values[1] = $"Gem Collection/O {ObjectIndexes.IridiumBand:D} 1/{ObjectIndexes.Emerald:D} 1 0 {ObjectIndexes.Topaz:D} 1 0 {ObjectIndexes.Ruby:D} 1 0 {ObjectIndexes.Jade:D} 1 0 {ObjectIndexes.Aquamarine:D} 1 0 {ObjectIndexes.Amethyst:D} 1 0 {ObjectIndexes.Diamond:D} 1 0/1";
			CraftsRoom19Values[2] = $"FAKE NEWS/BO 20 1/{ObjectIndexes.SoggyNewspaper:D} {rng.Next(10, 20)} 0 {ObjectIndexes.SoggyNewspaper:D} {rng.Next(1, 4)} 0 {ObjectIndexes.SoggyNewspaper:D} {rng.Next(1, 3)} 0/1";
			CraftsRoom19Values[3] = $"Exotic Foraging/R {ObjectIndexes.AmethystRing:D} 1/{ObjectIndexes.Coconut:D} 1 0 {ObjectIndexes.CactusFruit:D} 1 0 {ObjectIndexes.CaveCarrot:D} 1 0 {ObjectIndexes.FiddleheadFern:D} 1 0 {ObjectIndexes.Chanterelle:D} 1 0/1";
			CraftsRoom19Values[4] = $"Gross/R {ObjectIndexes.EmeraldRing:D} 1/{ObjectIndexes.Slime:D} 50 0 {ObjectIndexes.Seaweed:D} 1 0 {ObjectIndexes.GreenAlgae:D} 1 0 {ObjectIndexes.WhiteAlgae:D} 1 0 {ObjectIndexes.BugMeat:D} 10 0/1";
			CraftsRoom19Values[5] = $"Tree/BO 55 1/{ObjectIndexes.MapleSyrup:D} {rng.Next(1, 5)} 0 {ObjectIndexes.PineTar:D} {rng.Next(1, 5)} 0 {ObjectIndexes.OakResin:D} {rng.Next(1, 5)} 0 {ObjectIndexes.MapleSeed:D} {rng.Next(1, 5)} 0 {ObjectIndexes.PineCone:D} {rng.Next(1, 5)} 0 {ObjectIndexes.Acorn:D} {rng.Next(1, 5)} 0/1";


			string[] FishTank6Values = new string[5];
			FishTank6Values[0] = $"River Fish/O {ObjectIndexes.Bait:D} {rng.Next(20, 50)}/{ObjectIndexes.Sunfish:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Catfish:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Shad:D} {rng.Next(1, 3)} 0 {ObjectIndexes.TigerTrout:D} {rng.Next(1, 3)} 0/6";
			FishTank6Values[1] = $"Quality River Fish/O {ObjectIndexes.WildBait:D} {rng.Next(15, 35)}/{ObjectIndexes.Sunfish:D} 1 2 {ObjectIndexes.Catfish:D} 1 2 {ObjectIndexes.Shad:D} 1 2 {ObjectIndexes.TigerTrout:D} 1 2/6";
			FishTank6Values[2] = $"Rainy Day Fish/O {ObjectIndexes.CrabPot:D} {rng.Next(1, 4)}/{ObjectIndexes.Walleye:D} 1 0 {ObjectIndexes.Catfish:D} 1 0 {ObjectIndexes.Eel:D} 1 0 {ObjectIndexes.RedSnapper:D} 1 0 {ObjectIndexes.Shad:D} 1 0/1";
			FishTank6Values[3] = $"Quality Rainy Day Fish/O {ObjectIndexes.CrabPot:D} {rng.Next(2, 7)}/{ObjectIndexes.Walleye:D} 1 2 {ObjectIndexes.Catfish:D} 1 2 {ObjectIndexes.Eel:D} 1 2 {ObjectIndexes.RedSnapper:D} 1 2 {ObjectIndexes.Shad:D} 1 2/1";
			FishTank6Values[4] = $"Basic Fish/O {ObjectIndexes.Bait:D} {rng.Next(20, 100)}/{ObjectIndexes.Chub:D} 1 0 {ObjectIndexes.Carp:D} 1 0 {ObjectIndexes.Sardine:D} 1 0 {ObjectIndexes.Bullhead:D} 1 0 {ObjectIndexes.Bream:D} 1 0 {ObjectIndexes.LargemouthBass:D} 1 0 {ObjectIndexes.SmallmouthBass:D} 1 0/1";

			string[] FishTank7Values = new string[5];
			FishTank7Values[0] = $"Lake Fish/O {ObjectIndexes.DressedSpinner:D} 1/{ObjectIndexes.LargemouthBass:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Carp:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Bullhead:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Sturgeon:D} {rng.Next(1, 3)} 0 {ObjectIndexes.SmallmouthBass:D} {rng.Next(1, 3)} 0/0";
			FishTank7Values[1] = $"Quality Lake Fish/O {ObjectIndexes.DressedSpinner:D} 1/{ObjectIndexes.LargemouthBass:D} 1 2 {ObjectIndexes.Carp:D} 1 2 {ObjectIndexes.Bullhead:D} 1 2 {ObjectIndexes.Sturgeon:D} 1 2 {ObjectIndexes.SmallmouthBass:D} 1 2/0";
			FishTank7Values[2] = $"Deep Sea/O {ObjectIndexes.WildBait:D} {rng.Next(20, 100)}/{ObjectIndexes.MidnightSquid:D} 1 0 {ObjectIndexes.SpookFish:D} 1 0 {ObjectIndexes.Blobfish:D} 1 0 {ObjectIndexes.SuperCucumber:D} 1 0 {ObjectIndexes.Pearl:D} 1 0/1/4";
			FishTank7Values[3] = $"Quality Deep Sea/O {ObjectIndexes.WildBait:D} {rng.Next(50, 150)}/{ObjectIndexes.MidnightSquid:D} 1 2 {ObjectIndexes.SpookFish:D} 1 2 {ObjectIndexes.Blobfish:D} 1 2 {ObjectIndexes.SuperCucumber:D} 1 2/1";
			FishTank7Values[4] = $"Tasty Fish/O {ObjectIndexes.DishOTheSea:D} {rng.Next(4, 9)}/{ObjectIndexes.RainbowTrout:D} 1 0 {ObjectIndexes.Walleye:D} 1 0 {ObjectIndexes.Tuna:D} 1 0 {ObjectIndexes.Salmon:D} 1 0 {ObjectIndexes.Halibut:D} 1 0 {ObjectIndexes.Perch:D} 1 0 {ObjectIndexes.RedSnapper:D} 1 0 {ObjectIndexes.TigerTrout:D} 1 0/1/{rng.Next(5, 9)}";


			string[] FishTank8Values = new string[5];
			FishTank8Values[0] = $"Ocean Fish/O {ObjectIndexes.WarpTotemBeach:D} {rng.Next(4, 8)}/{ObjectIndexes.Sardine:D} 1 0 {ObjectIndexes.Tuna:D} 1 0 {ObjectIndexes.RedSnapper:D} 1 0 {ObjectIndexes.Tilapia:D} 1 0 {ObjectIndexes.Albacore:D} 1 0 {ObjectIndexes.Anchovy:D} 1 0 {ObjectIndexes.RedMullet:D} 1 0 {ObjectIndexes.Herring:D} 1 0/5";
			FishTank8Values[1] = $"Quality Ocean Fish/O {ObjectIndexes.WarpTotemBeach:D} {rng.Next(4, 8)}/{ObjectIndexes.Sardine:D} 1 2 {ObjectIndexes.Tuna:D} 1 2 {ObjectIndexes.RedSnapper:D} 1 2 {ObjectIndexes.Tilapia:D} 1 2 {ObjectIndexes.SeaCucumber:D} 1 2 {ObjectIndexes.Anchovy:D} 1 2 {ObjectIndexes.RedMullet:D} 1 2 {ObjectIndexes.Herring:D} 1 2/5/{rng.Next(4, 8)}";
			FishTank8Values[2] = $"Ocean Fish/O {ObjectIndexes.WarpTotemBeach:D} {rng.Next(4, 8)}/{ObjectIndexes.SeaCucumber:D} 1 0 {ObjectIndexes.Squid:D} 1 0 {ObjectIndexes.RedMullet:D} 1 0 {ObjectIndexes.Herring:D} 1 0 {ObjectIndexes.SuperCucumber:D} 1 0 {ObjectIndexes.Octopus:D} 1 0/5";
			FishTank8Values[3] = $"Expert Fishing/O {ObjectIndexes.WarpTotemBeach:D} {rng.Next(10, 20)}/{ObjectIndexes.Octopus:D} 1 0 {ObjectIndexes.SuperCucumber:D} 1 0 {ObjectIndexes.Lingcod:D} 1 0 {ObjectIndexes.ScorpionCarp:D} 1 0 {ObjectIndexes.LavaEel:D} 1 0 {ObjectIndexes.IcePip:D} 1 0/5/{rng.Next(4, 6)}";
			FishTank8Values[4] = $"Hard Fishing/O {ObjectIndexes.WarpTotemBeach:D} {rng.Next(5, 10)}/{ObjectIndexes.Pike:D} 1 0 {ObjectIndexes.Albacore:D} 1 0 {ObjectIndexes.Pufferfish:D} 1 0 {ObjectIndexes.Tuna:D} 1 0 {ObjectIndexes.TigerTrout:D} 1 0 {ObjectIndexes.Stonefish:D} 1 0 {ObjectIndexes.Sandfish:D} 1 0/5/{rng.Next(5, 8)}";


			string[] FishTank9Values = new string[5];
			FishTank9Values[0] = $"Night Fishing/R {ObjectIndexes.SmallGlowRing:D} 1/{ObjectIndexes.Walleye:D} 1 0 {ObjectIndexes.Bream:D} 1 0 {ObjectIndexes.Eel:D} 1 0/1";
			FishTank9Values[1] = $"Large Tackle Box/R {ObjectIndexes.TreasureChest:D} 1/{ObjectIndexes.Spinner:D} 1 0 {ObjectIndexes.DressedSpinner:D} 1 0 {ObjectIndexes.TrapBobber:D} 1 0 {ObjectIndexes.CorkBobber:D} 1 0 {ObjectIndexes.LeadBobber:D} 1 0 {ObjectIndexes.BarbedHook:D} 1 0 {ObjectIndexes.TreasureHunter:D} 1 0 {ObjectIndexes.WildBait:D} {rng.Next(20, 50)} 0/1";
			FishTank9Values[2] = $"Odd Fish/R {ObjectIndexes.GlowRing:D} 1/{ObjectIndexes.MutantCarp:D} 1 0 {ObjectIndexes.Eel:D} 1 0 {ObjectIndexes.Stonefish:D} 1 0 {ObjectIndexes.Octopus:D} 1 0 {ObjectIndexes.Squid:D} 1 0 {ObjectIndexes.Blobfish:D} 1 0/1/{rng.Next(4, 6)}";
			FishTank9Values[3] = $"Quality Night Fishing/R {ObjectIndexes.SmallGlowRing:D} 1/{ObjectIndexes.Walleye:D} 1 0 {ObjectIndexes.Bream:D} 1 0 {ObjectIndexes.Eel:D} 1 0/1";
			FishTank9Values[4] = $"Small Tackle Box/R {ObjectIndexes.MagnetRing:D} 1/{ObjectIndexes.Spinner:D} 1 0 {ObjectIndexes.TrapBobber:D} 1 0 {ObjectIndexes.LeadBobber:D} 1 0 {ObjectIndexes.Bait:D} {rng.Next(20, 50)} 0/1";


			string[] FishTank10Values = new string[5];
			FishTank10Values[0] = $"Specialty Fish/O {ObjectIndexes.DishOTheSea:D} {rng.Next(4, 8)}/{ObjectIndexes.Pufferfish:D} 1 0 {ObjectIndexes.Ghostfish:D} 1 0 {ObjectIndexes.Sandfish:D} 1 0 {ObjectIndexes.Woodskip:D} 1 0 {ObjectIndexes.ScorpionCarp:D} 1 0/4";
			FishTank10Values[1] = $"Mine Fish/O {ObjectIndexes.MinersTreat:D} {rng.Next(4, 8)}/{ObjectIndexes.Ghostfish:D} 1 0 {ObjectIndexes.IcePip:D} 1 0 {ObjectIndexes.LavaEel:D} 1 0 {ObjectIndexes.Stonefish:D} 1 0/4";
			FishTank10Values[2] = $"Legendary Fish/O {ObjectIndexes.FishStew:D} {rng.Next(5, 10)}/{ObjectIndexes.Legend:D} 1 0 {ObjectIndexes.Crimsonfish:D} 1 0 {ObjectIndexes.Angler:D} 1 0 {ObjectIndexes.Glacierfish:D} 1 0/4";
			FishTank10Values[3] = $"Quality Special Fish/O {ObjectIndexes.DishOTheSea:D} {rng.Next(6, 12)}/{ObjectIndexes.Pufferfish:D} 1 2 {ObjectIndexes.Ghostfish:D} 1 2 {ObjectIndexes.Sandfish:D} 1 2 {ObjectIndexes.Woodskip:D} 1 2 {ObjectIndexes.ScorpionCarp:D} 1 2/4";
			FishTank10Values[4] = $"Quality Mine Fish/O {ObjectIndexes.MinersTreat:D} {rng.Next(6, 10)}/{ObjectIndexes.Ghostfish:D} 1 2 {ObjectIndexes.IcePip:D} 1 2 {ObjectIndexes.LavaEel:D} 1 2 {ObjectIndexes.Stonefish:D} 1 2/4";


			string[] FishTank11Values = new string[5];
			FishTank11Values[0] = $"Crab Pot/O {ObjectIndexes.CrabPot:D} {rng.Next(1, 8)}/{ObjectIndexes.Lobster:D} 1 0 {ObjectIndexes.Crayfish:D} 1 0 {ObjectIndexes.Crab:D} 1 0 {ObjectIndexes.Cockle:D} 1 0 {ObjectIndexes.Mussel:D} 1 0 {ObjectIndexes.Shrimp:D} 1 0 {ObjectIndexes.Snail:D} 1 0 {ObjectIndexes.Periwinkle:D} 1 0 {ObjectIndexes.Oyster:D} 1 0 {ObjectIndexes.Clam:D} 1 0/6/{rng.Next(6, 10)}";
			FishTank11Values[1] = $"Quality Crab Pot/O {ObjectIndexes.CrabPot:D} {rng.Next(1, 14)}/{ObjectIndexes.Lobster:D} 1 2 {ObjectIndexes.Crayfish:D} 1 2 {ObjectIndexes.Crab:D} 1 2 {ObjectIndexes.Cockle:D} 1 2 {ObjectIndexes.Mussel:D} 1 2 {ObjectIndexes.Shrimp:D} 1 2 {ObjectIndexes.Snail:D} 1 2 {ObjectIndexes.Periwinkle:D} 1 2 {ObjectIndexes.Oyster:D} 1 2 {ObjectIndexes.Clam:D} 1 2/6/{rng.Next(6, 10)}";
			FishTank11Values[2] = $"Tasty Crab Pot/O {ObjectIndexes.CrabPot:D} {rng.Next(1, 8)}/{ObjectIndexes.Lobster:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Crab:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Mussel:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Shrimp:D} {rng.Next(1, 4)} 0 {ObjectIndexes.Oyster:D} {rng.Next(1, 4)} 0/6/{rng.Next(4, 6)}";
			FishTank11Values[3] = $"Fishing Dish/O {ObjectIndexes.TreasureChest:D} {rng.Next(1, 3)}/{ObjectIndexes.DishOTheSea:D} 1 0 {ObjectIndexes.BakedFish:D} 1 0 {ObjectIndexes.SpicyEel:D} 1 0 {ObjectIndexes.MakiRoll:D} 1 0 {ObjectIndexes.Sashimi:D} 1 0 {ObjectIndexes.CrabCakes:D} 1 0 {ObjectIndexes.Escargot:D} 1 0 {ObjectIndexes.FriedCalamari:D} 1 0/6/6";
			FishTank11Values[4] = $"Fish Soup/O {ObjectIndexes.TreasureChest:D} {rng.Next(1, 3)}/{ObjectIndexes.TroutSoup:D} 1 0 {ObjectIndexes.Chowder:D} 1 0 {ObjectIndexes.FishStew:D} 1 0 {ObjectIndexes.LobsterBisque:D} 1 0 {ObjectIndexes.PaleBroth:D} 1 0 {ObjectIndexes.AlgaeSoup:D} 1 0 {ObjectIndexes.TomKhaSoup:D} 1 0/6/4";


			string[] BoilerRoom20Values = new string[6];
			BoilerRoom20Values[0] = $"Bob-omb/O {ObjectIndexes.OmniGeode:D} {rng.Next(2, 15)}/{ObjectIndexes.CherryBomb:D} {rng.Next(2, 8)} 0 {ObjectIndexes.ExplosiveAmmo:D} {rng.Next(10, 20)} 0 {ObjectIndexes.Bomb:D} {rng.Next(2, 8)} 0 {ObjectIndexes.MegaBomb:D} {rng.Next(2, 8)} 0/1";
			BoilerRoom20Values[1] = $"Geode/O {ObjectIndexes.MegaBomb:D} {rng.Next(3, 18)}/{ObjectIndexes.Geode:D} {rng.Next(1, 8)} 0 {ObjectIndexes.FrozenGeode:D} {rng.Next(1, 8)} 0 {ObjectIndexes.MagmaGeode:D} {rng.Next(1, 8)} 0 {ObjectIndexes.OmniGeode:D} {rng.Next(1, 8)} 0/1";
			BoilerRoom20Values[2] = $"Blacksmith's/BO 13 1/{ObjectIndexes.CopperBar:D} {rng.Next(2, 5)} 0 {ObjectIndexes.IronBar:D} {rng.Next(2, 5)} 0 {ObjectIndexes.GoldBar:D} {rng.Next(2, 5)} 0/1";
			BoilerRoom20Values[3] = $"Blacksmith's Materials/O {ObjectIndexes.FrozenGeode:D} {rng.Next(3, 18)}/{ObjectIndexes.Coal:D} {rng.Next(1, 20)} 0 {ObjectIndexes.CopperOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.IronOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.GoldOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.IridiumOre:D} {rng.Next(1, 10)} 0/1/{rng.Next(4, 6)}";
			BoilerRoom20Values[4] = $"Blacksmith's Materials/O {ObjectIndexes.OmniGeode:D} {rng.Next(3, 18)}/{ObjectIndexes.Coal:D} {rng.Next(1, 20)} 0 {ObjectIndexes.CopperOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.IronOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.GoldOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.GoldOre:D} {rng.Next(1, 40)} 0 {ObjectIndexes.Emerald:D} {rng.Next(1, 2)} 0 {ObjectIndexes.Topaz:D} {rng.Next(1, 2)} 0 {ObjectIndexes.Aquamarine:D} {rng.Next(1, 2)} 0/1/{rng.Next(6, 9)}";
			BoilerRoom20Values[5] = $"Miners Pack/O {ObjectIndexes.MagmaGeode:D} {rng.Next(3, 18)}/{ObjectIndexes.MinersTreat:D} 1 0 {ObjectIndexes.SurvivalBurger:D} 1 0 {ObjectIndexes.RootsPlatter:D} 1 0 {ObjectIndexes.Bomb:D} {rng.Next(1, 8)} 0/1/{rng.Next(3, 5)}";

			string[] BoilerRoom21Values = new string[6];
			BoilerRoom21Values[0] = $"Geologist's/O {ObjectIndexes.OmniGeode:D} {rng.Next(2, 15)}/{ObjectIndexes.Quartz:D} {rng.Next(1, 5)} 0 {ObjectIndexes.EarthCrystal:D} {rng.Next(1, 5)} 0 {ObjectIndexes.FrozenTear:D} {rng.Next(1, 3)} 0 {ObjectIndexes.FireQuartz:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Jade:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Amethyst:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Geode:D} {rng.Next(1, 10)} 0/3";
			BoilerRoom21Values[1] = $"Geologist's Gem/O {ObjectIndexes.OmniGeode:D} {rng.Next(2, 15)}/{ObjectIndexes.Ruby:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Topaz:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Jade:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Emerald:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Aquamarine:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Amethyst:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Diamond:D} {rng.Next(1, 3)} 0/3/{rng.Next(5, 8)}";
			BoilerRoom21Values[2] = $"Geologist's Forage/R {ObjectIndexes.GlowRing:D} 1/{ObjectIndexes.RedMushroom:D} {rng.Next(1, 3)} 0 {ObjectIndexes.PurpleMushroom:D} {rng.Next(1, 3)} 0 {ObjectIndexes.EarthCrystal:D} {rng.Next(1, 3)} 0 {ObjectIndexes.FrozenTear:D} {rng.Next(1, 3)} 0 {ObjectIndexes.Quartz:D} {rng.Next(1, 3)} 0 {ObjectIndexes.FireQuartz:D} {rng.Next(1, 3)} 0/3";
			BoilerRoom21Values[3] = $"Precious Gem/R {ObjectIndexes.MagnetRing:D} 1/{ObjectIndexes.Tigerseye:D} 1 0 {ObjectIndexes.Opal:D} 1 0 {ObjectIndexes.FireOpal:D} 1 0 {ObjectIndexes.LemonStone:D} 1 0 {ObjectIndexes.Malachite:D} 1 0 {ObjectIndexes.Jasper:D} 1 0 {ObjectIndexes.Kyanite:D} 1 0/3/{rng.Next(4, 7)}";
			BoilerRoom21Values[4] = $"Precious Mineral/O {ObjectIndexes.IridiumBar:D} {rng.Next(1, 5)}/{ObjectIndexes.Alamite:D} 1 0 {ObjectIndexes.Aerinite:D} 1 0 {ObjectIndexes.Dolomite:D} 1 0 {ObjectIndexes.Geminite:D} 1 0 {ObjectIndexes.OceanStone:D} 1 0 {ObjectIndexes.GhostCrystal:D} 1 0/3/{rng.Next(3, 6)}";
			BoilerRoom21Values[5] = $"Precious Mineral/O {ObjectIndexes.IridiumBar:D} {rng.Next(1, 5)}/{ObjectIndexes.Calcite:D} 1 0 {ObjectIndexes.StarShards:D} 1 0 {ObjectIndexes.FairyStone:D} 1 0 {ObjectIndexes.Neptunite:D} 1 0 {ObjectIndexes.Helvite:D} 1 0 {ObjectIndexes.Fluorapatite:D} 1 0 {ObjectIndexes.Bixite:D} 1 0/3/{rng.Next(4, 7)}";

			string[] BoilerRoom22Values = new string[6];
			BoilerRoom22Values[0] = $"Adventurer's/R {ObjectIndexes.SavageRing:D} 1/{ObjectIndexes.BugMeat:D} {rng.Next(5, 20)} 0 {ObjectIndexes.Slime:D} {rng.Next(50, 99)} 0 {ObjectIndexes.BatWing:D} {rng.Next(5, 20)} 0 {ObjectIndexes.VoidEssence:D} {rng.Next(1, 5)} 0 {ObjectIndexes.SolarEssence:D} {rng.Next(1, 5)} 0/4";
			BoilerRoom22Values[1] = $"Old Junk/R {ObjectIndexes.RingofYoba:D} 1/{ObjectIndexes.ChippedAmphora:D} 1 0 {ObjectIndexes.ChickenStatue:D} 1 0 {ObjectIndexes.ChewingStick:D} 1 0 {ObjectIndexes.GlassShards:D} 1 0 {ObjectIndexes.RustySpoon:D} 1 0 {ObjectIndexes.RustyCog:D} 1 0 {ObjectIndexes.RustySpur:D} 1 0/4/{rng.Next(3, 6)}";
			BoilerRoom22Values[2] = $"Dwarf/O {ObjectIndexes.DwarvishTranslationGuide:D} 1/{ObjectIndexes.DwarfScrollI:D} 1 0 {ObjectIndexes.DwarfScrollII:D} 1 0 {ObjectIndexes.DwarfScrollIII:D} 1 0 {ObjectIndexes.DwarfScrollIV:D} 1 0 {ObjectIndexes.DwarvishHelm:D} 1 0 {ObjectIndexes.DwarfGadget:D} 1 0/4/{rng.Next(2, 5)}";
			BoilerRoom22Values[3] = $"Fossil/R {ObjectIndexes.WarriorRing:D} 1/{ObjectIndexes.PrehistoricScapula:D} 1 0 {ObjectIndexes.PrehistoricTibia:D} 1 0 {ObjectIndexes.PrehistoricSkull:D} 1 0 {ObjectIndexes.SkeletalHand:D} 1 0 {ObjectIndexes.PrehistoricRib:D} 1 0 {ObjectIndexes.PrehistoricVertebra:D} 1 0 {ObjectIndexes.SkeletalTail:D} 1 0 {ObjectIndexes.AmphibianFossil:D} 1 0 {ObjectIndexes.PalmFossil:D} 1 0 {ObjectIndexes.Trilobite:D} 1 0/1/{rng.Next(4, 7)}";
			BoilerRoom22Values[4] = $"Stoneworking/O {ObjectIndexes.GoldBar:D} {rng.Next(2, 7)}/{ObjectIndexes.Marble:D} 1 0 {ObjectIndexes.Sandstone:D} 1 0 {ObjectIndexes.Granite:D} 1 0 {ObjectIndexes.Basalt:D} 1 0 {ObjectIndexes.Limestone:D} 1 0 {ObjectIndexes.Soapstone:D} 1 0 {ObjectIndexes.Mudstone:D} 1 0 {ObjectIndexes.Slate:D} 1 0/4/{rng.Next(5, 8)}";
			BoilerRoom22Values[5] = $"Ancient Artifact/R {ObjectIndexes.TopazRing:D} 1/{ObjectIndexes.AncientDoll:D} 1 0 {ObjectIndexes.AncientDrum:D} 1 0 {ObjectIndexes.Arrowhead:D} 1 0 {ObjectIndexes.AncientSword:D} 1 0 {ObjectIndexes.PrehistoricHandaxe:D} 1 0 {ObjectIndexes.PrehistoricTool:D} 1 0/4/{rng.Next(2, 5)}";

			string[] BulletinBoard31Values = new string[14];
			BulletinBoard31Values[0] = $"Alex's/O {ObjectIndexes.MagmaGeode:D} {rng.Next(3, 15)}/{ObjectIndexes.SalmonDinner:D} 1 0 {ObjectIndexes.CompleteBreakfast:D} 1 0 {ObjectIndexes.BrownEgg:D} 1 0 {ObjectIndexes.WhiteEgg:D} 1 0/4/3";
			BulletinBoard31Values[1] = $"Elliot's/O {ObjectIndexes.StarShards:D} {rng.Next(1, 4)}/{ObjectIndexes.CrabCakes:D} 1 0 {ObjectIndexes.DuckFeather:D} 1 0 {ObjectIndexes.Lobster:D} 1 0 {ObjectIndexes.Pomegranate:D} 1 0 {ObjectIndexes.TomKhaSoup:D} 1 0/4/4";
			BulletinBoard31Values[2] = $"Harvey's/O {ObjectIndexes.SpicyEel:D} {rng.Next(2, 8)}/{ObjectIndexes.Coffee:D} 1 0 {ObjectIndexes.Pickles:D} 1 0 {ObjectIndexes.SuperMeal:D} 1 0 {ObjectIndexes.TruffleOil:D} 1 0 {ObjectIndexes.Wine:D} 1 0/4/4";
			BulletinBoard31Values[3] = $"Sam's/O {ObjectIndexes.DrumBlock:D} {rng.Next(2, 10)}/{ObjectIndexes.CactusFruit:D} 1 0 {ObjectIndexes.MapleBar:D} 1 0 {ObjectIndexes.Pizza:D} 1 0 {ObjectIndexes.Tigerseye:D} 1 0/4/3";
			BulletinBoard31Values[4] = $"Sebastian's/O {ObjectIndexes.FrozenGeode:D} {rng.Next(3, 10)}/{ObjectIndexes.FrozenTear:D} 1 0 {ObjectIndexes.Obsidian:D} 1 0 {ObjectIndexes.PumpkinSoup:D} 1 0 {ObjectIndexes.Sashimi:D} 1 0 {ObjectIndexes.VoidEgg:D} 1 0/4/4";
			BulletinBoard31Values[5] = $"Shane's/O {ObjectIndexes.StrangeBun:D} {rng.Next(3, 10)}/{ObjectIndexes.Beer:D} 1 0 {ObjectIndexes.HotPepper:D} 1 0 {ObjectIndexes.PepperPoppers:D} 1 0 {ObjectIndexes.Pizza:D} 1 0/1";
			BulletinBoard31Values[6] = $"Abigail's/BO 159 1/{ObjectIndexes.Amethyst:D} 1 0 {ObjectIndexes.BlackberryCobbler:D} 1 0 {ObjectIndexes.ChocolateCake:D} 1 0 {ObjectIndexes.Pufferfish:D} 1 0 {ObjectIndexes.Pumpkin:D} 1 0 {ObjectIndexes.SpicyEel:D} 1 0/4/{rng.Next(4, 6)}";
			BulletinBoard31Values[7] = $"Soup of the Day/O {ObjectIndexes.MagmaGeode:D} {rng.Next(3, 15)}/{ObjectIndexes.AlgaeSoup:D} 1 0 {ObjectIndexes.PaleBroth:D} 1 0 {ObjectIndexes.ParsnipSoup:D} 1 0 {ObjectIndexes.TomKhaSoup:D} 1 0 {ObjectIndexes.TroutSoup:D} 1 0 {ObjectIndexes.Chowder:D} 1 0 {ObjectIndexes.LobsterBisque:D} 1 0/4/{rng.Next(4, 7)}";
			BulletinBoard31Values[8] = $"Gus's/O {ObjectIndexes.SalmonDinner:D} {rng.Next(2, 10)}/{ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.Escargot:D} 1 0 {ObjectIndexes.FishTaco:D} 1 0 {ObjectIndexes.Orange:D} 1 0/1/3";
			BulletinBoard31Values[9] = $"Leah's/BO 162 1/{ObjectIndexes.Salad:D} 1 0 {ObjectIndexes.Hardwood:D} 10 0 {ObjectIndexes.StirFry:D} 1 0 {ObjectIndexes.GoatCheese:D} 1 0 {ObjectIndexes.Wine:D} 1 0 {ObjectIndexes.PoppyseedMuffin:D} 1 0 {ObjectIndexes.Truffle:D} 1 0 {ObjectIndexes.VegetableMedley:D} 1 0/4/{rng.Next(5, 7)}";
			BulletinBoard31Values[10] = $"Penny's/BO 107 1/{ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.Emerald:D} 1 0 {ObjectIndexes.Melon:D} 1 0 {ObjectIndexes.Poppy:D} 1 0 {ObjectIndexes.PoppyseedMuffin:D} 1 0 {ObjectIndexes.RedPlate:D} 1 0 {ObjectIndexes.RootsPlatter:D} 1 0 {ObjectIndexes.Sandfish:D} 1 0 {ObjectIndexes.TomKhaSoup:D} 1 0/4/6";
			BulletinBoard31Values[11] = $"Linus's/O {ObjectIndexes.WildBait:D} {rng.Next(10, 100)}/{ObjectIndexes.BlueberryTart:D} 1 0 {ObjectIndexes.CactusFruit:D} 1 0 {ObjectIndexes.Coconut:D} 1 0 {ObjectIndexes.DishOTheSea:D} 1 0 {ObjectIndexes.Yam:D} 1 0/4/{rng.Next(4, 6)}";
			BulletinBoard31Values[12] = $"Dwarf's/O {ObjectIndexes.MegaBomb:D} {rng.Next(1, 20)}/{ObjectIndexes.Amethyst:D} 1 0 {ObjectIndexes.Aquamarine:D} 1 0 {ObjectIndexes.Emerald:D} 1 0 {ObjectIndexes.Jade:D} 1 0 {ObjectIndexes.OmniGeode:D} 1 0 {ObjectIndexes.Ruby:D} 1 0 {ObjectIndexes.Topaz:D} 1 0/4/{rng.Next(5, 8)}";
			BulletinBoard31Values[13] = $"Red/O {ObjectIndexes.MagmaGeode:D} {rng.Next(1, 20)}/{ObjectIndexes.RedSnapper:D} 1 0 {ObjectIndexes.Jelly:D} 1 0 {ObjectIndexes.DwarfScrollI:D} 1 0 {ObjectIndexes.Apple:D} 1 0 {ObjectIndexes.Ruby:D} 1 0/4";

			string[] BulletinBoard32Values = new string[6];
			BulletinBoard32Values[0] = $"Emily's/O {ObjectIndexes.Diamond:D} {rng.Next(1, 3)}/{ObjectIndexes.Amethyst:D} 1 0 {ObjectIndexes.Aquamarine:D} 1 0 {ObjectIndexes.Cloth:D} 1 0 {ObjectIndexes.Emerald:D} 1 0 {ObjectIndexes.Jade:D} 1 0 {ObjectIndexes.Ruby:D} 1 0 {ObjectIndexes.SurvivalBurger:D} 1 0 {ObjectIndexes.Topaz:D} 1 0 {ObjectIndexes.Wool:D} 1 0/5/{rng.Next(6, 9)}";
			BulletinBoard32Values[1] = $"Jodi's/BO 64 1/{ObjectIndexes.ChocolateCake:D} 1 0 {ObjectIndexes.CrispyBass:D} 1 0 {ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.EggplantParmesan:D} 1 0 {ObjectIndexes.FriedEel:D} 1 0 {ObjectIndexes.Pancakes:D} 1 0 {ObjectIndexes.RhubarbPie:D} 1 0 {ObjectIndexes.VegetableMedley:D} 1 0/5/4";
			BulletinBoard32Values[2] = $"Haley's/O {ObjectIndexes.TeaSet:D} 1/{ObjectIndexes.Coconut:D} 1 0 {ObjectIndexes.FruitSalad:D} 1 0 {ObjectIndexes.PinkCake:D} 1 0 {ObjectIndexes.Sunflower:D} 1 0/5/3";
			BulletinBoard32Values[3] = $"Blue/O {ObjectIndexes.FrozenGeode:D} {rng.Next(1, 20)}/{ObjectIndexes.DwarfScrollIII:D} 1 0 {ObjectIndexes.Bream:D} 1 0 {ObjectIndexes.CrystalFruit:D} 1 0 {ObjectIndexes.Blueberry:D} 1 0 {ObjectIndexes.Kyanite:D} 1 0/5";
			BulletinBoard32Values[4] = $"Green/O {ObjectIndexes.StrangeDoll1:D} 1/{ObjectIndexes.Pickles:D} 1 0 {ObjectIndexes.DwarfScrollII:D} 1 0 {ObjectIndexes.OakResin:D} 1 0 {ObjectIndexes.GreenBean:D} 1 0 {ObjectIndexes.LargemouthBass:D} 1 0 {ObjectIndexes.Kale:D} 1 0 {ObjectIndexes.GreenAlgae:D} 1 0/0/6";
			BulletinBoard32Values[5] = $"Marriage/O {ObjectIndexes.WeddingRing:D} 1/{ObjectIndexes.CompleteBreakfast:D} 1 0 {ObjectIndexes.Lobster:D} 1 0 {ObjectIndexes.Sashimi:D} 1 0 {ObjectIndexes.Tigerseye:D} 1 0 {ObjectIndexes.Pickles:D} 1 0 {ObjectIndexes.HotPepper:D} 1 0 {ObjectIndexes.Melon:D} 1 0 {ObjectIndexes.Pufferfish:D} 1 0 {ObjectIndexes.Strawberry:D} 1 0 {ObjectIndexes.Cloth:D} 1 0 {ObjectIndexes.GoatCheese:D} 1 0 {ObjectIndexes.Diamond:D} 1 0/6/{rng.Next(8, 11)}";

			string[] BulletinBoard33Values = new string[7];
			BulletinBoard33Values[0] = $"Demetrius's/O {ObjectIndexes.PurpleMushroom:D} {rng.Next(2, 6)}/{ObjectIndexes.BeanHotpot:D} 1 0 {ObjectIndexes.IceCream:D} 1 0 {ObjectIndexes.RicePudding:D} 1 0 {ObjectIndexes.Strawberry:D} 1 0/6";
			BulletinBoard33Values[1] = $"Sebastian's/O {ObjectIndexes.FrozenGeode:D} {rng.Next(3, 11)}/{ObjectIndexes.FrozenTear:D} 1 0 {ObjectIndexes.Obsidian:D} 1 0 {ObjectIndexes.PumpkinSoup:D} 1 0 {ObjectIndexes.Sashimi:D} 1 0 {ObjectIndexes.VoidEgg:D} 1 0/6/4";
			BulletinBoard33Values[2] = $"Robin's/O {ObjectIndexes.Hardwood:D} {rng.Next(10, 75)}/{ObjectIndexes.GoatCheese:D} 3 0 {ObjectIndexes.Peach:D} 3 0 {ObjectIndexes.Spaghetti:D} 3 0/6";
			BulletinBoard33Values[3] = $"Maru's/BO 165 1/{ObjectIndexes.Battery:D} 1 0 {ObjectIndexes.Cauliflower:D} 1 0 {ObjectIndexes.CheeseCauliflower:D} 1 0 {ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.GoldBar:D} 1 0 {ObjectIndexes.IridiumBar:D} 1 0 {ObjectIndexes.MinersTreat:D} 1 0 {ObjectIndexes.PepperPoppers:D} 1 0 {ObjectIndexes.RhubarbPie:D} 1 0 {ObjectIndexes.Strawberry:D} 1 0/6/{rng.Next(7, 10)}";
			BulletinBoard33Values[4] = $"Pink/BO 161 1/{ObjectIndexes.PinkCake:D} 1 0 {ObjectIndexes.Peach:D} 1 0 {ObjectIndexes.Melon:D} 1 0 {ObjectIndexes.FairyRose:D} 1 0 {ObjectIndexes.Salmonberry:D} {rng.Next(1, 11)} 0/6/4";
			BulletinBoard33Values[5] = $"Orange/BO 161 1/{ObjectIndexes.OrangeSapling:D} 1 0 {ObjectIndexes.QualityFertilizer:D} 20 0 {ObjectIndexes.Pumpkin:D} 1 0 {ObjectIndexes.MapleSyrup:D} 1 0 {ObjectIndexes.NautilusShell:D} 1 0 {ObjectIndexes.Poppy:D} 1 0/2/4";
			BulletinBoard33Values[6] = $"Ice/BO 42 1/{ObjectIndexes.IceCream:D} 1 0 {ObjectIndexes.FrozenTear:D} 1 0 {ObjectIndexes.IcePip:D} 1 0 {ObjectIndexes.FrozenGeode:D} {rng.Next(1, 4)} 0 {ObjectIndexes.SnowYam:D} {rng.Next(1, 4)} 0/6";

			string[] BulletinBoard34Values = new string[6];
			BulletinBoard34Values[0] = $"Marnie's/O {ObjectIndexes.DinosaurEgg:D} 1/{ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.FarmersLunch:D} 1 0 {ObjectIndexes.PinkCake:D} 1 0 {ObjectIndexes.PumpkinPie:D} 1 0/3/3";
			BulletinBoard34Values[1] = $"Pam's/BO 12 1/{ObjectIndexes.Beer:D} 1 0 {ObjectIndexes.CactusFruit:D} 1 0 {ObjectIndexes.GlazedYams:D} 1 0 {ObjectIndexes.Mead:D} 1 0 {ObjectIndexes.PaleAle:D} 1 0 {ObjectIndexes.Parsnip:D} 1 0 {ObjectIndexes.ParsnipSoup:D} 1 0/3/{rng.Next(5, 7)}";
			BulletinBoard34Values[2] = $"Lewis's/BO 25 1/{ObjectIndexes.AutumnsBounty:D} 1 0 {ObjectIndexes.GlazedYams:D} 1 0 {ObjectIndexes.HotPepper:D} 1 0 {ObjectIndexes.VegetableMedley:D} 1 0/3/3";
			BulletinBoard34Values[3] = $"Clint's/O {ObjectIndexes.IridiumSprinkler:D} 1/{ObjectIndexes.Amethyst:D} 1 0 {ObjectIndexes.Aquamarine:D} 1 0 {ObjectIndexes.Emerald:D} 1 0 {ObjectIndexes.FiddleheadRisotto:D} 1 0 {ObjectIndexes.GoldBar:D} 1 0 {ObjectIndexes.IridiumBar:D} 1 0 {ObjectIndexes.Jade:D} 1 0 {ObjectIndexes.OmniGeode:D} 1 0 {ObjectIndexes.Ruby:D} 1 0 {ObjectIndexes.Topaz:D} 1 0/3/{rng.Next(7, 10)}";
			BulletinBoard34Values[4] = $"Yellow/O {ObjectIndexes.StrangeDoll2:D} 1/{ObjectIndexes.DwarfScrollIV:D} 1 0 {ObjectIndexes.Starfruit:D} 1 0 {ObjectIndexes.SolarEssence:D} 1 0 {ObjectIndexes.Honey:D} 1 0 {ObjectIndexes.Cheese:D} 1 0/3";
			BulletinBoard34Values[5] = $"Fire/O {ObjectIndexes.Coal:D} {rng.Next(5, 50)}/{ObjectIndexes.LavaEel:D} 1 0 {ObjectIndexes.FireQuartz:D} 1 0 {ObjectIndexes.Torch:D} {rng.Next(1, 6)} 0 {ObjectIndexes.HotPepper:D} 1 0 {ObjectIndexes.MagmaGeode:D} {rng.Next(1, 4)} 0 {ObjectIndexes.FireOpal:D} 1 0/4";

			string[] BulletinBoard35Values = new string[7];
			BulletinBoard35Values[0] = $"Wizard's/O {ObjectIndexes.VoidEgg:D} 1/{ObjectIndexes.PurpleMushroom:D} 1 0 {ObjectIndexes.SolarEssence:D} 1 0 {ObjectIndexes.SuperCucumber:D} 1 0 {ObjectIndexes.VoidEssence:D} 1 0/1";
			BulletinBoard35Values[1] = $"Willy's/O {ObjectIndexes.Legend:D} 1/{ObjectIndexes.Catfish:D} 1 0 {ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.Mead:D} 1 0 {ObjectIndexes.Octopus:D} 1 0 {ObjectIndexes.Pumpkin:D} 1 0 {ObjectIndexes.SeaCucumber:D} 1 0 {ObjectIndexes.Sturgeon:D} 1 0/1/{rng.Next(5, 8)}";
			BulletinBoard35Values[2] = $"Evelyn's/BO 62 {rng.Next(1, 10)}/{ObjectIndexes.Beet:D} 1 0 {ObjectIndexes.ChocolateCake:D} 1 0 {ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.FairyRose:D} 1 0 {ObjectIndexes.Stuffing:D} 1 0 {ObjectIndexes.Tulip:D} 1 0/1/{rng.Next(4, 6)}";
			BulletinBoard35Values[3] = $"Krobus's/BO 96 1/{ObjectIndexes.Diamond:D} 1 0 {ObjectIndexes.IridiumBar:D} 1 0 {ObjectIndexes.Pumpkin:D} 1 0 {ObjectIndexes.VoidEgg:D} 1 0 {ObjectIndexes.VoidMayonnaise:D} 1 0 {ObjectIndexes.WildHorseradish:D} 1 0/1/{rng.Next(5, 7)}";
			BulletinBoard35Values[4] = $"Purple/O {ObjectIndexes.PomegranateSapling:D} 1/{ObjectIndexes.IridiumBar:D} 1 0 {ObjectIndexes.SuperCucumber:D} 1 0 {ObjectIndexes.PurpleMushroom:D} 1 0 {ObjectIndexes.WildPlum:D} 1 0 {ObjectIndexes.Wine:D} 1 0/1";
			BulletinBoard35Values[5] = $"Earth/BO 20 1/{ObjectIndexes.EarthCrystal:D} 1 0 {ObjectIndexes.Clay:D} {rng.Next(5, 10)} 0 {ObjectIndexes.WinterRoot:D} 1 0 {ObjectIndexes.CaveCarrot:D} 1 0 {ObjectIndexes.Driftwood:D} 1 0 {ObjectIndexes.Quartz:D} 1 0/2/{rng.Next(5, 7)}";
			BulletinBoard35Values[6] = $"Seed/O {ObjectIndexes.RareSeed:D} {rng.Next(1, 10)}/{ObjectIndexes.AncientSeed:D} 1 0 {ObjectIndexes.MixedSeeds:D} {rng.Next(5, 10)} 0 {ObjectIndexes.SpringSeeds:D} {rng.Next(5, 10)} 0 {ObjectIndexes.SummerSeeds:D} {rng.Next(5, 10)} 0 {ObjectIndexes.FallSeeds:D} {rng.Next(5, 10)} 0 {ObjectIndexes.WinterSeeds:D} {rng.Next(5, 10)} 0/2/{rng.Next(5, 7)}";

			string[] VaultValues = new string[14];
			VaultValues[0] = $"1,000g/O {ObjectIndexes.Sprinkler:D} {rng.Next(1, 5)}/-1 1000 1000/4";
			VaultValues[1] = $"2,000g/O {ObjectIndexes.QualitySprinkler:D} {rng.Next(1, 5)}/-1 2000 2000/4";
			VaultValues[2] = $"2,500g/BO 24 1/-1 2500 2500/4";
			VaultValues[3] = $"5,000g/BO 10 1/-1 5000 5000/4";
			VaultValues[4] = $"3,500g/O {ObjectIndexes.QualityFertilizer:D} {rng.Next(5, 40)}/-1 3500 3500/2";
			VaultValues[5] = $"5,000g/BO 15 1/-1 5000 5000/2";
			VaultValues[6] = $"7,500g/BO 17 1/-1 7500 7500/2";
			VaultValues[7] = $"8,500g/BO 9 1/-1 8500 8500/2";
			VaultValues[8] = $"10,000g/BO 12 1/-1 10000 10000/3";
			VaultValues[9] = $"12,500g/O {ObjectIndexes.IridiumSprinkler:D} 1/-1 12500 12500/3";
			VaultValues[10] = $"15,000g/BO 19 1/-1 15000 15000/3";
			VaultValues[11] = $"22,500g/BO 25 1/-1 22500 22500/1";
			VaultValues[12] = $"28,500g/O {ObjectIndexes.IridiumSprinkler:D} {rng.Next(1, 5)}/-1 28500 28500/1";
			VaultValues[13] = $"35,000g/BO 21 1/-1 35000 35000/1";
			VaultValues[13] = $"37,500g/BO 165 1/-1 37500 37500/1";


			this._bundleReplacements["Pantry/0"] = Pantry0Values[rng.Next(0, 6)];
			this._bundleReplacements["Pantry/1"] = Pantry1Values[rng.Next(0, 6)];
			this._bundleReplacements["Pantry/2"] = Pantry2Values[rng.Next(0, 6)];
			this._bundleReplacements["Pantry/3"] = Pantry3Values[rng.Next(0, 6)];
			this._bundleReplacements["Pantry/4"] = Pantry4Values[rng.Next(0, 6)];
			this._bundleReplacements["Pantry/5"] = Pantry5Values[rng.Next(0, 7)];

			this._bundleReplacements["Crafts Room/13"] = CraftsRoom13Values[rng.Next(0, 6)];
			this._bundleReplacements["Crafts Room/14"] = CraftsRoom14Values[rng.Next(0, 6)];
			this._bundleReplacements["Crafts Room/15"] = CraftsRoom15Values[rng.Next(0, 4)];
			this._bundleReplacements["Crafts Room/16"] = CraftsRoom16Values[rng.Next(0, 6)];
			this._bundleReplacements["Crafts Room/17"] = CraftsRoom17Values[rng.Next(0, 6)];
			this._bundleReplacements["Crafts Room/19"] = CraftsRoom19Values[rng.Next(0, 6)];

			this._bundleReplacements["Fish Tank/6"] = FishTank6Values[rng.Next(0, 5)];
			this._bundleReplacements["Fish Tank/7"] = FishTank7Values[rng.Next(0, 5)];
			this._bundleReplacements["Fish Tank/8"] = FishTank8Values[rng.Next(0, 5)];
			this._bundleReplacements["Fish Tank/9"] = FishTank9Values[rng.Next(0, 5)];
			this._bundleReplacements["Fish Tank/10"] = FishTank10Values[rng.Next(0, 5)];
			this._bundleReplacements["Fish Tank/11"] = FishTank11Values[rng.Next(0, 5)];

			this._bundleReplacements["Boiler Room/20"] = BoilerRoom20Values[rng.Next(0, 6)];
			this._bundleReplacements["Boiler Room/21"] = BoilerRoom21Values[rng.Next(0, 6)];
			this._bundleReplacements["Boiler Room/22"] = BoilerRoom22Values[rng.Next(0, 6)];

			this._bundleReplacements["Vault/23"] = VaultValues[rng.Next(0, 4)];
			this._bundleReplacements["Vault/24"] = VaultValues[rng.Next(4, 7)];
			this._bundleReplacements["Vault/25"] = VaultValues[rng.Next(7, 11)];
			this._bundleReplacements["Vault/26"] = VaultValues[rng.Next(11, 14)];

			this._bundleReplacements["Bulletin Board/31"] = BulletinBoard31Values[rng.Next(0, 14)];
			this._bundleReplacements["Bulletin Board/32"] = BulletinBoard32Values[rng.Next(0, 6)];
			this._bundleReplacements["Bulletin Board/33"] = BulletinBoard33Values[rng.Next(0, 7)];
			this._bundleReplacements["Bulletin Board/34"] = BulletinBoard34Values[rng.Next(0, 6)];
			this._bundleReplacements["Bulletin Board/35"] = BulletinBoard35Values[rng.Next(0, 7)];

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

		private void CalculateObjectInformationEdits()
		{
			this._objectInformationReplacements.Clear();
			Random rng = Globals.RNG;

			IDictionary<Int32, string> CropPrices;
			CropPrices = new Dictionary<int, string>()
			{
                //Spring Crops
                { (int)ObjectIndexes.JazzSeeds, $"Jazz Seeds/{rng.Next(11, 20)}/-300/Seeds -74/Jazz Seeds/Plant in spring. Takes 7 days to produce a blue puffball flower. Normal seed market price is 30g"},
				{ (int)ObjectIndexes.CauliflowerSeeds,$"Cauliflower Seeds/{rng.Next(35, 55)}/-300/Seeds -74/Cauliflower Seeds/Plant these in the spring. Takes 12 days to produce a large cauliflower. Normal seed market price is 80g"},
				{ (int)ObjectIndexes.GarlicSeeds, $"Garlic Seeds/{rng.Next(15, 30)}/-300/Seeds -74/Garlic Seeds/Plant these in the spring. Takes 4 days to mature. Normal seed market price is 40g"},
				{ (int)ObjectIndexes.BeanStarter, $"Bean Starter/{rng.Next(25, 40)}/-300/Seeds -74/Bean Starter/Plant these in the spring. Takes 10 days to mature, but keeps producing after that. Yields multiple beans per harvest. Grows on a trellis. Normal seed market price is 60g"},
				{ (int)ObjectIndexes.ParsnipSeeds, $"Parsnip Seeds/{rng.Next(7, 13)}/-300/Seeds -74/Parsnip Seeds/Plant these in the spring. Takes 4 days to mature. Normal seed market price is 20g"},
				{ (int)ObjectIndexes.PotatoSeeds, $"Potato Seeds/{rng.Next(20, 35)}/-300/Seeds -74/Potato Seeds/Plant these in the spring. Takes 6 days to mature, and has a chance of yielding multiple potatoes at harvest. Normal seed market price is 50g"},
				{ (int)ObjectIndexes.KaleSeeds, $"Kale Seeds/{rng.Next(30, 42)}/-300/Seeds -74/Kale Seeds/Plant these in the spring. Takes 6 days to mature. Harvest with the scythe. Normal seed market price is 70g"},
				{ (int)ObjectIndexes.RhubarbSeeds, $"Rhubarb Seeds/{rng.Next(45, 60)}/-300/Seeds -74/Rhubarb Seeds/Plant these in the spring. Takes 13 days to mature. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.StrawberrySeeds, $"Strawberry Seeds/0/-300/Seeds -74/Strawberry Seeds/Plant these in spring. Takes 8 days to mature, and keeps producing strawberries after that. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.TulipBulb, $"Tulip Bulb/{rng.Next(3, 7)}/-300/Seeds -74/Tulip Bulb/Plant in spring. Takes 6 days to produce a colorful flower. Assorted colors. Normal seed market price is 10g"},
                //{ (int)ObjectIndexes.CherrySapling, $"Cherry Sapling/{rng.Next(50, 105) * 10}/-300/Basic -74/Cherry Sapling/Takes 28 days to produce a mature cherry tree. Bears fruit in the spring. Normal market price 3,400g. Only grows if the 8 surrounding \"tiles\" are empty."},
                //{ (int)ObjectIndexes.ApricotSapling, $"Apricot Sapling/{rng.Next(30, 65) *10}/-300/Basic -74/Apricot Sapling/Takes 28 days to produce a mature Apricot tree. Bears fruit in the spring. Normal market price 2,000g. Only grows if the 8 surrounding \"tiles\" are empty."},
                
                //Summer Crops
                { (int)ObjectIndexes.BlueberrySeeds, $"Blueberry Seeds/{rng.Next(35, 50)}/-300/Seeds -74/Blueberry Seeds/Plant these in the summer. Takes 13 days to mature, and continues to produce after first harvest. Normal seed market price is 80g"},
				{ (int)ObjectIndexes.CornSeeds, $"Corn Seeds/{rng.Next(65, 90)}/-300/Seeds -74/Corn Seeds/Plant these in the summer or fall. Takes 14 days to mature, and continues to produce after first harvest. Normal seed market price is 150g"},
				{ (int)ObjectIndexes.HopsStarter, $"Hops Starter/{rng.Next(25, 50)}/-300/Seeds -74/Hops Starter/Plant these in the summer. Takes 11 days to grow, but keeps producing after that. Grows on a trellis. Normal seed market price is 60g"},
				{ (int)ObjectIndexes.PepperSeeds, $"Pepper Seeds/{rng.Next(15, 30)}/-300/Seeds -74/Pepper Seeds/Plant these in the summer. Takes 5 days to mature, and continues to produce after first harvest. Normal seed market price is 40g"},
				{ (int)ObjectIndexes.PoppySeeds, $"Poppy Seeds/{rng.Next(40, 60)}/-300/Seeds -74/Poppy Seeds/Plant in summer. Produces a bright red flower in 7 days. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.RadishSeeds, $"Radish Seeds/{rng.Next(15, 30)}/-300/Seeds -74/Radish Seeds/Plant these in the summer. Takes 6 days to mature. Normal seed market price is 40g"},
				{ (int)ObjectIndexes.RedCabbageSeeds, $"Red Cabbage Seeds/{rng.Next(45, 60)}/-300/Seeds -74/Red Cabbage Seeds/Plant these in the summer. Takes 9 days to mature. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.StarfruitSeeds, $"Starfruit Seeds/{rng.Next(175, 250)}/-300/Seeds -74/Starfruit Seeds/Plant these in the summer. Takes 13 days to mature. Normal seed market price is 200g"},
				{ (int)ObjectIndexes.SpangleSeeds, $"Spangle Seeds/{rng.Next(20, 30)}/-300/Seeds -74/Spangle Seeds/Plant in summer. Takes 8 days to produce a vibrant tropical flower. Assorted colors. Normal seed market price is "},
				{ (int)ObjectIndexes.SunflowerSeeds, $"Sunflower Seeds/{rng.Next(15, 25)}/-300/Seeds -74/Sunflower Seeds/Plant in summer or fall. Takes 8 days to produce a large sunflower. Yields more seeds at harvest. Normal seed market price is 200g"},
				{ (int)ObjectIndexes.TomatoSeeds, $"Tomato Seeds/{rng.Next(20, 35)}/-300/Seeds -74/Tomato Seeds/Plant these in the summer. Takes 11 days to mature, and continues to produce after first harvest. Normal seed market price is 50g"},
				{ (int)ObjectIndexes.WheatSeeds, $"Wheat Seeds/{rng.Next(3, 11)}/-300/Seeds -74/Wheat Seeds/Plant these in the summer or fall. Takes 4 days to mature. Harvest with the scythe. Normal seed market price is 10g"},
                //{ (int)ObjectIndexes.OrangeSapling, $"Orange Sapling/{rng.Next(65, 125) * 10}/-300/Basic -74/Orange Sapling/Takes 28 days to produce a mature Orange tree. Bears fruit in the summer. Normal seed market price is 4,000g Only grows if the 8 surrounding \"tiles\" are empty."},
                //{ (int)ObjectIndexes.PeachSapling, $"Peach Sapling/{rng.Next(110, 175) * 10}/-300/Basic -74/Peach Sapling/Takes 28 days to produce a mature Peach tree. Bears fruit in the summer. Normal seed market price is 6,000g Only grows if the 8 surrounding \"tiles\" are empty."},
                
                //Fall Crops
                { (int)ObjectIndexes.AmaranthSeeds, $"Amaranth Seeds/{rng.Next(30, 45)}/-300/Seeds -74/Amaranth Seeds/Plant these in the fall. Takes 7 days to grow. Harvest with the scythe. Normal seed market price is 70g"},
				{ (int)ObjectIndexes.ArtichokeSeeds, $"Artichoke Seeds/{rng.Next(12, 20)}/-300/Seeds -74/Artichoke Seeds/Plant these in the fall. Takes 8 days to mature. Normal seed market price is 30g"},
				{ (int)ObjectIndexes.BeetSeeds, $"Beet Seeds/{rng.Next(8, 15)}/-300/Seeds -74/Beet Seeds/Plant these in the fall. Takes 6 days to mature. Normal seed market price is 20g"},
				{ (int)ObjectIndexes.BokChoySeeds, $"Bok Choy Seeds/{rng.Next(20, 35)}/-300/Seeds -74/Bok Choy Seeds/Plant these in the fall. Takes 4 days to mature. Normal seed market price is 50g"},
				{ (int)ObjectIndexes.CranberrySeeds, $"Cranberry Seeds/{rng.Next(110, 160)}/-300/Seeds -74/Cranberry Seeds/Plant these in the fall. Takes 7 days to mature, and continues to produce after first harvest. Normal seed market price is 240g"},
				{ (int)ObjectIndexes.EggplantSeeds, $"Eggplant Seeds/{rng.Next(7, 14)}/-300/Seeds -74/Eggplant Seeds/Plant these in the fall. Takes 5 days to mature, and continues to produce after first harvest. Normal seed market price is 20g"},
				{ (int)ObjectIndexes.FairySeeds, $"Fairy Seeds/{rng.Next(85, 115)}/-300/Seeds -74/Fairy Seeds/Plant in fall. Takes 12 days to produce a mysterious flower. Assorted Colors. Normal seed market price is 200g"},
				{ (int)ObjectIndexes.GrapeStarter, $"Grape Starter/{rng.Next(25, 40)}/-300/Seeds -74/Grape Starter/Plant these in the fall. Takes 10 days to grow, but keeps producing after that. Grows on a trellis. Normal seed market price is 60g"},
				{ (int)ObjectIndexes.PumpkinSeeds, $"Pumpkin Seeds/{rng.Next(40, 70)}/-300/Seeds -74/Pumpkin Seeds/Plant these in the fall. Takes 13 days to mature. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.YamSeeds, $"Yam Seeds/{rng.Next(25, 40)}/-300/Seeds -74/Yam Seeds/Plant these in the fall. Takes 10 days to mature. Normal seed market price is 60g"},
                //{ (int)ObjectIndexes.AppleSapling, $"Apple Sapling/{rng.Next(65, 125) * 10}/-300/Basic -74/Apple Sapling/Takes 28 days to produce a mature Apple tree. Bears fruit in the fall. Normal market price is 4,000g. Only grows if the 8 surrounding \"tiles\" are empty."},
                //{ (int)ObjectIndexes.PomegranateSapling, $"Pomegranate Sapling/{rng.Next(110, 175) * 10}/-300/Basic -74/Pomegranate Sapling/Takes 28 days to produce a mature Pomegranate tree. Bears fruit in the fall. Normal market price is 6,000g. Only grows if the 8 surrounding \"tiles\" are empty."},

            };

			foreach (KeyValuePair<int, string> pair in CropPrices)
			{
				this._objectInformationReplacements[pair.Key] = pair.Value;
			}
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

		private void CalculateFishEdits()
		{
			this._fishReplacements.Clear();
			Random rng = Globals.RNG;

			string[] fishBehavior = new string[5];
			fishBehavior[0] = $"floater"; fishBehavior[1] = $"dart"; fishBehavior[2] = $"smooth"; fishBehavior[3] = $"mixed"; fishBehavior[4] = $"sinker";

			IDictionary<Int32, string> FishEdits;
			FishEdits = new Dictionary<int, string>()
			{
				{ (int) ObjectIndexes.Pufferfish, $"Pufferfish/{rng.Next(70,80)}/{fishBehavior[rng.Next(0,5)]}/1/{rng.Next(20,50)}/1200 1600/summer/sunny/690 .4 685 .1/4/.3/.5/0"},
				{ (int) ObjectIndexes.Anchovy, $"Anchovy/{rng.Next(15,40)}/{fishBehavior[rng.Next(0,5)]}/1/{rng.Next(10,20)}/600 2600/spring fall/both/682 .2/1/.25/.3/0"},
				{ (int) ObjectIndexes.Tuna, $"Tuna/{rng.Next(60,70)}/{fishBehavior[rng.Next(0,5)]}/12/{rng.Next(30,90)}/600 1900/summer winter/both/689 .35 681 .1/3/.15/.55/0"},
				{ (int) ObjectIndexes.Sardine, $"Sardine/{rng.Next(10,50)}/{fishBehavior[rng.Next(0,5)]}/1/{rng.Next(6,18)}/600 1900/spring summer fall winter/both/683 .3/1/.65/.1/0"},
				{ (int) ObjectIndexes.Bream, $"Bream/{rng.Next(20,50)}/{fishBehavior[rng.Next(0,5)]}/12/{rng.Next(15,45)}/1800 2600/spring summer fall winter/both/684 .35/1/.45/.1/0"},
				{ (int) ObjectIndexes.LargemouthBass, $"Largemouth Bass/{rng.Next(30,65)}/{fishBehavior[rng.Next(0,5)]}/11/{rng.Next(20,40)}/600 1900/spring summer fall winter/both/685 .35/3/.4/.2/0"},
				{ (int) ObjectIndexes.SmallmouthBass, $"Smallmouth Bass/{rng.Next(15,40)}/{fishBehavior[rng.Next(0,5)]}/12/{rng.Next(15,30)}/600 2600/spring fall/both/682 .2/1/.45/.1/0"},
				{ (int) ObjectIndexes.RainbowTrout, $"Rainbow Trout/{rng.Next(30,60)}/{fishBehavior[rng.Next(0,5)]}/10/{rng.Next(15,40)}/600 1900/summer/sunny/684 .35/2/.35/.3/0"},
				{ (int) ObjectIndexes.Salmon, $"Salmon/{rng.Next(40,60)}/{fishBehavior[rng.Next(0,5)]}/24/{rng.Next(40,90)}/600 1900/fall/both/684 .35/3/.4/.2/0"},
				{ (int) ObjectIndexes.Walleye, $"Walleye/{rng.Next(30,65)}/{fishBehavior[rng.Next(0,5)]}/10/{rng.Next(20,60)}/1200 2600/fall winter/rainy/680 .35/2/.4/.15/0"},
				{ (int) ObjectIndexes.Perch, $"Perch/{rng.Next(20,60)}/{fishBehavior[rng.Next(0,5)]}/10/24/600 2600/winter/both/683 .2/1/.45/.1/0"},
				{ (int) ObjectIndexes.Carp, $"Carp/{rng.Next(5,30)}/{fishBehavior[rng.Next(0,5)]}/15/50/600 2600/spring summer fall/both/682 .2/1/.45/.1/0"},
				{ (int) ObjectIndexes.Catfish, $"Catfish/{rng.Next(55,80)}/{fishBehavior[rng.Next(0,5)]}/12/72/600 2400/spring fall winter/rainy/689 .4 680 .1/4/.4/.1/0"},
				{ (int) ObjectIndexes.Pike, $"Pike/{rng.Next(40,80)}/{fishBehavior[rng.Next(0,5)]}/15/60/600 2600/summer winter/both/690 .3 681 .1/3/.4/.15/0"},
				{ (int) ObjectIndexes.Sunfish, $"Sunfish/{rng.Next(15,45)}/{fishBehavior[rng.Next(0,5)]}/5/15/600 1900/spring summer/sunny/683 .2/1/.45/.1/0"},
				{ (int) ObjectIndexes.RedMullet, $"Red Mullet/{rng.Next(30,70)}/{fishBehavior[rng.Next(0,5)]}/8/22/600 1900/summer winter/both/680 .25/2/.4/.15/0"},
				{ (int) ObjectIndexes.Herring, $"Herring/{rng.Next(15,35)}/{fishBehavior[rng.Next(0,5)]}/8/20/600 2600/spring winter/both/685 .2/1/.45/.1/0"},
				{ (int) ObjectIndexes.Eel, $"Eel/{rng.Next(55,80)}/{fishBehavior[rng.Next(0,5)]}/12/80/1600 2600/spring fall/rainy/689 .35 680 .1/3/.55/.1/0"},
				{ (int) ObjectIndexes.Octopus, $"Octopus/{rng.Next(70,95)}/{fishBehavior[rng.Next(0,5)]}/12/48/600 1300/summer/both/688 .6 684 .1/5/.1/.08/0"},
				{ (int) ObjectIndexes.RedSnapper, $"Red Snapper/{rng.Next(30,50)}/{fishBehavior[rng.Next(0,5)]}/8/25/600 1900/summer fall winter/rainy/682 .25/2/.45/.1/0"},
				{ (int) ObjectIndexes.Squid, $"Squid/{rng.Next(55,80)}/{fishBehavior[rng.Next(0,5)]}/12/48/1800 2600/winter/both/690 .35 680 .1/3/.35/.3/0"},
				{ (int) ObjectIndexes.SeaCucumber, $"Sea Cucumber/{rng.Next(30,50)}/{fishBehavior[rng.Next(0,5)]}/3/20/600 1900/fall winter/both/683 .2 689 .4/3/.25/.25/0"},
				{ (int) ObjectIndexes.SuperCucumber, $"Super Cucumber/{rng.Next(60,90)}/{fishBehavior[rng.Next(0,5)]}/12/36/1800 2600/summer winter/both/683 .2 689 .4/4/.1/.25/0"},
				{ (int) ObjectIndexes.Ghostfish, $"Ghostfish/{rng.Next(40,60)}/{fishBehavior[rng.Next(0,5)]}/10/35/600 2600/spring summer fall winter/both/684 .35/2/.3/.3/0"},
				{ (int) ObjectIndexes.Stonefish, $"Stonefish/{rng.Next(40,75)}/{fishBehavior[rng.Next(0,5)]}/15/15/600 2600/spring summer fall winter/both/689 .2/2/.1/.1/3"},
				{ (int) ObjectIndexes.Crimsonfish, $"Crimsonfish/{rng.Next(70,95)}/{fishBehavior[rng.Next(0,5)]}/20/20/600 2000/winter/both/690 .15/4/.1/.1/5"},
				{ (int) ObjectIndexes.Angler, $"Angler/{rng.Next(75,90)}/{fishBehavior[rng.Next(0,5)]}/18/18/600 2600/spring summer fall winter/both/690 .1/4/.05/.1/3"},
				{ (int) ObjectIndexes.IcePip, $"Ice Pip/{rng.Next(70,95)}/{fishBehavior[rng.Next(0,5)]}/8/8/600 2600/spring summer fall winter/both/682 .1/2/.05/.1/5"},
				{ (int) ObjectIndexes.LavaEel, $"Lava Eel/{rng.Next(70,95)}/{fishBehavior[rng.Next(0,5)]}/32/32/600 2600/spring summer fall winter/both/684 .1/2/.05/.1/7"},
				{ (int) ObjectIndexes.Sandfish, $"Sandfish/{rng.Next(40,75)}/{fishBehavior[rng.Next(0,5)]}/8/24/600 2000/spring summer fall winter/both/682 .2/1/.65/.1/0"},
				{ (int) ObjectIndexes.ScorpionCarp, $"Scorpion Carp/{rng.Next(70,95)}/{fishBehavior[rng.Next(0,5)]}/12/32/600 2000/spring summer fall winter/both/683 .4/2/.15/.1/4"},
				{ (int) ObjectIndexes.Sturgeon, $"Sturgeon/{rng.Next(60,90)}/{fishBehavior[rng.Next(0,5)]}/12/60/600 1900/summer winter/both/689 .35 682 .1/3/.35/.2/0"},
				{ (int) ObjectIndexes.Bullhead, $"Bullhead/{rng.Next(20,65)}/{fishBehavior[rng.Next(0,5)]}/12/30/600 2600/spring summer fall winter/both/681 .25/2/.35/.2/0"},
				{ (int) ObjectIndexes.Chub, $"Chub/{rng.Next(20,60)}/{fishBehavior[rng.Next(0,5)]}/12/24/600 2600/spring summer fall winter/both/684 .35/1/.45/.1/0"},
				{ (int) ObjectIndexes.Albacore, $"Albacore/{rng.Next(40,75)}/{fishBehavior[rng.Next(0,5)]}/20/40/600 1100 1800 2600/fall winter/both/685 .35/3/.3/.15/0"},
				{ (int) ObjectIndexes.Shad, $"Shad/{rng.Next(20,65)}/{fishBehavior[rng.Next(0,5)]}/20/48/900 2600/spring summer fall/rainy/684 .35/2/.35/.2/0"},
				{ (int) ObjectIndexes.Halibut, $"Halibut/{rng.Next(25,75)}/{fishBehavior[rng.Next(0,5)]}/10/33/600 1100 1900 2600/spring summer winter/both/681 .35/3/.4/.2/0"},
				{ (int) ObjectIndexes.MidnightSquid, $"Midnight Squid/{rng.Next(20,70)}/{fishBehavior[rng.Next(0,5)]}/8/25/600 2600/spring summer fall winter/both/685 .35/3/.4/.1/0"},
				{ (int) ObjectIndexes.SpookFish, $"Spook Fish/{rng.Next(40,80)}/{fishBehavior[rng.Next(0,5)]}/8/25/600 2600/spring summer fall winter/both/685 .35/3/.4/.1/0"},
				{ (int) ObjectIndexes.Blobfish, $"Blobfish/{rng.Next(50,85)}/{fishBehavior[rng.Next(0,5)]}/8/25/600 2600/spring summer fall winter/both/685 .35/3/.4/.1/0"},

			};

			foreach (KeyValuePair<int, string> pair in FishEdits)
			{
				this._fishReplacements[pair.Key] = pair.Value;
			}

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