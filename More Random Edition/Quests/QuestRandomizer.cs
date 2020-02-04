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
			{109, "ItemDelivery/Fish Catching/[person] is challenging you to catch [a] [fish], saying that a true fishing master can figure out where to get it themselves./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/Hey, you did it! Not bad. Not bad at all. I'm impressed.#$b#Winter's a good time to break out the old fishing rod, isn't it?"},
			{110, "ItemDelivery/[otherperson]'s Attempt/[otherperson] wants you to give [person] [a] [item] and wants you to say that it's from them./Bring [person] [a] [item]./[person] [id]/-1/0/-1/true/Oh, I love this! You're so sweet!$h#$b#Huh? It's from who?$u#$b#Oh, you got it from [otherperson]? Well, I don't care where you got it from, it's beautiful! Thank you! *smooch*$h"},
			{111, "ItemDelivery/A Dark Reagent/[person] wants you to descend into the mines and find [a] [item]. It's needed for some kind of dark magic./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Ah, you've brought it. You've earned my gratitude, and a [reward]g reward. Now go."},
			{112, "ItemDelivery/A Favor For [person]/[person] got a new hammer and wants to test it out on a variety of materials./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Hey, it's the [item] I asked for. It looks strong... perfect.#$b#Thanks, @. I appreciate this."},
			{113, "ItemDelivery/[person]'s Request/[person] wrote to you asking for [number] [item]s./Bring [person] [number] [item]s./[person] [id] [number]/-1/[reward]/-1/true/Oh, you brought them! I know I can always count on you, @.$h#$b#Mmhmm... This is perfect. It's exactly what I need. Thanks!"},
			{114, "ItemDelivery/Fish Stew/[person] wants to make fish stew, but needs [a] [fish]./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/*sniff*...*sniff*... What's that? Something smells like [fish]!#$b#Aha! You brought it! Thanks a million!"},
			{115, "ItemDelivery/Fresh Food/[person] wants a taste of locally grown produce and is asking for a fresh [crop]./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/Oh... You followed through! Thanks, this looks delicious!$h"},
			{116, "ItemDelivery/Chef's Gift/[person] wants to surprise [otherperson] with a gift./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/Oh, thank you, dear. This [crop] looks delicious. [otherperson] will be so happy.#$b#[otherperson] loves when I cook eggs with [crop] for breakfast."},
			{117, "ItemDelivery/[person]'s Notice/[person] will pay \"top coin\" to whoever brings in a plate of [dish]. Apparently they're really craving the stuff./Bring [person] some [dish]./[person] [id]/-1/[reward]/-1/true/It's about time! I was starting to get the shakes, I wanted this [dish] so bad. *munch*... Mmm, now that's good.#$b#Thanks, @.$h"},
			{118, "ItemDelivery/Aquatic Research/[person] is studying the toxin levels of the local [fish] and would like you to find one./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/There you are. The specimen looks perfect. I'm going to get it on ice straight away. Thanks, @!"},
			{119, "ItemDelivery/A Soldier's Gift/Kent wants to give [otherperson] [a] [crop] for their birthday./Bring Kent [a] [crop]./Kent [id]/-1/[reward]/-1/true/Hey. Shhh... Don't let [otherperson] see.#$b#Ah, this looks juicy. They'll love it. Thank you so much.$h"},
			{120, "ItemDelivery/[person]'s Needs/[person] wants [a] [item], but won't explain what it's for. Maybe it's none of your business./Bring [person] some [item]./[person] [id]/-1/[reward]/-1/true/You got the [item]? Let me see...#$b#It's high quality... very slick. Great. Thank you."},
			{121, "ItemDelivery/Wanted: [fish]/[person] put out a notice requesting a fresh [fish]./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/Something smells like fresh [fish]. Oh, that's good.$h#$b#Take care, friend."},
			{122, "ItemDelivery/[person] Needs Juice/[person]'s TV Remote is dead. For some reason [a] [item] is needed to fix it./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Hey, you pulled through with the [item]! Thanks, kid... You're a life-saver!$h"},
			{123, "ItemDelivery/Staff Of Riches/[person] is creating a staff of phenomenal riches. Who knows what it's for? The power of [a] [item] is needed to finish it./Bring [person] [a] [item]./[person] [id]/-1/[reward]/-1/true/Ah, precious [item]. You've done well, @. You have my gratitude. Now, leave."},
			{124, "ItemDelivery/Catch a Fish/[person] is challenging you to catch [a] [fish]./Bring [person] [a] [fish]./[person] [id]/-1/[reward]/-1/true/Hey, that's a real lunker! You've certainly got the angler's blood in you.$h"},
			{125, "ItemDelivery/Exotic Spirits/[person] wants to make [a] [cropstart]-no-no, but the main ingredient's missing./Bring [person] [a] [crop]./[person] [id]/-1/[reward]/-1/true/[crop]! Now there's a soothing sight for my eyes.$h#$b#It's going to be perfect for my [cropstart]-no-no. Thanks!"}
		};

		/// <summary>
		/// A mapping of quest IDs to what mail key it belongs to
		/// </summary>
		private static readonly Dictionary<int, string> QuestToMailMap = new Dictionary<int, string>()
		{
			{ 101, "spring_19_1" },
			{ 103, "summer_14_1" },
			{ 104, "summer_20_1" },
			{ 105, "summer_25_1" },
			{ 106, "fall_3_1" },
			{ 108, "fall_19_1" },
			{ 109, "winter_2_1" },
			{ 110, "winter_6_1" },
			{ 111, "winter_12_1" },
			{ 112, "winter_17_1" },
			{ 113, "winter_21_1" },
			{ 114, "winter_26_1" },
			{ 115, "spring_6_2" },
			{ 116,  "spring_15_2" },
			{ 117, "spring_21_2" },
			{ 118, "summer_6_2" },
			{ 119, "summer_15_2"},
			{ 120, "summer_21_2" },
			{ 121, "fall_6_2"},
			{ 122, "fall_19_2" },
			{ 123, "winter_5_2"},
			{ 124, "winter_13_2" },
			{ 125, "winter_19_2" }
		};

		/// <summary>
		/// The default mail data that could potentially be replaced
		/// </summary>
		private static readonly Dictionary<string, string> DefaultMailData = new Dictionary<string, string>()
		{
			{ "spring_19_1", "Farmer @-^I have a request for you. I need a fresh [crop] for a recipe I want to make. Could you bring me one?^   -[person]%item quest 101 %%[#][person] Asks For [crop]" },
			{ "summer_14_1", "Hey Kid,^My stomach's about as empty as a TV remote with no batteries. I'm real hungry for [dish]. You got some? Just any old food won't do. I need [dish].^   -[person]%item quest 103 %%[#][person]'s Request" },
			{ "summer_20_1", "@-^I'm gathering data on the correlation between soil alkalinity and crop fructose levels. Long story short, I need a fresh [crop] from your farm. If you brought me one I'd be very grateful.^   -[person]%item quest 104 %%[#][person] Asks For A Sample" },
			{ "summer_25_1", "To Farmer @:^My knee's acting up again, and you know what helps? ^Rubbing the darn thing with [crop].^ Trouble is, my supply's run dry. If you've got one to spare I'd be much obliged.^   -[person] %item quest 105 %%[#][person] Needs A Crop" },
			{ "fall_3_1", "Dear @,^I'd like to give some cows a special treat. They're such good girls and hungry, too. Could you bring me one bunch of [crop]? They love the stuff. ^Thanks, Dear.^   -[person] %item quest 106 %%[#][person]'s Request" },
			{ "fall_19_1", "@-^I'd like to buy [a] [crop] from you. [otherperson] and I want to carve a [cropstart]-o-lantern for the upcoming Spirit's Eve festival. ^   -[person]%item quest 108 %%[#][person]'s Request" },
			{ "winter_2_1", "Hey, I've got a little challenge for you:^    Catch me [a] [fish].^No, I don't know where or when to find one. Figure it out!^   -[person]%item quest 109 %%[#][person]'s Fishing Challenge" },
			{ "winter_6_1", "I've got an unusual request. Could you keep this a secret?^ I want you to deliver [a] [item] to [person]. It's their favorite thing. Tell them it's from me.^   -[otherperson] %item quest 110 %%[#][otherperson]'s Unusual Request" },
			{ "winter_12_1", "@-^I am researching the forgotten art of shadow divination. I require an item known as '[item]'. Bring it to me and you will be rewarded.^ -[person] %item quest 111 %%[#][person]'s Request" },
			{ "winter_17_1", "@,^Sorry to bother you again, but I need another favor.^I got a new hammer and I want to try it on a variety of metals.^Could you find [a] [item] and bring it to me? ^Thanks.^ -[person] %item quest 112 %%[#][person]'s Request" },
			{ "winter_21_1", "@-^How are you doing? Hope the winter hasn't been too hard on you.^Anyway, I'm writing to ask you for [a] [item]. Actually, I need [number] of them, if you've got it.^If not, no worries. Take Care!^  -[person]%item quest 113 %%[#][person]'s Request" },
			{ "winter_26_1", "Dear @-^I want to make fish stew, but I need [a] [fish]! I don't know if they're in season now... sorry about the short notice. Could you catch one for me?^   -[person]%item quest 114 %%[#][person] Needs A Certain Fish" },
			{ "spring_6_2", "@-^I'm really craving a fresh [crop]. I haven't been able to find one at the store, so I'm asking you. I'll pay you well for it!^   -[person]%item quest 115 %%[#][person]'s Request" },
			{ "spring_15_2", "Dear @,^I'd like to surprise [otherperson] with a gift. They're a big fan of [crop]. Could you bring me one?^   -[person]%item quest 116 %%[#][person]'s Surprise" },
			{ "spring_21_2", "NOTICE^[person] here... I really, really want a plate of [dish]. I'll pay top coin to whoever brings me some.^   -[person]%item quest 117 %%[#][person]'s Craving" },
			{ "summer_6_2", "Fisherman Wanted^I need a good [fish] specimen. I'm conducting an experiment on the toxin created by the [fish].^Reward: [reward]g^ -[person]%item quest 118 %%[#][person]' Study On Toxins" },
			{ "summer_15_2", "Dear Farmer @,^I'd like to give [otherperson] [a] [crop] for their birthday. Are you growing any on your farm? If you brought me one, I'd be very grateful.^   -Kent%item quest 119 %%[#]Kent Needs A Gift" },
			{ "summer_21_2", "@,^I need [a] [item]. Don't ask me why.^ -[person]%item quest 120 %%[#][person]' Request" },
			{ "fall_6_2", "Wanted: 1 fresh [fish] for a marvelous bisque I'm creating.^Who: [person], a soup fanatic.^Reward: [reward]g %item quest 121 %%[#][person] Needs Lobster" },
			{ "fall_19_2", "NOTICE^Does anybody have [a] [item]? My TV remote died, and it's a real hassle getting off the couch to change the station.^  -[person]%item quest 122 %%[#][person] Needs Help" },
			{ "winter_5_2", "@,^I'm creating an enchanted staff of phenomenal riches.^However, I'm missing something: [a] [item].^All the money I make from my staff will be yours as a reward! Bring it as soon as you can.^ -[person] %item quest 123 %%[#][person] Needs Some Random Item" },
			{ "winter_13_2", "I've got a fishing challenge for you: Catch me [a] [fish].^They don't go down easy, but I know you can do it.^  -[person]%item quest 124 %%[#][person]'s Better Fishing Challenge" },
			{ "winter_19_2", "Dear @,^I'm at my wit's end. I desperately want to make [a] [cropstart]-no-no, but I have no way of getting a [crop]!^Can you help me?^  -[person]%item quest 125 %%[#][person]'s Making a Drink" }
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
				OtherPerson = Globals.RNGGetRandomValueFromList(People.Where(x => x != Person).ToList());
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
