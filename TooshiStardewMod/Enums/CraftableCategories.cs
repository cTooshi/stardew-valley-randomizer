namespace Randomizer
{
	/// <summary>
	/// A list of categories of crafting items
	/// This helps determine what the randomized recipes can possibly be
	/// </summary>
	public enum CraftableCategories
	{
		/// <summary>
		/// Use this for things like paths
		/// </summary>
		CheapAndNeedMany,

		/// <summary>
		/// Use this for things like chests, torches, etc. - things you need early game
		/// </summary>
		Cheap,

		/// <summary>
		/// Use this for more useful things like scarecrows, sprinklers, etc.
		/// </summary>
		Moderate,

		/// <summary>
		/// Use this for things like fertilizer
		/// </summary>
		ModerateAndNeedMany,

		/// <summary>
		/// Use this for not quite endgame items such as quality sprinklers
		/// </summary>
		Difficult,

		/// <summary>
		/// Use this for things like quality fertilizer
		/// </summary>
		DifficulteAndNeedMany,

		/// <summary>
		/// Use this for all endgame things, such as Iridium sprinklers
		/// </summary>
		Endgame

	}
}
