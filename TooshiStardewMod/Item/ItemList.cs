using System.Collections.Generic;

namespace Randomizer
{
	public class ItemList
	{
		public static Dictionary<int, Item> Items = new Dictionary<int, Item>
		{
			// Spring Foragables - TODO: look into Salmonberries and Spring Onions
			{ (int)ObjectIndexes.WildHorseradish, new ForagableItem((int)ObjectIndexes.WildHorseradish) },
			{ (int)ObjectIndexes.Daffodil, new ForagableItem((int)ObjectIndexes.Daffodil) },
			{ (int)ObjectIndexes.Leek, new ForagableItem((int)ObjectIndexes.Leek) },
			{ (int)ObjectIndexes.Dandelion, new ForagableItem((int)ObjectIndexes.Dandelion) },
			{ (int)ObjectIndexes.Morel, new ForagableItem((int)ObjectIndexes.Morel) },
			{ (int)ObjectIndexes.CommonMushroom, new ForagableItem((int)ObjectIndexes.CommonMushroom) }, // Also fall

			// Summer Foragables
			{ (int)ObjectIndexes.SpiceBerry, new ForagableItem((int)ObjectIndexes.SpiceBerry) },
			{ (int)ObjectIndexes.Grape, new ForagableItem((int)ObjectIndexes.Grape) },
			{ (int)ObjectIndexes.SweetPea, new ForagableItem((int)ObjectIndexes.SweetPea) },
			{ (int)ObjectIndexes.RedMushroom, new ForagableItem((int)ObjectIndexes.RedMushroom) }, // Also fall
			{ (int)ObjectIndexes.FiddleheadFern, new ForagableItem((int)ObjectIndexes.FiddleheadFern) },

			// Fall Foragables
			{ (int)ObjectIndexes.WildPlum, new ForagableItem((int)ObjectIndexes.WildPlum) },
			{ (int)ObjectIndexes.Hazelnut, new ForagableItem((int)ObjectIndexes.Hazelnut) },
			{ (int)ObjectIndexes.Blackberry, new ForagableItem((int)ObjectIndexes.Blackberry) },
			{ (int)ObjectIndexes.Chanterelle, new ForagableItem((int)ObjectIndexes.Chanterelle) },

			// Winter Foragables
			{ (int)ObjectIndexes.WinterRoot, new ForagableItem((int)ObjectIndexes.WinterRoot) },
			{ (int)ObjectIndexes.CrystalFruit, new ForagableItem((int)ObjectIndexes.CrystalFruit) },
			{ (int)ObjectIndexes.SnowYam, new ForagableItem((int)ObjectIndexes.SnowYam) },
			{ (int)ObjectIndexes.Crocus, new ForagableItem((int)ObjectIndexes.Crocus) },
			{ (int)ObjectIndexes.Holly, new ForagableItem((int)ObjectIndexes.Holly) },

			// Beach Foragables
			{ (int)ObjectIndexes.NautilusShell, new ForagableItem((int)ObjectIndexes.NautilusShell) },
			{ (int)ObjectIndexes.Coral, new ForagableItem((int)ObjectIndexes.Coral) },
			{ (int)ObjectIndexes.SeaUrchin, new ForagableItem((int)ObjectIndexes.SeaUrchin) },
			{ (int)ObjectIndexes.RainbowShell, new ForagableItem((int)ObjectIndexes.RainbowShell) },
			{ (int)ObjectIndexes.Clam, new ForagableItem((int)ObjectIndexes.Clam) },
			{ (int)ObjectIndexes.Cockle, new ForagableItem((int)ObjectIndexes.Cockle) },
			{ (int)ObjectIndexes.Mussel, new ForagableItem((int)ObjectIndexes.Mussel) },
			{ (int)ObjectIndexes.Oyster, new ForagableItem((int)ObjectIndexes.Oyster) },

			// Desert Forabagles
			{ (int)ObjectIndexes.Coconut, new ForagableItem((int)ObjectIndexes.Coconut) },
			{ (int)ObjectIndexes.CactusFruit, new ForagableItem((int)ObjectIndexes.CactusFruit) }
		};
	}
}
