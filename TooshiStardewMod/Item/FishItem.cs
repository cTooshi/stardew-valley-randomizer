namespace Randomizer
{
	/// <summary>
	/// Represents a fish
	/// </summary>
	public class FishItem : Item
	{
		public FishItem(int id, ObtainingDifficulties difficultyToObtain = ObtainingDifficulties.LargeTimeRequirements) : base(id)
		{
			DifficultyToObtain = difficultyToObtain;
			IsFish = true;
		}
	}
}
