namespace Randomizer
{
	public class ModConfig
	{
		public bool CreateSpoilerLog { get; set; } = true;

		public bool RandomizeFish { get; set; } = true;
		public bool RandomizeForagables { get; set; } = true;
		public bool AddRandomArtifactItem { get; set; } = true;

		public bool RandomizeCraftingRecipes { get; set; } = true;
		public bool RandomizeCraftingRecipeLevels_Needs_Above_Setting_On { get; set; } = true;

		public bool RandomizeWeapons { get; set; } = true;
		public bool RandomizeBundles { get; set; } = true;
		public bool RandomizeBuildingCosts { get; set; } = true;

		public bool RandomizeCrops { get; set; } = true;
		public bool RandomizeFruitTrees { get; set; } = true;

		public bool RandomizeAnimalSkins { get; set; } = true;
		public bool RandomizeNPCSkins { get; set; } = false;

		public bool RandomizeIntroStory { get; set; } = true;
		public bool RandomizeQuests { get; set; } = true;

		public bool RandomizeRain { get; set; } = true;
		public bool RandomizeMusic { get; set; } = true;

		public bool RandomizeMineLayouts_May_Cause_Crashes { get; set; } = false;
	}
}
