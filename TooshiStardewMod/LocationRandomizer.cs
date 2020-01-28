using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// This randomizes the Locations.xnb data
	/// </summary>
	public class LocationRandomizer
	{
		private static List<Item> _allForagables { get; } = ItemList.Items.Values.Where(x => x.ShouldBeForagable).ToList();

		public static List<Item> SpringForagables { get; } = new List<Item>();
		public static List<Item> SummerForagables { get; } = new List<Item>();
		public static List<Item> FallForagables { get; } = new List<Item>();
		public static List<Item> WinterForagables { get; } = new List<Item>();

		public static List<Item> BeachForagables { get; } = new List<Item>();
		public static List<Item> WoodsForagables { get; } = new List<Item>();
		public static List<Item> DesertForagables { get; } = new List<Item>();

		/// <summary>
		/// Randomizes all foragables to a random season and location - does not yet handle fishing or dirt items
		/// </summary>
		/// <returns>A dictionary of locations to replace</returns>
		public static Dictionary<string, string> Randomize()
		{
			SpringForagables.Clear();
			SummerForagables.Clear();
			FallForagables.Clear();
			WinterForagables.Clear();
			BeachForagables.Clear();
			WoodsForagables.Clear();
			DesertForagables.Clear();

			var locationsReplacements = new Dictionary<string, string>();
			GroupForagablesBySeason();

			List<LocationData> foragableLocationDataList = GetForagableLocationDataList();

			Globals.SpoilerWrite("==== Foragables ====");
			foreach (LocationData foragableLocationData in foragableLocationDataList)
			{
				locationsReplacements.Add(foragableLocationData.LocationName, foragableLocationData.ToString());

				Globals.SpoilerWrite("");
				Globals.SpoilerWrite($">> {foragableLocationData.LocationName} <<");

				WriteResultsForSeason(Seasons.Spring, foragableLocationData);
				WriteResultsForSeason(Seasons.Summer, foragableLocationData);
				WriteResultsForSeason(Seasons.Fall, foragableLocationData);
				WriteResultsForSeason(Seasons.Winter, foragableLocationData);

				Globals.SpoilerWrite("");
				Globals.SpoilerWrite($"Extra digging item and rarity: {foragableLocationData.ExtraDiggingItem.Name} | {foragableLocationData.ExtraDiggingItemRarity}");
			}
			Globals.SpoilerWrite("");

			return locationsReplacements;
		}

		/// <summary>
		/// Writes out the results for the given season and foragable location data
		/// </summary>
		/// <param name="season">The season to write the results for</param>
		/// <param name="locationData">The data to write the results for</param>
		private static void WriteResultsForSeason(Seasons season, LocationData locationData)
		{
			List<ForagableData> dataToWrite = null;
			switch (season)
			{
				case Seasons.Spring:
					dataToWrite = locationData.SpringForagables;
					break;
				case Seasons.Summer:
					dataToWrite = locationData.SummerForagables;
					break;
				case Seasons.Fall:
					dataToWrite = locationData.FallForagables;
					break;
				case Seasons.Winter:
					dataToWrite = locationData.WinterForagables;
					break;
			}

			if (dataToWrite == null)
			{
				Globals.ConsoleWrite($"ERROR: Could not find the foragable list for {season.ToString()}");
				return;
			}

			Globals.SpoilerWrite("");
			Globals.SpoilerWrite(season.ToString());
			foreach (ForagableData foragableData in dataToWrite)
			{
				Globals.SpoilerWrite($"{foragableData.ItemId}: {ItemList.Items[foragableData.ItemId].Name} | {foragableData.ItemRarity}");
			}
		}

		/// <summary>
		/// Populates the list of foragables by season
		/// </summary>
		public static void GroupForagablesBySeason()
		{
			List<Item> foragableItems = ItemList.Items.Values.Where(x => x.ShouldBeForagable).ToList();

			// Initializes each unique area with their unique foragables
			AddMultipleToList(foragableItems, BeachForagables, 3);
			AddMultipleToList(foragableItems, WoodsForagables, 1);
			AddMultipleToList(foragableItems, DesertForagables, 2);

			// Initializes each season with an even number of foragables
			int numberToDistribute = foragableItems.Count / 4;
			AddMultipleToList(foragableItems, SpringForagables, numberToDistribute);
			AddMultipleToList(foragableItems, SummerForagables, numberToDistribute);
			AddMultipleToList(foragableItems, FallForagables, numberToDistribute);
			AddMultipleToList(foragableItems, WinterForagables, numberToDistribute);

			// Ensure the rest of the foragables get distributed
			DistributeRemainingForagables(foragableItems);
		}

		/// <summary>
		/// Populates the given list with five foragables
		/// </summary>
		/// <param name="foragableList">The list of all foragables to choose from</param>
		/// <param name="listToPopulate">The list to populate</param>
		private static void AddMultipleToList(List<Item> foragableList, List<Item> listToPopulate, int numberToAdd)
		{
			if (foragableList.Count < numberToAdd)
			{
				Globals.ModRef.Monitor.Log($"Not enough foragables to initialize everything - trying to add {numberToAdd} from a list of {foragableList.Count}.");
				return;
			}

			for (int i = 0; i < numberToAdd; i++)
			{
				AddToList(foragableList, listToPopulate);
			}
		}

		/// <summary>
		/// Distribute the rest of the foragable list
		/// </summary>
		/// <param name="foragableList">The list of all foragables to choose from</param>
		private static void DistributeRemainingForagables(List<Item> foragableList)
		{
			bool keepLooping = true;

			while (keepLooping)
			{
				int season = Globals.RNG.Next(0, 4);
				switch (season)
				{
					case 0:
						keepLooping = AddToList(foragableList, SpringForagables);
						break;
					case 1:
						keepLooping = AddToList(foragableList, SummerForagables);
						break;
					case 2:
						keepLooping = AddToList(foragableList, FallForagables);
						break;
					case 3:
						keepLooping = AddToList(foragableList, WinterForagables);
						break;
					default:
						Globals.ModRef.Monitor.Log("ERROR: Should not have generated a value above 3 for a season check!");
						keepLooping = false;
						break;
				}
			}
		}

		/// <summary>
		/// Adds a foragable to the given list
		/// </summary>
		/// <param name="foragableList">The list of all foragables to choose from</param>
		/// <param name="listToPopulate">The list to populate</param>
		/// <returns>Whether there's more in the list to add after the call</returns>
		private static bool AddToList(List<Item> foragableList, List<Item> listToPopulate)
		{
			if (foragableList.Count == 0) { return false; }
			listToPopulate.Add(Globals.RNGGetAndRemoveRandomValueFromList(foragableList));
			return foragableList.Count > 0;
		}

		/// <summary>
		/// Gets the list for foragable location data - one per location
		/// </summary>
		/// <returns></returns>
		private static List<LocationData> GetForagableLocationDataList()
		{
			var foragableLocations = new List<Locations>
			{
				Locations.Desert,
				Locations.BusStop,
				Locations.Forest,
				Locations.Town,
				Locations.Mountain,
				Locations.Backwoods,
				Locations.Railroad,
				Locations.Beach,
				Locations.Woods
			};

			var forgableLocationDataList = new List<LocationData>();
			foreach (Locations location in foragableLocations)
			{
				// Add any item to the desert
				if (location == Locations.Desert)
				{
					AddUniqueNewForagable(DesertForagables);
				}

				LocationData foragableLocationData = new LocationData()
				{
					Location = location
				};

				PopulateLocationBySeason(foragableLocationData, Seasons.Spring);
				PopulateLocationBySeason(foragableLocationData, Seasons.Summer);
				PopulateLocationBySeason(foragableLocationData, Seasons.Fall);
				PopulateLocationBySeason(foragableLocationData, Seasons.Winter);
				SetExtraDiggableItemInfo(foragableLocationData);

				forgableLocationDataList.Add(foragableLocationData);
			}

			LocationData mineLocationData = new LocationData() { Location = Locations.UndergroundMine };
			SetExtraDiggableItemInfo(mineLocationData);
			forgableLocationDataList.Add(mineLocationData);
			return forgableLocationDataList;
		}

		/// <summary>
		/// Populate the location data's season arrays
		/// </summary>
		/// <param name="foragableLocationData">The location data</param>
		/// <param name="season">The season</param>
		private static void PopulateLocationBySeason(LocationData foragableLocationData, Seasons season)
		{
			List<ForagableData> foragableDataList = null;
			List<Item> foragableItemList = null;

			switch (season)
			{
				case Seasons.Spring:
					foragableDataList = foragableLocationData.SpringForagables;
					foragableItemList = SpringForagables;
					break;
				case Seasons.Summer:
					foragableDataList = foragableLocationData.SummerForagables;
					foragableItemList = SummerForagables;
					break;
				case Seasons.Fall:
					foragableDataList = foragableLocationData.FallForagables;
					foragableItemList = FallForagables;
					break;
				case Seasons.Winter:
					foragableDataList = foragableLocationData.WinterForagables;
					foragableItemList = WinterForagables;
					break;
			}

			if (foragableDataList == null || foragableItemList == null)
			{
				Globals.ModRef.Monitor.Log($"ERROR: Could not get foragable list for season: {season}");
			}

			if (foragableLocationData.Location == Locations.Desert)
			{
				foragableItemList = DesertForagables;
			}

			// Give the beach a random item from the season, then only assign the beach items after that
			if (foragableLocationData.Location == Locations.Beach)
			{
				Item randomSeasonItem = foragableItemList[Globals.RNG.Next(0, foragableItemList.Count)];
				foragableDataList.Add(new ForagableData(randomSeasonItem.Id));
				foragableItemList = BeachForagables;
			}

			// Give the woods a random item from ANY season, then only assign the woods items after that
			if (foragableLocationData.LocationName == "Woods")
			{
				AddUniqueNewForagable(WoodsForagables);
				foragableItemList = WoodsForagables;
			}

			foreach (Item item in foragableItemList)
			{
				foragableDataList.Add(new ForagableData(item.Id));
			}

			// Remove the item that was added to the woods
			if (foragableLocationData.LocationName == "Woods" && WoodsForagables.Count > 1)
			{
				WoodsForagables.RemoveAt(WoodsForagables.Count - 1);
			}

			// Add a random item that's really rare to see
			Item randomItem = Globals.RNGGetRandomValueFromList(
				ItemList.Items.Values.Where(x =>
					x.DifficultyToObtain >= ObtainingDifficulties.MediumTimeRequirements &&
					x.DifficultyToObtain < ObtainingDifficulties.Impossible).ToList()
			);
			foragableDataList.Add(new ForagableData(randomItem.Id) { ItemRarity = 0.001 });
		}

		/// <summary>
		/// Adds a random new item to a list
		/// </summary>
		/// <param name="listToPopulate">The list to populate</param>
		private static void AddUniqueNewForagable(List<Item> listToPopulate)
		{
			if (listToPopulate.Count >= _allForagables.Count)
			{
				Globals.ModRef.Monitor.Log("WARNING: Tried to add a unique foragable when the given list was full!");
				return;
			}

			int itemIndex;
			do
			{
				itemIndex = Globals.RNG.Next(0, _allForagables.Count);
			} while (listToPopulate.Contains(_allForagables[itemIndex]));

			listToPopulate.Add(_allForagables[itemIndex]);
		}

		/// <summary>
		/// Sets the diggable item info on the given data
		/// </summary>
		/// <param name="locationData">The location data</param>
		private static void SetExtraDiggableItemInfo(LocationData locationData)
		{
			ObtainingDifficulties difficulty = GetRandomItemDifficulty();
			double probability = 0;
			switch (difficulty)
			{
				case ObtainingDifficulties.NoRequirements:
					probability = (double)Range.GetRandomValue(30, 60) / 100;
					break;
				case ObtainingDifficulties.SmallTimeRequirements:
					probability = (double)Range.GetRandomValue(30, 40) / 100;
					break;
				case ObtainingDifficulties.MediumTimeRequirements:
					probability = (double)Range.GetRandomValue(20, 30) / 100;
					break;
				case ObtainingDifficulties.LargeTimeRequirements:
					probability = (double)Range.GetRandomValue(10, 20) / 100;
					break;
				case ObtainingDifficulties.UncommonItem:
					probability = (double)Range.GetRandomValue(5, 15) / 100;
					break;
				case ObtainingDifficulties.RareItem:
					probability = (double)Range.GetRandomValue(1, 5) / 100;
					break;
				default:
					Globals.ConsoleWrite($"ERROR: Attempting to get a diggable item with invalid difficulty: {difficulty}");
					difficulty = ObtainingDifficulties.NoRequirements;
					probability = (double)Range.GetRandomValue(30, 60) / 100;
					break;
			}

			locationData.ExtraDiggingItem = ItemList.GetRandomItemAtDifficulty(difficulty);
			locationData.ExtraDiggingItemRarity = probability;
		}

		/// <summary>
		/// Gets a random item difficulty
		/// - 1/2 = no req
		/// - 1/4 = small time req
		/// - 1/8 = medium time req
		/// - 1/16 = large time req
		/// - 1/32 = uncommon
		/// - 1/64 = rare
		/// </summary>
		/// <returns></returns>
		private static ObtainingDifficulties GetRandomItemDifficulty()
		{
			if (Globals.RNGGetNextBoolean())
			{
				return ObtainingDifficulties.NoRequirements;
			}

			else if (Globals.RNGGetNextBoolean())
			{
				return ObtainingDifficulties.SmallTimeRequirements;
			}

			else if (Globals.RNGGetNextBoolean())
			{
				return ObtainingDifficulties.MediumTimeRequirements;
			}

			else if (Globals.RNGGetNextBoolean())
			{
				return ObtainingDifficulties.LargeTimeRequirements;
			}

			else if (Globals.RNGGetNextBoolean())
			{
				return ObtainingDifficulties.UncommonItem;
			}

			else
			{
				return ObtainingDifficulties.RareItem;
			}
		}
	}
}
