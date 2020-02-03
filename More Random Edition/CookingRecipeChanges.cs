using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Reflection;

namespace Randomizer
{
	public class CookingRecipeChanges
	{
		/// <summary>
		/// A mapping of cooking recipes that include crop names to the id of that crop
		/// </summary>
		private static Dictionary<string, int> CropDishesMap = new Dictionary<string, int>
		{
			{ "Cheese Cauli.", (int)ObjectIndexes.CheeseCauliflower },
			{ "Parsnip Soup", (int)ObjectIndexes.ParsnipSoup },
			{ "Bean Hotpot", (int)ObjectIndexes.BeanHotpot },
			{ "Glazed Yams", (int)ObjectIndexes.GlazedYams },
			{ "Pepper Poppers", (int)ObjectIndexes.PepperPoppers },
			{ "Rhubarb Pie", (int)ObjectIndexes.RhubarbPie },
			{ "Eggplant Parm.", (int)ObjectIndexes.EggplantParmesan },
			{ "Blueberry Tart", (int)ObjectIndexes.BlueberryTart },
			{ "Pumpkin Soup", (int)ObjectIndexes.PumpkinSoup },
			{ "Cran. Sauce", (int)ObjectIndexes.CranberrySauce },
			{ "Pumpkin Pie", (int)ObjectIndexes.PumpkinPie },
			{ "Radish Salad", (int)ObjectIndexes.RadishSalad },
			{ "Cranberry Candy", (int)ObjectIndexes.CranberryCandy },
			{ "Artichoke Dip", (int)ObjectIndexes.ArtichokeDip },
			{ "Rice Pudding", (int)ObjectIndexes.RicePudding },
			{ "Fruit Salad", (int)ObjectIndexes.FruitSalad }
		};

		/// <summary>
		/// A mapping of cooking recipes that include fish names to the id of that fish
		/// </summary>
		private static Dictionary<string, int> FishDishesMap = new Dictionary<string, int>
		{
			{ "Carp Surprise", (int)ObjectIndexes.CarpSurprise },
			{ "Salmon Dinner", (int)ObjectIndexes.SalmonDinner },
			{ "Crispy Bass", (int)ObjectIndexes.CrispyBass },
			{ "Trout Soup", (int)ObjectIndexes.TroutSoup },
			{ "Fried Eel", (int)ObjectIndexes.FriedEel },
			{ "Spicy Eel", (int)ObjectIndexes.SpicyEel }
		};

		/// <summary>
		/// Fixes the cooking recipe values if they were changed
		/// Called during the RenderingActiveMenu event if fish or crops are randomized
		/// </summary>
		public static void FixCookingRecipeHoverText()
		{
			IClickableMenu genericMenu = Game1.activeClickableMenu;
			if (genericMenu == null || !(genericMenu is CraftingPage)) { return; }

			CraftingPage craftingMenu = (CraftingPage)genericMenu;
			if (!(bool)GetInstanceField(craftingMenu, "cooking")) { return; }

			List<Dictionary<ClickableTextureComponent, CraftingRecipe>> pagesOfCraftingRecipes =
				(List<Dictionary<ClickableTextureComponent, CraftingRecipe>>)GetInstanceField(craftingMenu, "pagesOfCraftingRecipes");

			foreach (Dictionary<ClickableTextureComponent, CraftingRecipe> page in pagesOfCraftingRecipes)
			{
				foreach (ClickableTextureComponent key in page.Keys)
				{
					CraftingRecipe recipe = page[key];
					FixFishDish(recipe);
					FixCropDish(recipe);
				}
			}
		}

		/// <summary>
		/// Replaces the display name of the given recipe with the renamed fish dish
		/// </summary>
		/// <param name="recipe">The recipe</param>
		private static void FixFishDish(CraftingRecipe recipe)
		{
			if (!Globals.Config.RandomizeFish) { return; }

			if (FishDishesMap.ContainsKey(recipe.DisplayName))
			{
				recipe.DisplayName = ItemList.GetItemName(FishDishesMap[recipe.DisplayName]);
			}
		}

		/// <summary>
		/// Replaces the display name of the given recipe with the renamed crop dish
		/// </summary>
		/// <param name="recipe">The recipe</param>
		private static void FixCropDish(CraftingRecipe recipe)
		{
			if (!Globals.Config.RandomizeCrops) { return; }

			if (CropDishesMap.ContainsKey(recipe.DisplayName))
			{
				recipe.DisplayName = ItemList.GetItemName(CropDishesMap[recipe.DisplayName]);
			}
		}

		/// <summary>
		/// Gets the instance value of a field, even if it's private
		/// USE SPARANGLY, THIS KIND OF PRACTICE IS EXTREMELY FROWNED UPON
		/// Credit goes here: https://stackoverflow.com/questions/3303126/how-to-get-the-value-of-private-field-in-c
		/// </summary>
		/// <typeparam name="T">The type</typeparam>
		/// <param name="instance">The instance</param>
		/// <param name="fieldName">The field name to get</param>
		/// <returns>The retrieved field</returns>
		private static object GetInstanceField<T>(T instance, string fieldName)
		{
			BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo field = typeof(T).GetField(fieldName, bindFlags);
			return field.GetValue(instance);
		}
	}
}