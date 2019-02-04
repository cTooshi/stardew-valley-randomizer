using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Represents a seed
	/// </summary>
	public class SeedItem : Item
	{
		public int Price { get; set; }
		public List<Seasons> GrowingSeasons { get; set; } = new List<Seasons>();
		public string CategoryString { get; set; }
		public string Description { get; set; }

		public List<int> GrowthStages { get; set; } = new List<int>();
		public int GraphicId { get; set; }
		public int HarvestedCropId { get; set; }
		public int TimeToGrow
		{
			get { return GrowthStages.Sum(); }
		}
		public string CropSuffix { get; set; }

		public SeedItem(int id, string categoryString, List<Seasons> growingSeasons) : base(id)
		{
			IsSeed = true;
			DifficultyToObtain = ObtainingDifficulties.LargeTimeRequirements;
			CategoryString = categoryString;
			GrowingSeasons = growingSeasons;
		}

		/// <summary>
		/// Gets the string that's part of Data/ObjectInformation
		/// </summary>
		/// <returns />
		public string GetObjectInformationString()
		{
			return $"{Name}/{Price}/{CategoryString}/{Name}/{Description}";
		}

		/// <summary>
		/// Gets the string that's part of Data/Crops
		/// </summary>
		/// <returns />
		public string GetCropInfoString()
		{
			string growthStagesString = "";
			foreach (int stageTime in GrowthStages)
			{
				growthStagesString += $"{stageTime} ";
			}
			growthStagesString = growthStagesString.Trim();

			return $"{growthStagesString}/{GetSeasonsString()}/{GraphicId}/{HarvestedCropId}/{CropSuffix}";
		}

		public string GetSeasonsString()
		{
			string seasonsString = "";
			foreach (Seasons season in GrowingSeasons)
			{
				seasonsString += $"{season.ToString()} ";
			}
			return seasonsString.Trim();
		}
	}
}
