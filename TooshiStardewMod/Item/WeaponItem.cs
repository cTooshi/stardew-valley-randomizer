namespace Randomizer
{
	/// <summary>
	/// Represents a weapon - doesn't use the same IDs as normal items, but we'll treat it like an item
	/// </summary>
	public class WeaponItem : Item
	{
		public string Description { get; set; }
		public Range Damage { get; set; }
		public double Knockback { get; set; }
		public int Speed { get; set; }
		public int AddedPrecision { get; set; }
		public int AddedDefense { get; set; }
		public WeaponType Type { get; set; }
		public int BaseMineLevelDrop { get; set; }
		public int MinMineLevelDrop { get; set; }
		public int AddedAOE { get; set; }
		public double CritChance { get; set; }
		public double CritMultiplier { get; set; }

		public WeaponItem(int id) : base(id)
		{
			DifficultyToObtain = ObtainingDifficulties.NonCraftingItem;
			CanStack = false;
			ShouldBeForagable = false;
		}

		public override string ToString()
		{
			return $"{Name}/{Description}/{Damage.MinValue}/{Damage.MaxValue}/{Knockback}/{Speed}/{AddedPrecision}/{AddedDefense}/{(int)Type}/{BaseMineLevelDrop}/{MinMineLevelDrop}/{AddedAOE}/{CritChance}/{CritMultiplier}";
		}
	}
}
