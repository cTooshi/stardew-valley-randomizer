using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Reflection;
using SVItem = StardewValley.Item;
using SVObject = StardewValley.Object;

namespace Randomizer
{
	/// <summary>
	/// Represents an overridden seed shop - it's used to override the normal
	/// things the shop can sell. In this case, it will make the fruit tree
	/// prices NOT hard-coded
	/// </summary>
	public class OverriddenSeedShop
	{
		/// <summary>
		/// A copy of the original "shopStock" function in SeedShop.cs, with the fruit tree
		/// prices not hard-coded. There was a code snippet that was likely intended to be used
		/// as a buyback feature, but it didn't seem to work, so it's been taken out for now.
		/// </summary>
		/// <returns></returns>
		public static List<SVItem> NewShopStock()
		{
			List<SVItem> shopStock = new List<SVItem>();
			if (Game1.currentSeason.Equals("spring"))
			{
				shopStock.Add(new SVObject(Vector2.Zero, 472, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 473, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 474, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 475, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 427, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 477, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 429, int.MaxValue));
				if (Game1.year > 1)
					shopStock.Add(new SVObject(Vector2.Zero, 476, int.MaxValue));
			}
			if (Game1.currentSeason.Equals("summer"))
			{
				shopStock.Add(new SVObject(Vector2.Zero, 479, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 480, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 481, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 482, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 483, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 484, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 453, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 455, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 302, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 487, int.MaxValue));
				shopStock.Add(new SVObject(431, int.MaxValue, false, 100, 0));
				if (Game1.year > 1)
					shopStock.Add(new SVObject(Vector2.Zero, 485, int.MaxValue));
			}
			if (Game1.currentSeason.Equals("fall"))
			{
				shopStock.Add(new SVObject(Vector2.Zero, 490, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 487, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 488, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 491, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 492, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 493, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 483, int.MaxValue));
				shopStock.Add(new SVObject(431, int.MaxValue, false, 100, 0));
				shopStock.Add(new SVObject(Vector2.Zero, 425, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 299, int.MaxValue));
				shopStock.Add(new SVObject(Vector2.Zero, 301, int.MaxValue));
				if (Game1.year > 1)
					shopStock.Add(new SVObject(Vector2.Zero, 489, int.MaxValue));
			}
			shopStock.Add(new SVObject(Vector2.Zero, 297, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, 245, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, 246, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, 423, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, 247, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, 419, int.MaxValue));
			if ((int)Game1.stats.DaysPlayed >= 15)
			{
				shopStock.Add(new SVObject(368, int.MaxValue, false, 50, 0));
				shopStock.Add(new SVObject(370, int.MaxValue, false, 50, 0));
				shopStock.Add(new SVObject(465, int.MaxValue, false, 50, 0));
			}
			if (Game1.year > 1)
			{
				shopStock.Add(new SVObject(369, int.MaxValue, false, 75, 0));
				shopStock.Add(new SVObject(371, int.MaxValue, false, 75, 0));
				shopStock.Add(new SVObject(466, int.MaxValue, false, 75, 0));
			}
			Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
			int which = random.Next(112);
			if (which == 21)
				which = 36;
			List<SVItem> objList2 = shopStock;
			Wallpaper wallpaper1 = new Wallpaper(which, false);
			wallpaper1.Stack = int.MaxValue;
			objList2.Add(wallpaper1);
			List<SVItem> objList3 = shopStock;
			Wallpaper wallpaper2 = new Wallpaper(random.Next(40), true);
			wallpaper2.Stack = int.MaxValue;
			objList3.Add(wallpaper2);
			List<SVItem> objList4 = shopStock;
			Furniture furniture = new Furniture(1308, Vector2.Zero);
			furniture.Stack = int.MaxValue;
			objList4.Add(furniture);

			shopStock.Add(new SVObject(Vector2.Zero, (int)ObjectIndexes.CherrySapling, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, (int)ObjectIndexes.ApricotSapling, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, (int)ObjectIndexes.OrangeSapling, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, (int)ObjectIndexes.PeachSapling, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, (int)ObjectIndexes.PomegranateSapling, int.MaxValue));
			shopStock.Add(new SVObject(Vector2.Zero, (int)ObjectIndexes.AppleSapling, int.MaxValue));

			if (Game1.player.hasAFriendWithHeartLevel(8, true))
				shopStock.Add(new SVObject(Vector2.Zero, 458, int.MaxValue));

			return shopStock;
		}

		/// <summary>
		/// Replaces the shopStock method in SeedShop.cs with this file's NewShopStock method
		/// NOTE: THIS IS UNSAFE CODE, CHANGE WITH EXTREME CAUTION
		/// </summary>
		public static void ReplaceShopStockMethod()
		{
			MethodInfo methodToReplace = typeof(SeedShop).GetMethod("shopStock");
			MethodInfo methodToInject = typeof(OverriddenSeedShop).GetMethod("NewShopStock");
			Globals.RepointMethod(methodToReplace, methodToInject);
		}
	}
}
