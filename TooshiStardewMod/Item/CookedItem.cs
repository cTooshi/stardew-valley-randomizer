namespace Randomizer
{
	/// <summary>
	/// Represents an item you make in your kitchen
	/// </summary>
	public class CookedItem : Item
	{
		public CookedItem(int id) : base(id)
		{
			IsCooked = true;
			DifficultyToObtain = ObtainingDifficulties.LargeTimeRequirements;
		}
	}
}
