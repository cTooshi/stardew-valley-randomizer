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
		AllLetter,

		// Crafting room
		CraftingResource,
		CraftingHappyCrops,
		CraftingTree,
		CraftingTotems,
		CraftingBindle,
		CraftingSpringForaging,
		CraftingSummerForaging,
		CraftingFallForaging,
		CraftingWinterForaging,

		//Pantry bundles //TODO - uncomment when possible
		PantryAnimal,
		//PantryQualityCrops,
		PantryQualityForagables,
		PantryCooked,
		//PantryFlower,
		//PantrySpringCrops,
		//PantrySummerCrops,
		//PantryFallCrops,
		//PantryWinterCrops,
		PantryEgg,
		//PantryRareFoods,
		PantryDesert,
		PantryDessert,
		PantryMexicanFood
	}
}
