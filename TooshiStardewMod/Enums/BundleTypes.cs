namespace Randomizer
{
	/// <summary>
	/// The types of bundles
	/// The prefix on the name matters!
	/// </summary>
	public enum BundleTypes
	{
		None, // Default

		// Can be used by any room
		AllRandom,

		// Crafting room
		CraftingResource,
		CraftingHappyCrops,
		CraftingTree,
		CraftingTotems,
		CraftingBindle,
		CraftingSpringForaging,
		CraftingSummerForaging,
		CraftingFallForaging,
		CraftingWinterForaging
	}
}
