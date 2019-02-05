using System.Collections.Generic;

namespace Randomizer
{
	/// <summary>
	/// Contains information about foragable items per season
	/// </summary>
	public class ForagableLocationData
	{
		public Locations Location { get; set; }
		public string LocationName { get { return Location.ToString(); } }
		public List<ForagableData> SpringForagables { get; } = new List<ForagableData>();
		public List<ForagableData> SummerForagables { get; } = new List<ForagableData>();
		public List<ForagableData> FallForagables { get; } = new List<ForagableData>();
		public List<ForagableData> WinterForagables { get; } = new List<ForagableData>();

		/// <summary>
		/// Returns whether there's any foragable location data
		/// </summary>
		/// <returns />
		public bool HasData()
		{
			return SpringForagables.Count + SummerForagables.Count + FallForagables.Count + WinterForagables.Count > 0;
		}

		/// <summary>
		/// Gets the string for this set of location data
		/// </summary>
		/// <returns>
		/// A string in the following format:
		/// springForagables/summerForagables/fallForagables/winterForagables/springFishing/summerFishing/fallFishing/winterFishing/dirtFindings
		/// </returns>
		public override string ToString()
		{
			string springForagables = GetStringForSeason(SpringForagables);
			string summerForagables = GetStringForSeason(SummerForagables);
			string fallForagables = GetStringForSeason(FallForagables);
			string winterForagables = GetStringForSeason(WinterForagables);
			string foragableString = $"{springForagables}/{summerForagables}/{fallForagables}/{winterForagables}";
			return $"{foragableString}/{GetBackendLocationData()}";
		}

		/// <summary>
		/// Gets the string for the given season of data
		/// </summary>
		/// <param name="data">The season of data</param>
		/// <returns>
		/// -1 if there's no data; the string in the following format otherwise:
		///   {itemId} {itemRarity} ...
		/// </returns>
		private string GetStringForSeason(List<ForagableData> foragableList)
		{
			if (foragableList.Count == 0) { return "-1"; }
			string output = "";

			foreach (ForagableData data in foragableList)
			{
				output += $"{data.ToString()} ";
			}
			return output.Trim();
		}

		/// <summary>
		/// Gets the hard-coded string of location data for the current location name
		/// </summary>
		/// <returns></returns>
		private string GetBackendLocationData()
		{
			switch (LocationName)
			{
				case "Desert":
					return "153 -1 164 -1 165 -1/153 -1 164 -1 165 -1/153 -1 164 -1 165 -1/153 -1 164 -1 165 -1/390 .25 330 1";
				case "BusStop":
					return "-1/-1/-1/-1/584 .08 378 .15 102 .15 390 .25 330 1";
				case "Forest":
					return "153 -1 145 0 143 0 137 1 132 0 706 0 702 0/153 -1 145 0 144 -1 138 0 132 0 706 0 704 0 702 0/143 0 153 -1 140 -1 139 0 137 1 132 0 706 0 702 0 699 0/699 0 143 0 153 -1 144 -1 141 -1 140 -1 132 0 707 0 702 0/378 .08 579 .1 588 .1 102 .15 390 .25 330 1";
				case "Town":
					return "137 -1 132 -1 143 -1 145 -1 153 -1 706 -1/138 -1 132 -1 144 -1 145 -1 153 -1 706 -1/139 -1 137 -1 132 -1 140 -1 143 -1 153 -1 706 -1 699 -1/132 -1 140 -1 141 -1 143 -1 144 -1 153 -1 707 -1 699 -1/378 .2 110 .2 583 .1 102 .2 390 .25 330 1";
				case "Mountain":
					return "136 -1 142 -1 153 -1 702 -1 700 -1 163 -1/136 -1 142 -1 153 -1 138 -1 702 -1 700 -1 698 -1/136 -1 140 -1 142 -1 153 -1 702 -1 700 -1/136 -1 140 -1 141 -1 153 -1 707 -1 702 -1 700 -1 698 -1/382 .06 581 .1 378 .1 102 .15 390 .25 330 1";
				case "Backwoods":
					return "136 -1 142 -1 153 -1 702 -1 700 -1 163 -1/136 -1 142 -1 153 -1 138 -1 702 -1 700 -1 698 -1/136 -1 140 -1 142 -1 153 -1 702 -1 700 -1/136 -1 140 -1 141 -1 153 -1 707 -1 702 -1 700 -1 698 -1/382 .06 582 .1 378 .1 102 .15 390 .25 330 1";
				case "Railroad":
					return "-1/-1/-1/-1/580 .1 378 .15 102 .19 390 .25 330 1";
				case "Beach":
					return "129 -1 131 -1 147 -1 148 -1 152 -1 708 -1/128 -1 130 -1 146 -1 149 -1 150 -1 152 -1 155 -1 708 -1 701 -1/129 -1 131 -1 148 -1 150 -1 152 -1 154 -1 155 -1 705 -1 701 -1/708 -1 130 -1 131 -1 146 -1 147 -1 150 -1 151 -1 152 -1 154 -1 705 -1/384 .08 589 .09 102 .15 390 .25 330 1";
				case "Woods":
					return "734 -1 142 -1 143 -1/734 -1 142 -1 143 -1/734 -1 142 -1 143 -1/734 -1 142 -1 143 -1/390 .25 330 1";
				default:
					Globals.ModRef.Monitor.Log($"ERROR: No location data found for {LocationName}!");
					return "-1/-1/-1/-1/-1";
			}
		}
	}
}
