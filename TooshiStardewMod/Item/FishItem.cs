using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Represents a fish
	/// </summary>
	public class FishItem : Item
	{
		// TODO when this is better randomized: fish behaviors

		public List<Seasons> AvailableSeasons { get; set; } =
			new List<Seasons> { Seasons.Spring, Seasons.Summer, Seasons.Fall, Seasons.Winter };
		public List<Seasons> WoodsOnlySeasons { get; set; } = new List<Seasons>();
		public List<Weather> Weathers { get; set; } // Empty means any
		public List<Locations> AvailableLocations { get; set; } = new List<Locations>();
		public Range Times { get; set; } = new Range(600, 2600); // That's anytime in the day
		public Range ExcludedTimes { get; set; } = new Range(0, 0);

		public bool IsNightFish { get { return Times.MaxValue >= 1800; } }
		public bool IsRainFish { get { return Weathers.Contains(Weather.Rain); } }
		public bool IsSunFish { get { return Weathers.Contains(Weather.Sun); } }
		public bool IsWindFish { get { return Weathers.Contains(Weather.Wind); } }

		public bool IsSpringFish { get { return AvailableSeasons.Contains(Seasons.Spring); } }
		public bool IsSummerFish { get { return AvailableSeasons.Contains(Seasons.Summer); } }
		public bool IsFallFish { get { return AvailableSeasons.Contains(Seasons.Fall); } }
		public bool IsWinterFish { get { return AvailableSeasons.Contains(Seasons.Winter); } }

		public FishItem(int id, ObtainingDifficulties difficultyToObtain = ObtainingDifficulties.LargeTimeRequirements) : base(id)
		{
			DifficultyToObtain = difficultyToObtain;
			IsFish = true;
		}

		/// <summary>
		/// Gets all the fish
		/// </summary>
		/// <param name="includeLegendaries">Include the legendary fish</param>
		/// <returns />
		public static List<Item> Get(bool includeLegendaries = false)
		{
			return ItemList.Items.Values.Where(x =>
				x.IsFish &&
				x.DifficultyToObtain != ObtainingDifficulties.Impossible &&
				(includeLegendaries || x.DifficultyToObtain < ObtainingDifficulties.EndgameItem)
			).ToList();
		}

		/// <summary>
		/// Gets all the fish
		/// </summary>
		/// <param name="includeLegendaries">Include the legendary fish</param>
		/// <returns />
		public static List<FishItem> GetListAsFishItem(bool includeLegendaries = false)
		{
			return ItemList.Items.Values.Where(x =>
				x.IsFish &&
				x.DifficultyToObtain != ObtainingDifficulties.Impossible &&
				(includeLegendaries || x.DifficultyToObtain < ObtainingDifficulties.EndgameItem)
			).Cast<FishItem>().ToList();
		}

		/// <summary>
		/// Gets all the fish that can be caught during the given season
		/// </summary>
		/// <param name="season">The season</param>
		/// <param name="includeLegendaries">Include the legendary fish</param>
		/// <returns />
		public static List<Item> Get(Seasons season, bool includeLegendaries = false)
		{
			List<FishItem> allFish = GetListAsFishItem(includeLegendaries);
			switch (season)
			{
				case Seasons.Spring:
					return allFish.Where(x => x.IsSpringFish).Cast<Item>().ToList();
				case Seasons.Summer:
					return allFish.Where(x => x.IsSummerFish).Cast<Item>().ToList();
				case Seasons.Fall:
					return allFish.Where(x => x.IsFallFish).Cast<Item>().ToList();
				case Seasons.Winter:
					return allFish.Where(x => x.IsWinterFish).Cast<Item>().ToList();
			}

			Globals.ConsoleWrite($"ERROR: Tried to get fish belonging to the non-existent season: {season.ToString()}");
			return new List<Item>();
		}

		/// <summary>
		/// Gets all the fish that can be caught during the given weather type
		/// </summary>
		/// <param name="weather">The weather type</param>
		/// <param name="includeLegendaries">Include the legendary fish</param>
		/// <returns />
		public static List<Item> Get(Weather weather, bool includeLegendaries = false)
		{
			List<FishItem> allFish = GetListAsFishItem(includeLegendaries);
			switch (weather)
			{
				case Weather.Any:
					return allFish.Where(x => x.Weathers.Count == 0).Cast<Item>().ToList();
				case Weather.Sun:
					return allFish.Where(x => x.IsSunFish).Cast<Item>().ToList();
				case Weather.Rain:
					return allFish.Where(x => x.IsRainFish).Cast<Item>().ToList();
				case Weather.Wind:
					return allFish.Where(x => x.IsWindFish).Cast<Item>().ToList();
			}

			Globals.ConsoleWrite($"ERROR: Tried to get fish belonging to the non-existent weather: {weather.ToString()}");
			return new List<Item>();
		}

		/// <summary>
		/// Gets all the fish that can be caught at a given location
		/// </summary>
		/// <param name="location">The weather type</param>
		/// <param name="includeLegendaries">Include the legendary fish</param>
		/// <returns />
		public static List<Item> Get(Locations location, bool includeLegendaries = false)
		{
			List<FishItem> allFish = GetListAsFishItem(includeLegendaries);
			return allFish.Where(x => x.AvailableLocations.Contains(location)).Cast<Item>().ToList();
		}

		/// <summary>
		/// Gets the fish that can be caught at 2am that can't be caught in the morning
		/// OR that have exclusions and can be caught at night
		/// </summary>
		/// <param name="startingTime">The time</param>
		/// <param name="includeLegendaries">Include the legendary fish</param>
		/// <returns />
		public static List<Item> GetNightFish(bool includeLegendaries = false)
		{
			var test = GetListAsFishItem(includeLegendaries);
			return GetListAsFishItem(includeLegendaries).Cast<FishItem>().Where(x =>
				(x.Times.MinValue >= 1200 && x.Times.MaxValue >= 2600) ||
				(x.ExcludedTimes.MaxValue >= 600 && x.ExcludedTimes.MaxValue < 2600)
			).Cast<Item>().ToList();
		}

		/// <summary>
		/// Gets all the legendary fish
		/// </summary>
		/// <returns />
		public static List<Item> GetLegendaries()
		{
			return ItemList.Items.Values.Where(x =>
				x.IsFish &&
				x.DifficultyToObtain == ObtainingDifficulties.EndgameItem
			).ToList();
		}
	}
}
