namespace Randomizer
{
	/// <summary>
	/// Used to track how many of an item might be required for something
	/// </summary>
	public class RequiredItem
	{
		public Item Item { get; set; }
		public int NumberOfItems { get; set; }
		public ItemQualities MinimumQuality { get; set; } = ItemQualities.Normal;
		private Range _rangeOfItems { get; set; }

		/// <summary>
		/// The amount of money required - setting this will mean that the item
		/// will NOT be used in any bundle string!
		/// </summary>
		public int MoneyAmount { get; set; } = -1;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="requiredItem">The item that's required</param>
		/// <param name="minValue">The max number of items required to craft this</param>
		/// <param name="maxValue">The minimum number of items required to craft this</param>
		public RequiredItem(Item requiredItem, int minValue, int maxValue)
		{
			Item = requiredItem;
			_rangeOfItems = new Range(minValue, maxValue);
			NumberOfItems = _rangeOfItems.GetRandomValue();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="requiredItem">The item that's required</param>
		/// <param name="numberOfItems">The number of items required to craft this</param>
		public RequiredItem(Item requiredItem, int numberOfItems)
		{
			Item = requiredItem;
			_rangeOfItems = new Range(numberOfItems, numberOfItems);
			NumberOfItems = _rangeOfItems.GetRandomValue();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="itemId">The item id of the item that's required</param>
		/// <param name="minValue">The max number of items required to craft this</param>
		/// <param name="maxValue">The minimum number of items required to craft this</param>
		public RequiredItem(int itemId, int minValue, int maxValue)
		{
			Item = ItemList.Items[itemId];
			_rangeOfItems = new Range(minValue, maxValue);
			NumberOfItems = _rangeOfItems.GetRandomValue();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="itemId">The item id of the item that's required</param>
		/// <param name="numberOfItems">The number of items required to craft this</param>
		public RequiredItem(int itemId, int numberOfItems)
		{
			Item = ItemList.Items[itemId];
			_rangeOfItems = new Range(numberOfItems, numberOfItems);
			NumberOfItems = _rangeOfItems.GetRandomValue();
		}

		/// <summary>
		/// Gets the string used for bundles
		/// </summary>
		/// <returns />
		public string GetStringForBundles()
		{
			if (MoneyAmount >= 0)
			{
				return $"-1 {MoneyAmount} {MoneyAmount}";
			}

			int numberOfItems = Item.IsRing ? 1 : NumberOfItems; // Rings cannot stack
			return $"{Item.Id} {numberOfItems} {(int)MinimumQuality}";
		}
	}
}
