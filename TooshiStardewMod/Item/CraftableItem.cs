using System.Collections.Generic;

namespace Randomizer
{
	public class CraftableItem : Item
	{
		public List<CraftingMaterialItem> RequiredItemsToCraft { get; } = new List<CraftingMaterialItem>();
		public string Path { get; set; }
		public string SkillString { get; set; }
		public Range LearnableLevels { get; set; } = new Range(1, 1);
		public bool IsLearnedOnLevelup
		{
			get { return SkillString.Length > 0; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The id of the item</param>
		/// <param name="path">The hard-coded path for this craftable item</param>
		/// <param name="skillString">The name of the skill you need to level up to learn the recipe</param>
		public CraftableItem(int id, string path, string skillString = "") : base(id)
		{
			IsCraftable = true;
			Path = path;
			SkillString = skillString;
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

		/// <summary>
		/// Gets the level you learn this skill at
		/// </summary>
		/// <returns>
		/// Any value in the given range. Excludes 0, 5, and 10.
		/// Returns 9 if it's 10; returns 1 if it's 0; returns 4 or 6 if it's 5
		/// </returns>
		public int GetLevelLearnedAt()
		{
			int generatedLevel = LearnableLevels.GetRandomValue();
			if (generatedLevel > 8) { return 9; }
			if (generatedLevel < 1) { return 1; }
			if (generatedLevel == 5)
			{
				return Globals.RNGGetNextBoolean() ? 4 : 6;
			}

			return generatedLevel;
		}

		/// <summary>
		/// Gets the string to be used for the crafting recipe
		/// </summary>
		/// <returns></returns>
		public string GetCraftingString()
		{
			string itemsRequiredString = "18 9"; //TODO: do this part so that it randomly generates this!
			string stringSuffix = IsLearnedOnLevelup ? $"{SkillString} {GetLevelLearnedAt()}" : "";
			string craftingString = $"{itemsRequiredString}{Path}{stringSuffix}";

			Globals.ConsoleWrite($"{Name} crafting string: {craftingString}");
			return craftingString;
		}
	}
}
