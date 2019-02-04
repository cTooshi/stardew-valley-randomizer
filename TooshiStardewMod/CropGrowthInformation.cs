using System.Collections.Generic;

namespace Randomizer
{
	/// <summary>
	/// Contains the information in Data/Crops to randomly distribute
	/// </summary>
	public class CropGrowthInformation
	{
		public int NumberOfGrowthStages { get; set; }
		public int GraphicId { get; set; }
		public int OriginalCropId { get; set; }
		public string CropSuffix { get; set; }

		public CropGrowthInformation(int numberOfGrowthStages, int graphicId, int originalCropId, string cropSuffix)
		{
			NumberOfGrowthStages = numberOfGrowthStages;
			GraphicId = graphicId;
			OriginalCropId = originalCropId;
			CropSuffix = cropSuffix;
		}

		public static List<CropGrowthInformation> All = new List<CropGrowthInformation>
		{
			new CropGrowthInformation(4, 0, (int)ObjectIndexes.Parsnip, "/-1/0/false/false/false"),
			//FINISHME
		};
	}
}
