namespace Randomizer
{
	/// <summary>
	/// Represents a crop
	/// </summary>
	public class CropItem : Item
	{
		public CropItem(int id) : base(id)
		{
			IsCrop = true;
			DifficultyToObtain = ObtainingDifficulties.LargeTimeRequirements;
		}
	}
}
