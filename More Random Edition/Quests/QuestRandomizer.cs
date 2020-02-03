using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class QuestRandomizer
	{
		private static readonly List<string> People = NPC.QuestableNPCsList;
		private static readonly List<int> CropIds = ItemList.GetCrops(true).Select(x => x.Id).ToList();
		private static readonly List<int> DishIds = ItemList.GetCookeditems().Select(x => x.Id).ToList();
		private static readonly List<int> FishIds = FishItem.Get().Select(x => x.Id).ToList();
		private static readonly List<int> ItemIds = ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.Impossible).Select(x => x.Id).ToList();

		private static readonly int ParsnipCropId = ((SeedItem)ItemList.Items[(int)ObjectIndexes.ParsnipSeeds]).CropGrowthInfo.CropId;
		private static readonly Dictionary<int, string> DefaultQuestData = new Dictionary<int, string>()
		{
			{3, $"Basic/The Mysterious Qi/You've found another note written by 'Mr. Qi'. The request is even more unusual this time./Place 10 {ItemList.GetItemName((int)ObjectIndexes.Beet)}s inside Mayor Lewis' fridge./null/-1/0/-1/false" },
			{6, $"ItemHarvest/Getting Started/If you want to become a farmer, you have to start with the basics. Use your hoe to till the soil, then use a seed packet on the tilled soil to sow a crop. Water every day until the crop is ready for harvest./Cultivate and harvest [a] {ItemList.GetItemName(ParsnipCropId)}./{ParsnipCropId}/h7 8/100/-1/true"},
			{22, $"Basic/Fish Casserole/Jodi swung by the farm to ask you to dinner at 7:00 PM. Her only request was that you bring one {ItemList.GetItemName((int)ObjectIndexes.LargemouthBass)} for her fish casserole./Enter Jodi's house with one {ItemList.GetItemName((int)ObjectIndexes.LargemouthBass)} at 7:00 PM./-1/-1/0/-1/true"},
			{101, "ItemDelivery/[person]'s Request/[person] needs a fresh [crop] for a recipe and is asking you to grow one./Bring [person] a [crop]./[person] [id]/-1/[reward]/-1/true/Oh, that looks so delicious! Thank you, this is just what I wanted. It's going to be perfect for my yellow curry."},
			{103, "ItemDelivery/[person] Is Hungry/[person] is hankerin' for a [dish]. Nothing else will do. You can probably make one yourself if you have the ingredients./Bring [person] a [dish]./[person] [id]/-1/[reward]/-1/true/Gimme that. *slurp*... Ahh, that's the stuff.#$b#It's real nice and hoppy... notes of citrus and pine, but with a robust body to keep it grounded.#$b#Thanks, kid. This means a lot to me. I knew I could count on you.$h"},
			{104, "ItemDelivery/Crop Research/[person] needs a fresh [crop] for research purposes./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/This is perfect! It's just what I need for my research. It's going to be hard not to eat it! Thanks a bunch."},
			{105, "ItemDelivery/Knee Therapy/[person] needs [a] [crop] to soothe an aching knee./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/Took you long enough... hmmph... Well it's good and spicy at least. Thanks."},
			{106, "ItemDelivery/Cow's Delight/[person] wants to give some cows a special treat and is asking for a single bunch of [crop]s./Bring [person] one bunch of [crop]s./[person] [id]/-1/[reward]/-1/true/Oh, the [crop]s I asked for! Thank you so much... the cows are going to love this!$h"},
			{108, "ItemDelivery/Carving [crop]s/[person] wants to carve [a] [crop] with [otherperson]. Can you bring them one from the farm?/Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/Oh, the [crop]! It's a good one... [otherperson] will be so happy to see this. Thanks, @!$h"},
			{109, "ItemDelivery/[fish] Catching/[person] is challenging you to catch [a] [fish], saying that a true fishing master can figure out where to get it themselves./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/Hey, you did it! Not bad. Not bad at all. I'm impressed.#$b#Winter's a good time to break out the old fishing rod, isn't it?"},
			{110, "ItemDelivery/[otherperson]'s Attempt/[otherperson] wants you to give [person] [a] [item] and wants you to say that it's from them./Bring [person] [a] [item]./[person] [id]/-1/0/-1/true/Oh, I love this! You're so sweet!$h#$b#Huh? It's from who?$u#$b#Oh, you got it from [otherperson]? Well, I don't care where you got it from, it's beautiful! Thank you! *smooch*$h"},
			{111, "ItemDelivery/A Dark Reagent/[person] wants you to descend into the mines and find [a] [item]. It's needed for some kind of dark magic./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Ah, you've brought it. You've earned my gratitude, and a [reward]g reward. Now go."},
			{112, "ItemDelivery/A Favor For [person]/[person] got a new hammer and wants to test it out on a variety of materials./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Hey, it's the [item] I asked for. It looks strong... perfect.#$b#Thanks, @. I appreciate this."},
			{113, "ItemDelivery/[person]'s Request/[person] wrote to you asking for [number] [item]s./Bring [person] [number] [item]s./[person] [id] [number]/-1/[reward]/-1/true/Oh, you brought it! I know I can always count on you, @.$h#$b#Mmhmm... This is perfect. It's exactly what I need. Thanks!"},
			{114, "ItemDelivery/Fish Stew/[person] wants to make fish stew, but needs [a] [fish]./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/*sniff*...*sniff*... What's that? Something smells like [fish]!#$b#Aha! You brought it! Thanks a million!"},
			{115, "ItemDelivery/Fresh Food/[person] wants a taste of locally grown produce and is asking for a fresh [crop]./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/Oh... You followed through! Thanks, this looks delicious!$h"},
			{116, "ItemDelivery/Chef's Gift/[person] wants to surprise [otherperson] with a gift./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/Oh, thank you, dear. This [crop] looks delicious. [otherperson] will be so happy.#$b#[otherperson] loves when I cook eggs with [crop] for breakfast."},
			{117, "ItemDelivery/[person]'s Notice/[person] will pay \"top coin\" to whoever brings in a plate of [dish]. Apparently they're really craving the stuff./Bring [person] some [dish]./[person] [id]/-1/[reward]/-1/true/It's about time! I was starting to get the shakes, I wanted this [dish] so bad. *munch*... Mmm, now that's good.#$b#Thanks, @.$h"},
			{118, "ItemDelivery/Aquatic Research/[person] is studying the toxin levels of the local [fish] and would like you to find one./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/There you are. The specimen looks perfect. I'm going to get it on ice straight away. Thanks, @!"},
			{119, "ItemDelivery/A Soldier's Gift/Kent wants to give [otherperson] [a] [crop] for their birthday./Bring Kent [a] [crop]./Kent [id]/-1/[reward]/-1/true/Hey. Shhh... Don't let [otherperson] see.#$b#Ah, this looks juicy. They'll love it. Thank you so much.$h"},
			{120, "ItemDelivery/[person]'s Needs/[person] wants [a] [item], but won't explain what it's for. Maybe it's none of your business./Bring [person] some [item]./[person] [id]/-1/[reward]/-1/true/You got the [item]? Let me see...#$b#It's high quality... very slick. Great. Thank you."},
			{121, "ItemDelivery/Wanted: [fish]/[person] put out a notice requesting a fresh [fish]./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/Something smells like fresh [fish]. Oh, that's good.$h#$b#Take care, friend."},
			{122, "ItemDelivery/[person] Needs Juice/[person]'s TV Remote is dead. For some reason [a] [item] is needed to fix it./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Hey, you pulled through with the [item]! Thanks, kid... You're a life-saver!$h"},
			{123, "ItemDelivery/Staff Of Power/[person] is creating a staff of phenomenal power. Who knows what it's for? The power of [a] [item] is needed to finish it./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Ah, precious [item]. You've done well, @. You have my gratitude. Now, leave."},
			{124, "ItemDelivery/Catch [a] [fish]/[person] is challenging you to catch [a] [fish]./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/Hey, that's a real lunker! You've certainly got the angler's blood in you.$h"},
			{125, "ItemDelivery/Exotic Spirits/[person] wants to make [a] [cropstart]-no-no, but the main ingredient's missing./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/[crop]! Now there's a soothing sight for my eyes.$h#$b#It's going to be perfect for my [cropstart]-no-no. Thanks!"}
		};

		/// <summary>
		/// A mapping of quest IDs to what mail key it belongs to
		/// </summary>
		private static readonly Dictionary<int, string> QuestToMailMap = new Dictionary<int, string>()
		{
			{ 101, "spring_19_1" }
		};

		/// <summary>
		/// The default mail data that could potentially be replaced
		/// </summary>
		private static readonly Dictionary<string, string> DefaultMailData = new Dictionary<string, string>()
		{
			{ "spring_19_1", "Farmer @-^I have a request for you. I need a fresh [crop] for a recipe I want to make. Could you bring me one?^   -[person]%item quest 101 %%[#][person] Asks For [crop]" }
		};

		/// <summary>
		/// Randomizes quest items to get, people, rewards, etc.
		/// </summary>
		/// <returns>The quest information to modify</returns>
		public static QuestInformation Randomize()
		{
			Dictionary<int, string> questReplacements = new Dictionary<int, string>();
			Dictionary<string, string> mailReplacements = new Dictionary<string, string>();
			RandomizeQuestsAndMailStrings(questReplacements, mailReplacements);

			WriteToSpoilerLog(questReplacements);

			return new QuestInformation(questReplacements, mailReplacements);
		}

		/// <summary>
		/// Populates the given quest and mail replacement dictionaries
		/// </summary>
		/// <param name="questReplacements">The dictionary of quest replacements to fill</param>
		/// <param name="mailReplacements">The dictionary of mail replacements to fill</param>
		private static void RandomizeQuestsAndMailStrings(
			Dictionary<int, string> questReplacements,
			Dictionary<string, string> mailReplacements)
		{
			foreach (int questId in DefaultQuestData.Keys)
			{
				ReplacementObject replacements = new ReplacementObject();

				string questString = DefaultQuestData[questId];
				questString = GetReplacementString(questString, replacements);
				questReplacements.Add(questId, questString);

				if (QuestToMailMap.ContainsKey(questId) && DefaultMailData.ContainsKey(QuestToMailMap[questId]))
				{
					string mailKey = QuestToMailMap[questId];
					string mailString = GetReplacementString(DefaultMailData[mailKey], replacements);
					mailReplacements.Add(mailKey, mailString);
				}
			}
		}

		/// <summary>
		/// Takes an input string and replacement object and returns it with all replacements made
		/// </summary>
		/// <param name="inputString">The input string</param>
		/// <param name="replacements">The replacements to be made</param>
		/// <returns>The string with replacements made</returns>
		private static string GetReplacementString(string inputString, ReplacementObject replacements)
		{
			string currentValue = inputString;
			currentValue = ReplaceToken(currentValue, "[person]", replacements.Person);
			currentValue = ReplaceToken(currentValue, "[otherperson]", replacements.OtherPerson);
			currentValue = ReplaceToken(currentValue, "[crop]", replacements.CropId, true);
			currentValue = ReplaceToken(currentValue, "[cropstart]", replacements.CropId, true, true);
			currentValue = ReplaceToken(currentValue, "[dish]", replacements.DishId, true);
			currentValue = ReplaceToken(currentValue, "[fish]", replacements.FishId, true);
			currentValue = ReplaceToken(currentValue, "[item]", replacements.ItemId, true);
			currentValue = ReplaceToken(currentValue, "[number]", replacements.Number);
			currentValue = ReplaceToken(currentValue, "[reward]", replacements.Reward);
			currentValue = ReplaceArticleTokens(currentValue);

			return currentValue;
		}

		/// <summary>
		/// Used as a container of all of the replacements to be potentially made for a given quest
		/// </summary>
		private class ReplacementObject
		{
			public string Person { get; }
			public string OtherPerson { get; }
			public int CropId { get; }
			public int DishId { get; }
			public int FishId { get; }
			public int ItemId { get; }
			public int Number { get; }
			public int Reward { get; }

			public ReplacementObject()
			{
				Person = Globals.RNGGetRandomValueFromList(People);
				OtherPerson = Globals.RNGGetRandomValueFromList(People);
				CropId = Globals.RNGGetRandomValueFromList(CropIds);
				DishId = Globals.RNGGetRandomValueFromList(DishIds);
				FishId = Globals.RNGGetRandomValueFromList(FishIds);
				ItemId = Globals.RNGGetRandomValueFromList(ItemIds);
				Number = Globals.RNG.Next(2, 10);
				Reward = Globals.RNG.Next(300, 3000);
			}
		}

		/// <summary>
		/// Returns a string with tokens replaced with an item name and id
		/// </summary>
		/// <param name="input">The input string</param>
		/// <param name="token">The token</param>
		/// <param name="number">Either the number to use as the replacement, or the item Id if it's an item</param>
		/// <param name="isItem">Whether this entry is for an item id</param>
		/// <param name="shortenName">Whether the name of the item name should be shortened to 4 characters</param>
		/// <returns>The string with the replacements made</returns>
		private static string ReplaceToken(string input, string token, int number, bool isItem = false, bool shortenName = false)
		{
			if (!input.Contains(token))
				return input;
			if (isItem) // number is the itemID
			{
				string itemName = ItemList.GetItemName(number);
				if (shortenName)
				{
					itemName = Globals.GetStringStart(itemName, 4);
				}
				return input.Replace(token, itemName).Replace("[id]", number.ToString());
			}
			else
				return input.Replace(token, number.ToString());
		}

		/// <summary>
		/// Returns a string with tokens replaced by a string in the list
		/// </summary>
		/// <param name="input">The input string</param>
		/// <param name="token">The token</param>
		/// <param name="replacements">The string to replace the tokens with</param>
		/// <returns>The new string</returns>
		private static string ReplaceToken(string input, string token, string replacementString)
		{
			if (!input.Contains(token))
				return input;
			return input.Replace(token, replacementString);
		}

		/// <summary>
		/// Replaces the article tokens in a quest string ([a] could be "a" or "an")
		/// </summary>
		/// <param name="input">The input string</param>
		/// <returns>The string with the replacements made</returns>
		private static string ReplaceArticleTokens(string input)
		{
			if (!input.Contains("[a]"))
				return input;
			string[] substrings = input.Split(' ');
			List<int> articleLocations = FindAllIndexOf("[a]", substrings);

			foreach (int i in articleLocations)
			{
				if (substrings[i + 1] != "")
					substrings[i] = Globals.GetArticle(substrings[i + 1]);
			}

			return String.Join(" ", substrings);
		}

		/// <summary>
		/// Finds all of the indexes of a given token in a string array
		/// </summary>
		/// <param name="token">The token</param>
		/// <param name="stringArray">The string array</param>
		/// <returns>The list of indexes found</returns>
		private static List<int> FindAllIndexOf(string token, string[] stringArray)
		{
			List<int> allFoundIndices = new List<int>();

			for (int i = 0; i < stringArray.Length; i++)
			{
				if (stringArray[i] == token)
					allFoundIndices.Add(i);
			}
			return allFoundIndices;
		}

		/// <summary>
		/// Writes the dictionary info to the spoiler log
		/// </summary>
		/// <param name="questList">The info to write out</param>
		private static void WriteToSpoilerLog(Dictionary<int, string> questList)
		{
			if (!Globals.Config.RandomizeQuests) { return; }

			Globals.SpoilerWrite("==== QUESTS ====");
			foreach (KeyValuePair<int, string> pair in questList)
			{
				Globals.SpoilerWrite($"{pair.Key}: \"{pair.Value}\"");
			}
			Globals.SpoilerWrite("");
		}
	}
}
