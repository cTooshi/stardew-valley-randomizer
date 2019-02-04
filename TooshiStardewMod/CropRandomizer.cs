using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Randomizes fruit trees
	/// </summary>
	public class CropRandomizer
	{
		public static EditedObjectInformation Randomize()
		{
			EditedObjectInformation editedObjectInfo = new EditedObjectInformation();
			RandomizeFruitTrees(editedObjectInfo);
			RandomizeCrops(editedObjectInfo);
			return editedObjectInfo;
		}

		/// <summary>
		/// Randomize fruit tree information
		/// </summary>
		/// <param name="editedObjectInfo">The edited object information</param>
		private static void RandomizeFruitTrees(EditedObjectInformation editedObjectInfo)
		{
			List<Item> allPotentialTrees = ItemList.Items.Values.Where(x =>
				x.DifficultyToObtain != ObtainingDifficulties.Impossible
			).ToList();

			Item tree1 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree2 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree3 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree4 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree5 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);
			Item tree6 = Globals.RNGGetAndRemoveRandomValueFromList(allPotentialTrees);

			string[] seasons = { "spring", "spring", "summer", "summer", "fall", "fall" };
			seasons[Globals.RNG.Next(0, 6)] = "winter";

			// TODO: These prices don't actually seem to work for fruit trees
			int[] prices =
			{
				tree1.GetPriceForObtainingDifficulty(0.2),
				tree2.GetPriceForObtainingDifficulty(0.2),
				tree3.GetPriceForObtainingDifficulty(0.2),
				tree4.GetPriceForObtainingDifficulty(0.2),
				tree5.GetPriceForObtainingDifficulty(0.2),
				tree6.GetPriceForObtainingDifficulty(0.2)
			};

			// Fruit tree asset replacements
			var fruitTreeReplacements = new Dictionary<int, string>
			{
				{ (int)ObjectIndexes.CherrySapling, $"0/{seasons[0]}/{tree1.Id}/{prices[0]}" },
				{ (int)ObjectIndexes.ApricotSapling, $"1/{seasons[1]}/{tree2.Id}/{prices[1]}"},
				{ (int)ObjectIndexes.OrangeSapling, $"2/{seasons[2]}/{tree3.Id}/{prices[2]}"},
				{ (int)ObjectIndexes.PeachSapling, $"3/{seasons[3]}/{tree4.Id}/{prices[3]}"},
				{ (int)ObjectIndexes.PomegranateSapling, $"4/{seasons[4]}/{tree5.Id}/{prices[4]}"},
				{ (int)ObjectIndexes.AppleSapling, $"5/{seasons[5]}/{tree6.Id}/{prices[5]}"},
			};

			foreach (KeyValuePair<int, string> pair in fruitTreeReplacements)
			{
				editedObjectInfo.FruitTreeReplacements[pair.Key] = pair.Value;
			}

			// Fruit tree item/shop info replacements
			Random rng = Globals.RNG;
			var objectReplacements = new Dictionary<int, string>
			{
				{ (int)ObjectIndexes.CherrySapling, $"{tree1.Name} Sapling/{prices[0] / 2}/-300/Basic -74/{tree1.Name} Sapling/Takes 28 days to produce a mature {tree1.Name} tree. Bears item in the {seasons[0]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.ApricotSapling, $"{tree2.Name} Sapling/{prices[1] / 2}/-300/Basic -74/{tree2.Name} Sapling/Takes 28 days to produce a mature {tree2.Name} tree. Bears item in the {seasons[1]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.OrangeSapling, $"{tree3.Name} Sapling/{prices[2] / 2}/-300/Basic -74/{tree3.Name} Sapling/Takes 28 days to produce a mature {tree3.Name} tree. Bears item in the {seasons[2]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.PeachSapling, $"{tree4.Name} Sapling/{prices[3] / 2}/-300/Basic -74/{tree4.Name} Sapling/Takes 28 days to produce a mature {tree4.Name} tree. Bears item in the {seasons[3]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.AppleSapling, $"{tree5.Name} Sapling/{prices[4] / 2}/-300/Basic -74/{tree5.Name} Sapling/Takes 28 days to produce a mature {tree5.Name} tree. Bears item in the {seasons[4]}. Only grows if the 8 surrounding \"tiles\" are empty."},
				{ (int)ObjectIndexes.PomegranateSapling, $"{tree6.Name} Sapling/{prices[5] / 2}/-300/Basic -74/{tree6.Name} Sapling/Takes 28 days to produce a mature {tree6.Name} tree. Bears item in the {seasons[5]}. Only grows if the 8 surrounding \"tiles\" are empty."},
			};

			foreach (KeyValuePair<int, string> pair in objectReplacements)
			{
				editedObjectInfo.ObjectInformationReplacements[pair.Key] = pair.Value;
			}
		}

		/// <summary>
		/// Randomizes the crops - currently only does prices, and only for seasonal crops
		/// </summary>
		/// <param name="editedObjectInfo">The edited object information</param>
		/// crop format: name/price/-300/Seeds -74/name/tooltip
		private static void RandomizeCrops(EditedObjectInformation editedObjectInfo)
		{
			// this swapped parsnips and amaranth
			editedObjectInfo.CropsReplacements[(int)ObjectIndexes.ParsnipSeeds] = "1 2 2 2/spring/39/300/-1/1/false/false/false";
			editedObjectInfo.CropsReplacements[(int)ObjectIndexes.AmaranthSeeds] = "1 1 1 1/fall/0/24/-1/0/false/false/false";
			//"299": "1 2 2 2/fall/39/300/-1/1/false/false/false",
			//"472": "1 1 1 1/spring/0/24/-1/0/false/false/false",

			Random rng = Globals.RNG;
			var cropPrices = new Dictionary<int, string>()
			{
				//Spring Crops
				{ (int)ObjectIndexes.JazzSeeds, $"Jazz Seeds/{rng.Next(11, 20)}/-300/Seeds -74/Jazz Seeds/Plant in spring. Takes 7 days to produce a blue puffball flower. Normal seed market price is 30g"},
				{ (int)ObjectIndexes.CauliflowerSeeds,$"Cauliflower Seeds/{rng.Next(35, 55)}/-300/Seeds -74/Cauliflower Seeds/Plant these in the spring. Takes 12 days to produce a large cauliflower. Normal seed market price is 80g"},
				{ (int)ObjectIndexes.GarlicSeeds, $"Garlic Seeds/{rng.Next(15, 30)}/-300/Seeds -74/Garlic Seeds/Plant these in the spring. Takes 4 days to mature. Normal seed market price is 40g"},
				{ (int)ObjectIndexes.BeanStarter, $"Bean Starter/{rng.Next(25, 40)}/-300/Seeds -74/Bean Starter/Plant these in the spring. Takes 10 days to mature, but keeps producing after that. Yields multiple beans per harvest. Grows on a trellis. Normal seed market price is 60g"},
				{ (int)ObjectIndexes.ParsnipSeeds, $"Not Parsnip Seeds/{rng.Next(7, 13)}/-300/Seeds -74/Not Parsnip Seeds/Plant these in the spring. Takes 4 days to mature. Normal seed market price is 20g"},
				{ (int)ObjectIndexes.PotatoSeeds, $"Potato Seeds/{rng.Next(20, 35)}/-300/Seeds -74/Potato Seeds/MODIFIED Plant these in the spring. Takes 6 days to mature, and has a chance of yielding multiple potatoes at harvest. Normal seed market price is 50g"},
				{ (int)ObjectIndexes.KaleSeeds, $"Kale Seeds/{rng.Next(30, 42)}/-300/Seeds -74/Kale Seeds/Plant these in the spring. Takes 6 days to mature. Harvest with the scythe. Normal seed market price is 70g"},
				{ (int)ObjectIndexes.RhubarbSeeds, $"Rhubarb Seeds/{rng.Next(45, 60)}/-300/Seeds -74/Rhubarb Seeds/Plant these in the spring. Takes 13 days to mature. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.StrawberrySeeds, $"Strawberry Seeds/0/-300/Seeds -74/Strawberry Seeds/Plant these in spring. Takes 8 days to mature, and keeps producing strawberries after that. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.TulipBulb, $"Tulip Bulb/{rng.Next(3, 7)}/-300/Seeds -74/Tulip Bulb/Plant in spring. Takes 6 days to produce a colorful flower. Assorted colors. Normal seed market price is 10g"},

				{ (int)ObjectIndexes.Amaranth, "TEST CROP/150/20/Basic -75/TEST CROP/YOUR MOTHER." }, //REMOVEME

				//Summer Crops
				{ (int)ObjectIndexes.BlueberrySeeds, $"Blueberry Seeds/{rng.Next(35, 50)}/-300/Seeds -74/Blueberry Seeds/Plant these in the summer. Takes 13 days to mature, and continues to produce after first harvest. Normal seed market price is 80g"},
				{ (int)ObjectIndexes.CornSeeds, $"Corn Seeds/{rng.Next(65, 90)}/-300/Seeds -74/Corn Seeds/Plant these in the summer or fall. Takes 14 days to mature, and continues to produce after first harvest. Normal seed market price is 150g"},
				{ (int)ObjectIndexes.HopsStarter, $"Hops Starter/{rng.Next(25, 50)}/-300/Seeds -74/Hops Starter/Plant these in the summer. Takes 11 days to grow, but keeps producing after that. Grows on a trellis. Normal seed market price is 60g"},
				{ (int)ObjectIndexes.PepperSeeds, $"Pepper Seeds/{rng.Next(15, 30)}/-300/Seeds -74/Pepper Seeds/Plant these in the summer. Takes 5 days to mature, and continues to produce after first harvest. Normal seed market price is 40g"},
				{ (int)ObjectIndexes.PoppySeeds, $"Poppy Seeds/{rng.Next(40, 60)}/-300/Seeds -74/Poppy Seeds/Plant in summer. Produces a bright red flower in 7 days. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.RadishSeeds, $"Radish Seeds/{rng.Next(15, 30)}/-300/Seeds -74/Radish Seeds/Plant these in the summer. Takes 6 days to mature. Normal seed market price is 40g"},
				{ (int)ObjectIndexes.RedCabbageSeeds, $"Red Cabbage Seeds/{rng.Next(45, 60)}/-300/Seeds -74/Red Cabbage Seeds/Plant these in the summer. Takes 9 days to mature. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.StarfruitSeeds, $"Starfruit Seeds/{rng.Next(175, 250)}/-300/Seeds -74/Starfruit Seeds/Plant these in the summer. Takes 13 days to mature. Normal seed market price is 200g"},
				{ (int)ObjectIndexes.SpangleSeeds, $"Spangle Seeds/{rng.Next(20, 30)}/-300/Seeds -74/Spangle Seeds/Plant in summer. Takes 8 days to produce a vibrant tropical flower. Assorted colors. Normal seed market price is "},
				{ (int)ObjectIndexes.SunflowerSeeds, $"Sunflower Seeds/{rng.Next(15, 25)}/-300/Seeds -74/Sunflower Seeds/Plant in summer or fall. Takes 8 days to produce a large sunflower. Yields more seeds at harvest. Normal seed market price is 200g"},
				{ (int)ObjectIndexes.TomatoSeeds, $"Tomato Seeds/{rng.Next(20, 35)}/-300/Seeds -74/Tomato Seeds/Plant these in the summer. Takes 11 days to mature, and continues to produce after first harvest. Normal seed market price is 50g"},
				{ (int)ObjectIndexes.WheatSeeds, $"Wheat Seeds/{rng.Next(3, 11)}/-300/Seeds -74/Wheat Seeds/Plant these in the summer or fall. Takes 4 days to mature. Harvest with the scythe. Normal seed market price is 10g"},
			   
				//Fall Crops
				{ (int)ObjectIndexes.AmaranthSeeds, $"Amaranth Seeds/{rng.Next(30, 45)}/-300/Seeds -74/Amaranth Seeds/Plant these in the fall. Takes 7 days to grow. Harvest with the scythe. Normal seed market price is 70g"},
				{ (int)ObjectIndexes.ArtichokeSeeds, $"Artichoke Seeds/{rng.Next(12, 20)}/-300/Seeds -74/Artichoke Seeds/Plant these in the fall. Takes 8 days to mature. Normal seed market price is 30g"},
				{ (int)ObjectIndexes.BeetSeeds, $"Beet Seeds/{rng.Next(8, 15)}/-300/Seeds -74/Beet Seeds/Plant these in the fall. Takes 6 days to mature. Normal seed market price is 20g"},
				{ (int)ObjectIndexes.BokChoySeeds, $"Bok Choy Seeds/{rng.Next(20, 35)}/-300/Seeds -74/Bok Choy Seeds/Plant these in the fall. Takes 4 days to mature. Normal seed market price is 50g"},
				{ (int)ObjectIndexes.CranberrySeeds, $"Cranberry Seeds/{rng.Next(110, 160)}/-300/Seeds -74/Cranberry Seeds/Plant these in the fall. Takes 7 days to mature, and continues to produce after first harvest. Normal seed market price is 240g"},
				{ (int)ObjectIndexes.EggplantSeeds, $"Eggplant Seeds/{rng.Next(7, 14)}/-300/Seeds -74/Eggplant Seeds/Plant these in the fall. Takes 5 days to mature, and continues to produce after first harvest. Normal seed market price is 20g"},
				{ (int)ObjectIndexes.FairySeeds, $"Fairy Seeds/{rng.Next(85, 115)}/-300/Seeds -74/Fairy Seeds/Plant in fall. Takes 12 days to produce a mysterious flower. Assorted Colors. Normal seed market price is 200g"},
				{ (int)ObjectIndexes.GrapeStarter, $"Grape Starter/{rng.Next(25, 40)}/-300/Seeds -74/Grape Starter/Plant these in the fall. Takes 10 days to grow, but keeps producing after that. Grows on a trellis. Normal seed market price is 60g"},
				{ (int)ObjectIndexes.PumpkinSeeds, $"Pumpkin Seeds/{rng.Next(40, 70)}/-300/Seeds -74/Pumpkin Seeds/Plant these in the fall. Takes 13 days to mature. Normal seed market price is 100g"},
				{ (int)ObjectIndexes.YamSeeds, $"Yam Seeds/{rng.Next(25, 40)}/-300/Seeds -74/Yam Seeds/Plant these in the fall. Takes 10 days to mature. Normal seed market price is 60g"},
			};

			foreach (KeyValuePair<int, string> pair in cropPrices)
			{
				editedObjectInfo.ObjectInformationReplacements[pair.Key] = pair.Value;
			}
		}
	}
}
