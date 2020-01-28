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
		private Dictionary<string, string> _blueprintReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _grandpaStringReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _stringReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _fishReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _questReplacements = new Dictionary<int, string>();
		private Dictionary<string, string> _locationsReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _objectInformationReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _fruitTreeReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _cropReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _weaponReplacements = new Dictionary<int, string>();
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
			if (asset.AssetNameEquals("Data/Fish")) { return ModEntry.configDict.ContainsKey("fish") ? ModEntry.configDict["fish"] : true; }
			if (asset.AssetNameEquals("Data/Quests")) { return true; }
			if (asset.AssetNameEquals("Data/Locations")) { return ModEntry.configDict.ContainsKey("foragable and fish locations") ? ModEntry.configDict["foragable and fish locations"] : true; ; }
			if (asset.AssetNameEquals("Data/fruitTrees")) { return ModEntry.configDict.ContainsKey("fruit trees") ? ModEntry.configDict["fruit trees"] : true; }
			if (asset.AssetNameEquals("Data/Crops")) { return ModEntry.configDict.ContainsKey("crops prices") ? ModEntry.configDict["crop prices"] : true; }

			if (asset.AssetNameEquals("Data/weapons")) { return true; } //TODO: add an entry for this
			return false;
		}

		private void ApplyEdits<TKey, TValue>(IAssetData asset, IDictionary<TKey, TValue> edits)
		{
			IAssetDataForDictionary<TKey, TValue> assetDict = asset.AsDictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> edit in edits)
			{
				assetDict.Data[edit.Key] = edit.Value;
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
			else if (asset.AssetNameEquals("Data/weapons"))
			{
				this.ApplyEdits(asset, this._weaponReplacements);
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
			this._mod.Helper.Content.InvalidateCache("Data/weapons");
		}

		public void CalculateEditsBeforeLoad()
		{
			_grandpaStringReplacements = StringsRandomizer.RandomizeGrandpasStory();
		}

		public void CalculateEdits()
		{
			ItemList.Initialize();
			ValidateItemList();

			EditedObjectInformation editedObjectInfo = new EditedObjectInformation();
			FishRandomizer.Randomize(editedObjectInfo);
			_fishReplacements = editedObjectInfo.FishReplacements;

			CropRandomizer.Randomize(editedObjectInfo);
			_fruitTreeReplacements = editedObjectInfo.FruitTreeReplacements;
			_cropReplacements = editedObjectInfo.CropsReplacements;

			_objectInformationReplacements = editedObjectInfo.ObjectInformationReplacements;

			_blueprintReplacements = BlueprintRandomizer.Randomize();
			_recipeReplacements = CraftingRecipeRandomizer.Randomize();
			_stringReplacements = StringsRandomizer.Randomize();
			_locationsReplacements = LocationRandomizer.Randomize();
			_bundleReplacements = BundleRandomizer.Randomize();
			MusicReplacements = MusicRandomizer.Randomize();
			_questReplacements = QuestRandomizer.Randomize();
			_weaponReplacements = WeaponRandomizer.Randomize();
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
	}
}