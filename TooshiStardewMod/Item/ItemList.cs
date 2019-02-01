using System.Collections.Generic;

namespace Randomizer
{
	public class ItemList
	{
		public static Dictionary<int, Item> Items = new Dictionary<int, Item>
		{
			// Spring Foragables - TODO: look into Salmonberries and Spring Onions
			{ (int)ObjectIndexes.WildHorseradish, new Item((int)ObjectIndexes.WildHorseradish) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Daffodil, new Item((int)ObjectIndexes.Daffodil) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Leek, new Item((int)ObjectIndexes.Leek) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Dandelion, new Item((int)ObjectIndexes.Dandelion) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Morel, new Item((int)ObjectIndexes.Morel) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.CommonMushroom, new Item((int)ObjectIndexes.CommonMushroom) { ShouldBeForagable = true } }, // Also fall

			// Summer Foragables
			{ (int)ObjectIndexes.SpiceBerry, new Item((int)ObjectIndexes.SpiceBerry) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Grape, new Item((int)ObjectIndexes.Grape) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.SweetPea, new Item((int)ObjectIndexes.SweetPea) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.RedMushroom, new Item((int)ObjectIndexes.RedMushroom) { ShouldBeForagable = true } }, // Also fall
			{ (int)ObjectIndexes.FiddleHeadFern, new Item((int)ObjectIndexes.FiddleHeadFern) { ShouldBeForagable = true } },

			// Fall Foragables
			{ (int)ObjectIndexes.WildPlum, new Item((int)ObjectIndexes.WildPlum) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Hazelnut, new Item((int)ObjectIndexes.Hazelnut) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Blackberry, new Item((int)ObjectIndexes.Blackberry) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Chanterelle, new Item((int)ObjectIndexes.Chanterelle) { ShouldBeForagable = true } },

			// Winter Foragables
			{ (int)ObjectIndexes.WinterRoot, new Item((int)ObjectIndexes.WinterRoot) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.CrystalFruit, new Item((int)ObjectIndexes.CrystalFruit) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.SnowYam, new Item((int)ObjectIndexes.SnowYam) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Crocus, new Item((int)ObjectIndexes.Crocus) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Holly, new Item((int)ObjectIndexes.Holly) { ShouldBeForagable = true } },

			// Beach Foragables
			{ (int)ObjectIndexes.NautilusShell, new Item((int)ObjectIndexes.NautilusShell) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Coral, new Item((int)ObjectIndexes.Coral) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.SeaUrchin, new Item((int)ObjectIndexes.SeaUrchin) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.RainbowShell, new Item((int)ObjectIndexes.RainbowShell) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Clam, new Item((int)ObjectIndexes.Clam) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Cockle, new Item((int)ObjectIndexes.Cockle) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Mussel, new Item((int)ObjectIndexes.Mussel) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.Oyster, new Item((int)ObjectIndexes.Oyster) { ShouldBeForagable = true } },

			// Desert Forabagles
			{ (int)ObjectIndexes.Coconut, new Item((int)ObjectIndexes.Coconut) { ShouldBeForagable = true } },
			{ (int)ObjectIndexes.CactusFruit, new Item((int)ObjectIndexes.CactusFruit) { ShouldBeForagable = true } }
		};
	}
}
