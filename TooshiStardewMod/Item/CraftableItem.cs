using System.Collections.Generic;

namespace Randomizer
{
	public class CraftableItem : Item
	{
		public List<CraftingMaterialItem> RequiredItemsToCraft { get; } = new List<CraftingMaterialItem>();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The id of the item</param>
		public CraftableItem(int id) : base(id)
		{
			IsCraftable = true;
		}

		/// <summary>
		/// Adds the given crafting material information to the items required to craft this item
		/// </summary>
		/// <param name="item">The item required</param>
		/// <param name="minValue">The minimum amount required</param>
		/// <param name="maxValue">The maximum amount required</param>
		public void AddCraftingMaterial(Item item, int minValue, int maxValue)
		{
			RequiredItemsToCraft.Add(new CraftingMaterialItem(item, minValue, maxValue));
		}
	}
}
