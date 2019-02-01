namespace Randomizer
{
	/// <summary>
	/// Represents an item in the game
	/// </summary>
	public class Item
	{
		public int Id { get; }
		public ForagableLocationData ForagableLocationData { get; } = new ForagableLocationData();
		public bool ShouldBeForagable = false;
		public bool IsForagable
		{
			get { return ForagableLocationData.HasData(); }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The item ID</param>
		public Item(int id)
		{
			Id = id;
		}
	}
}
