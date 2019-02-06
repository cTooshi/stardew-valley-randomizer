using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class BoilerRoomBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Populates the bundle with the name, required items, minimum required, and color
		/// </summary>
		protected override void Populate()
		{
			BundleType = Globals.RNGGetAndRemoveRandomValueFromList(RoomBundleTypes);
			List<RequiredItem> potentialItems = new List<RequiredItem>();

			switch (BundleType)
			{
				case BundleTypes.BoilerArtifacts:
					Name = "Artifact";
					potentialItems = RequiredItem.CreateList(
						ItemList.GetArtifacts().Where(x => x.DifficultyToObtain < ObtainingDifficulties.RareItem).ToList()
					);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = 3;
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BoilerMinerals:
					Name = "Mineral";
					potentialItems = RequiredItem.CreateList(ItemList.GetGeodeMinerals());
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BoilerGeode:
					Name = "Geode";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.Geode, 1, 10),
						new RequiredItem((int)ObjectIndexes.FrozenGeode, 1, 10),
						new RequiredItem((int)ObjectIndexes.MagmaGeode, 1, 10),
						new RequiredItem((int)ObjectIndexes.OmniGeode, 1, 10),
					};
					Color = BundleColors.Red;
					break;
				case BundleTypes.BoilerGemstone:
					Name = "Gemstone";
					potentialItems = RequiredItem.CreateList(new List<int>
					{
						(int)ObjectIndexes.Quartz,
						(int)ObjectIndexes.FireQuartz,
						(int)ObjectIndexes.EarthCrystal,
						(int)ObjectIndexes.FrozenTear,
						(int)ObjectIndexes.Aquamarine,
						(int)ObjectIndexes.Amethyst,
						(int)ObjectIndexes.Emerald,
						(int)ObjectIndexes.Ruby,
						(int)ObjectIndexes.Topaz,
						(int)ObjectIndexes.Jade,
						(int)ObjectIndexes.Diamond
					}, 1, 5);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 2, RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BoilerMetal:
					Name = "Metal";
					potentialItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.CopperOre, 5, 10),
						new RequiredItem((int)ObjectIndexes.IronOre, 5, 10),
						new RequiredItem((int)ObjectIndexes.GoldOre, 5, 10),
						new RequiredItem((int)ObjectIndexes.IridiumOre, 5, 10),
						new RequiredItem((int)ObjectIndexes.CopperBar, 1, 5),
						new RequiredItem((int)ObjectIndexes.IronBar, 1, 5),
						new RequiredItem((int)ObjectIndexes.GoldBar, 1, 5),
						new RequiredItem((int)ObjectIndexes.IridiumBar, 1, 5),
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(6, 8));
					MinimumRequiredItems = Range.GetRandomValue(RequiredItems.Count - 2, RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BoilerExplosive:
					Name = "Explosive";
					RequiredItems = new List<RequiredItem>
					{
						new RequiredItem((int)ObjectIndexes.CherryBomb, 1, 5),
						new RequiredItem((int)ObjectIndexes.Bomb, 1, 5),
						new RequiredItem((int)ObjectIndexes.MegaBomb, 1, 5),
					};
					Color = BundleColors.Red;
					break;
				case BundleTypes.BoilerRing:
					Name = "Ring";
					RequiredItems = RequiredItem.CreateList(
						Globals.RNGGetRandomValuesFromList(ItemList.GetRings(), 8)
					);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BoilerSpoopy:
					Name = "Spoopy";
					potentialItems = new List<RequiredItem>
					{
						//new RequiredItem((int)ObjectIndexes.Pumpkin, 6), //TODO: bring back when crops are done
						new RequiredItem((int)ObjectIndexes.JackOLantern, 6),
						new RequiredItem((int)ObjectIndexes.Ghostfish, 6),
						new RequiredItem((int)ObjectIndexes.BatWing, 6),
						new RequiredItem((int)ObjectIndexes.VoidEssence, 6),
						new RequiredItem((int)ObjectIndexes.VoidEgg, 6),
						new RequiredItem((int)ObjectIndexes.PurpleMushroom, 6),
						new RequiredItem((int)ObjectIndexes.GhostCrystal, 6),
						new RequiredItem((int)ObjectIndexes.SpookFish, 6)
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 6);
					MinimumRequiredItems = 3;
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BoilerMonster:
					Name = "Monster";
					RequiredItems = new List<RequiredItem>()
					{
						new RequiredItem((int)ObjectIndexes.BugMeat, 10, 50),
						new RequiredItem((int)ObjectIndexes.Slime, 10, 50),
						new RequiredItem((int)ObjectIndexes.BatWing, 10, 50),
						new RequiredItem((int)ObjectIndexes.SolarEssence, 10, 50),
						new RequiredItem((int)ObjectIndexes.VoidEssence, 10, 50)
					};
					Color = BundleColors.Red;
					break;
			}
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			RequiredItem randomOre = Globals.RNGGetRandomValueFromList(new List<RequiredItem>()
			{
				new RequiredItem((int)ObjectIndexes.CopperOre, 100),
				new RequiredItem((int)ObjectIndexes.IronOre, 100),
				new RequiredItem((int)ObjectIndexes.GoldOre, 100),
				new RequiredItem((int)ObjectIndexes.IridiumOre, 10),
			});

			RequiredItem randomBar = Globals.RNGGetRandomValueFromList(new List<RequiredItem>()
			{
				new RequiredItem((int)ObjectIndexes.CopperBar, 15),
				new RequiredItem((int)ObjectIndexes.IronBar, 15),
				new RequiredItem((int)ObjectIndexes.GoldBar, 15),
				new RequiredItem((int)ObjectIndexes.IridiumBar)
			});

			RequiredItem randomGeode = Globals.RNGGetRandomValueFromList(new List<RequiredItem>()
			{
				new RequiredItem((int)ObjectIndexes.Geode, 25),
				new RequiredItem((int)ObjectIndexes.FrozenGeode, 25),
				new RequiredItem((int)ObjectIndexes.MagmaGeode, 25),
				new RequiredItem((int)ObjectIndexes.OmniGeode, 25)
			});

			RequiredItem randomMonsterDrop = Globals.RNGGetRandomValueFromList(new List<RequiredItem>()
			{
				new RequiredItem((int)ObjectIndexes.BugMeat, 200),
				new RequiredItem((int)ObjectIndexes.Slime, 150),
				new RequiredItem((int)ObjectIndexes.BatWing, 100),
				new RequiredItem((int)ObjectIndexes.SolarEssence, 50),
				new RequiredItem((int)ObjectIndexes.VoidEssence, 50)
			});

			RequiredItem randomExplosive = Globals.RNGGetRandomValueFromList(new List<RequiredItem>()
			{
				new RequiredItem((int)ObjectIndexes.CherryBomb, 25, 50),
				new RequiredItem((int)ObjectIndexes.Bomb, 25, 50),
				new RequiredItem((int)ObjectIndexes.MegaBomb, 25, 50)
			});

			RequiredItem randomGemstone = Globals.RNGGetRandomValueFromList(new List<RequiredItem>()
			{
				new RequiredItem((int)ObjectIndexes.Quartz, 25, 50),
				new RequiredItem((int)ObjectIndexes.FireQuartz, 25, 50),
				new RequiredItem((int)ObjectIndexes.EarthCrystal, 25, 50),
				new RequiredItem((int)ObjectIndexes.FrozenTear, 25, 50),
				new RequiredItem((int)ObjectIndexes.Aquamarine, 25, 50),
				new RequiredItem((int)ObjectIndexes.Amethyst, 25, 50),
				new RequiredItem((int)ObjectIndexes.Emerald, 25, 50),
				new RequiredItem((int)ObjectIndexes.Ruby, 25, 50),
				new RequiredItem((int)ObjectIndexes.Topaz, 25, 50),
				new RequiredItem((int)ObjectIndexes.Jade, 25, 50),
				new RequiredItem((int)ObjectIndexes.Diamond, 10, 30),
			});

			var potentialRewards = new List<RequiredItem>
			{
				randomOre,
				randomBar,
				randomGeode,
				randomMonsterDrop,
				randomExplosive,
				randomGemstone,
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetGeodeMinerals()), 25),
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetArtifacts())),
				new RequiredItem(Globals.RNGGetRandomValueFromList(ItemList.GetRings())),
				new RequiredItem((int)ObjectIndexes.Crystalarium),
				new RequiredItem((int)ObjectIndexes.MayonnaiseMachine),
				new RequiredItem((int)ObjectIndexes.Coal, 100)
			};

			if (Globals.RNGGetNextBoolean(1)) // 1% chance of a prismatic shard reward
			{
				Reward = new RequiredItem((int)ObjectIndexes.PrismaticShard);
			}

			else
			{
				Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
			}
		}
	}
}
