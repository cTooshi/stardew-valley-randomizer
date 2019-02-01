using System;
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

		private static List<Item> _springForagables { get; } = new List<Item>();
		private static List<Item> _summerForagables { get; } = new List<Item>();
		private static List<Item> _fallForagables { get; } = new List<Item>();
		private static List<Item> _winterForagables { get; } = new List<Item>();

		private static List<Item> _beachItems { get; } = new List<Item>();
		private static List<Item> _woodsItems { get; } = new List<Item>();
		private static List<Item> _desertItems { get; } = new List<Item>();

		/// <summary>
		/// Randomizes all foragables to a random season and location - does not yet handle fishing or dirt items
		/// </summary>
		/// <param name="locationsReplacements">The dictionary used for the replacements</param>
		/// <param name="rng">The Random object</param>
		public static void Randomize(Dictionary<string, string> locationsReplacements, Random rng)
		{
			locationsReplacements.Clear();
			GroupForagablesBySeason(rng);

			List<ForagableLocationData> foragableLocationDataList = GetForagableLocationDataList(rng);

			foreach (ForagableLocationData foragableLocationData in foragableLocationDataList)
			{
				locationsReplacements.Add(foragableLocationData.LocationName, foragableLocationData.ToString());
				Globals.mod.Monitor.Log($"Location string {foragableLocationData.LocationName}: {foragableLocationData.ToString()}");
			}
		}

		/// <summary>
		/// Populates the list of foragables by season
		/// </summary>
		/// <param name="rng">The Random object</param>
		public static void GroupForagablesBySeason(Random rng)
		{
			List<Item> foragableItems = ItemList.Items.Values.Where(x => x.ShouldBeForagable).ToList();

			// Initializes each season with 5 foragables
			AddMultipleToList(foragableItems, _springForagables, 5, rng);
			AddMultipleToList(foragableItems, _summerForagables, 5, rng);
			AddMultipleToList(foragableItems, _fallForagables, 5, rng);
			AddMultipleToList(foragableItems, _winterForagables, 5, rng);

			// Initializes each unique area with their unique foragables
			AddMultipleToList(foragableItems, _beachItems, 3, rng);
			AddMultipleToList(foragableItems, _woodsItems, 1, rng);
			AddMultipleToList(foragableItems, _desertItems, 2, rng);

			// Ensure the rest of the foragables get distributed
			DistributeRemainingForagables(foragableItems, rng);
		}

		/// <summary>
		/// Populates the given list with five foragables
		/// </summary>
		/// <param name="foragableList">The list of all foragables to choose from</param>
		/// <param name="listToPopulate">The list to populate</param>
		/// <param name="rng">The Random object</param>
		private static void AddMultipleToList(List<Item> foragableList, List<Item> listToPopulate, int numberToAdd, Random rng)
		{
			if (foragableList.Count < numberToAdd)
			{
				Globals.mod.Monitor.Log($"Not enough foragables to initialize everything - trying to add {numberToAdd} from a list of {foragableList.Count}.");
				return;
			}

			for (int i = 0; i < numberToAdd; i++)
			{
				AddToList(foragableList, listToPopulate, rng);
			}
		}

		/// <summary>
		/// Distribute the rest of the foragable list
		/// </summary>
		/// <param name="foragableList">The list of all foragables to choose from</param>
		/// <param name="rng">The Random object</param>
		private static void DistributeRemainingForagables(List<Item> foragableList, Random rng)
		{
			bool keepLooping = true;

			while (keepLooping)
			{
				int season = rng.Next(0, 4);
				switch (season)
				{
					case 0:
						keepLooping = AddToList(foragableList, _springForagables, rng);
						break;
					case 1:
						keepLooping = AddToList(foragableList, _summerForagables, rng);
						break;
					case 2:
						keepLooping = AddToList(foragableList, _fallForagables, rng);
						break;
					case 3:
						keepLooping = AddToList(foragableList, _winterForagables, rng);
						break;
					default:
						Globals.mod.Monitor.Log("ERROR: Should not have generated a value above 3 for a season check!");
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
		/// <param name="rng">The Random object</param>
		/// <returns>Whether there's more in the list to add after the call</returns>
		private static bool AddToList(List<Item> foragableList, List<Item> listToPopulate, Random rng)
		{
			if (foragableList.Count == 0) { return false; }

			int foragableIndex = rng.Next(0, foragableList.Count);
			listToPopulate.Add(foragableList[foragableIndex]);
			foragableList.RemoveAt(foragableIndex);

			return foragableList.Count > 0;
		}

		/// <summary>
		/// Gets the list for foragable location data - one per location
		/// </summary>
		/// <param name="rng"></param>
		/// <returns></returns>
		private static List<ForagableLocationData> GetForagableLocationDataList(Random rng)
		{
			// TODO: do not hard code this list
			string[] locationNameList = {
				"Desert",
				"BusStop",
				"Forest",
				"Town",
				"Mountain",
				"Backwoods",
				"Railroad",
				"Beach",
				"Woods"
			};

			var forgabableLocationDataList = new List<ForagableLocationData>();
			foreach (string locationName in locationNameList)
			{
				// Add any item to the desert
				if (locationName == "Desert")
				{
					AddUniqueNewForagable(_desertItems, rng);
				}

				ForagableLocationData foragableLocationData = new ForagableLocationData()
				{
					LocationName = locationName
				};

				PopulateLocationBySeason(foragableLocationData, Seasons.Spring, rng);
				PopulateLocationBySeason(foragableLocationData, Seasons.Summer, rng);
				PopulateLocationBySeason(foragableLocationData, Seasons.Fall, rng);
				PopulateLocationBySeason(foragableLocationData, Seasons.Winter, rng);

				forgabableLocationDataList.Add(foragableLocationData);
			}

			return forgabableLocationDataList;
		}

		/// <summary>
		/// Populate the location data's season arrays
		/// </summary>
		/// <param name="foragableLocationData">The location data</param>
		/// <param name="season">The season</param>
		/// <param name="rng">The Random object</param>
		private static void PopulateLocationBySeason(ForagableLocationData foragableLocationData, Seasons season, Random rng)
		{
			List<ForagableData> foragableDataList = null;
			List<Item> foragableItemList = null;

			switch (season)
			{
				case Seasons.Spring:
					foragableDataList = foragableLocationData.SpringForagables;
					foragableItemList = _springForagables;
					break;
				case Seasons.Summer:
					foragableDataList = foragableLocationData.SummerForagables;
					foragableItemList = _summerForagables;
					break;
				case Seasons.Fall:
					foragableDataList = foragableLocationData.FallForagables;
					foragableItemList = _fallForagables;
					break;
				case Seasons.Winter:
					foragableDataList = foragableLocationData.WinterForagables;
					foragableItemList = _winterForagables;
					break;
			}

			if (foragableDataList == null || foragableItemList == null)
			{
				Globals.mod.Monitor.Log($"ERROR: Could not get foragable list for season: {season}");
			}

			if (foragableLocationData.LocationName == "Desert")
			{
				foragableItemList = _desertItems;
			}

			// Give the beach a random item from the season, then only assign the beach items after that
			if (foragableLocationData.LocationName == "Beach")
			{
				Item randomSeasonItem = foragableItemList[rng.Next(0, foragableItemList.Count)];
				foragableDataList.Add(new ForagableData(randomSeasonItem.Id, rng));
				foragableItemList = _beachItems;
			}

			// Give the woods a random item from ANY season, then only assign the woods items after that
			if (foragableLocationData.LocationName == "Woods")
			{
				AddUniqueNewForagable(_woodsItems, rng);
				foragableItemList = _woodsItems;
			}

			foreach (Item item in foragableItemList)
			{
				foragableDataList.Add(new ForagableData(item.Id, rng));
			}

			// Remove the item that was added to the woods
			if (foragableLocationData.LocationName == "Woods" && _woodsItems.Count > 1)
			{
				_woodsItems.RemoveAt(_woodsItems.Count - 1);
			}
		}

		/// <summary>
		/// Adds a random new item to a list
		/// </summary>
		/// <param name="listToPopulate">The list to populate</param>
		/// <param name="rng">The Random object</param>
		private static void AddUniqueNewForagable(List<Item> listToPopulate, Random rng)
		{
			if (listToPopulate.Count >= _allForagables.Count)
			{
				Globals.mod.Monitor.Log("WARNING: Tried to add a unique foragable when the given list was full!");
				return;
			}

			int itemIndex;
			do
			{
				itemIndex = rng.Next(0, _allForagables.Count);
			} while (listToPopulate.Contains(_allForagables[itemIndex]));

			listToPopulate.Add(_allForagables[itemIndex]);
		}
	}
}
