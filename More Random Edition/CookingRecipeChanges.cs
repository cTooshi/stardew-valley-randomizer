using StardewValley;
using StardewValley.Menus;
using System.Reflection;

namespace Randomizer
{
	public class CookingRecipeChanges
	{
		public static void FixCookingRecipeHoverText()
		{
			IClickableMenu menu = Game1.activeClickableMenu;
			if (menu == null || !(menu is CraftingPage)) { return; }

			CraftingPage test = (CraftingPage)menu;

			if (!(bool)GetInstanceField(test, "cooking")) { return; }

			CraftingRecipe recipe = (CraftingRecipe)GetInstanceField(test, "hoverRecipe");
			if (recipe != null)
			{
				//TODO: create & check a dictionary of recipe names to item ids
				if (recipe.DisplayName == "Carp Surprise")
				{
					recipe.DisplayName = ItemList.GetItemName((int)ObjectIndexes.CarpSurprise);
				}
			}

		}

		private static object GetInstanceField<T>(T instance, string fieldName)
		{
			BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo field = typeof(T).GetField(fieldName, bindFlags);
			return field.GetValue(instance);
		}
	}
}

//ItemList.Items[(int)ObjectIndexes.CheeseCauliflower].OverrideName = $"Cheese {cauliflower}";
//			ItemList.Items[(int)ObjectIndexes.ParsnipSoup].OverrideName = $"{parsnip} Soup";
//			ItemList.Items[(int)ObjectIndexes.BeanHotpot].OverrideName = $"{greenbean} Hotpot";
//			ItemList.Items[(int)ObjectIndexes.GlazedYams].OverrideName = $"Glazed {yam} Platter";
//			ItemList.Items[(int)ObjectIndexes.PepperPoppers].OverrideName = $"{hotpepper} Poppers";
//			ItemList.Items[(int)ObjectIndexes.RhubarbPie].OverrideName = $"{rhubarb} Pie";
//			ItemList.Items[(int)ObjectIndexes.EggplantParmesan].OverrideName = $"{eggplant} Parmesan";
//			ItemList.Items[(int)ObjectIndexes.BlueberryTart].OverrideName = $"{blueberry} Tart";
//			ItemList.Items[(int)ObjectIndexes.PumpkinSoup].OverrideName = $"{pumpkin} Soup";
//			ItemList.Items[(int)ObjectIndexes.CranberrySauce].OverrideName = $"{cranberry} Sauce";
//			ItemList.Items[(int)ObjectIndexes.PumpkinPie].OverrideName = $"{pumpkin} Pie";
//			ItemList.Items[(int)ObjectIndexes.RadishSalad].OverrideName = $"{radish} Salad";
//			ItemList.Items[(int)ObjectIndexes.CranberryCandy].OverrideName = $"{cranberry} Candy";
//			ItemList.Items[(int)ObjectIndexes.PoppyseedMuffin].OverrideName = $"{poppyseed} Muffin";
//			ItemList.Items[(int)ObjectIndexes.ArtichokeDip].OverrideName = $"{artichoke} Dip";
//			ItemList.Items[(int)ObjectIndexes.FruitSalad].OverrideName = "Harvest Salad";
//			ItemList.Items[(int)ObjectIndexes.RicePudding].OverrideName = $"{rice} Pudding";

//			ItemList.Items[(int)ObjectIndexes.CarpSurprise].OverrideName = $"{carp} Surprise";
//			ItemList.Items[(int)ObjectIndexes.SalmonDinner].OverrideName = $"{salmon} Dinner";
//			ItemList.Items[(int)ObjectIndexes.CrispyBass].OverrideName = $"Crispy {bass}";
//			ItemList.Items[(int)ObjectIndexes.TroutSoup].OverrideName = $"{trout} Soup";
//			ItemList.Items[(int)ObjectIndexes.FriedEel].OverrideName = $"Fried {eel}";
//			ItemList.Items[(int)ObjectIndexes.SpicyEel].OverrideName = $"Spicy {eel}";