using System;
using System.Text.RegularExpressions;

namespace Randomizer
{
	/// <summary>
	/// Represents an item in the game
	/// </summary>
	public class Item
	{
		public int Id { get; }
		public string Name
		{
			get { return GetNameFromId(Id, PutLastWordOfNameInParens); }
		}
		public bool PutLastWordOfNameInParens { get; set; }
		public ForagableLocationData ForagableLocationData { get; } = new ForagableLocationData();
		public bool ShouldBeForagable { get; set; }
		public bool IsForagable
		{
			get { return ShouldBeForagable || ForagableLocationData.HasData(); }
		}

		public bool IsTrash { get; set; }
		public bool IsCraftable { get; set; }
		public bool IsSmelted { get; set; }
		public bool IsFish { get; set; }
		public bool IsArtifact { get; set; }
		public bool IsMayonaisse { get; set; }
		public bool IsGeodeMineral { get; set; }
		public bool IsCrabPotItem { get; set; }
		public bool RequiresOilMaker { get; set; }
		public bool RequiresBeehouse { get; set; }

		public bool IsResource { get; set; }
		public Range ItemsRequiredForRecipe { get; set; } = new Range(1, 1);
		public double RequiredItemMultiplier = 1;

		/// <summary>
		/// The difficulty that this item is to obtain
		/// Will return values appropriate to foragable items - they are never impossible
		/// </summary>
		public ObtainingDifficulties DifficultyToObtain
		{
			get
			{
				if (_difficultyToObtain == ObtainingDifficulties.Impossible && IsForagable)
				{
					return ObtainingDifficulties.LargeTimeRequirements;
				}
				return _difficultyToObtain;
			}
			set
			{
				_difficultyToObtain = value;
			}
		}
		private ObtainingDifficulties _difficultyToObtain { get; set; } = ObtainingDifficulties.Impossible;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The item ID</param>
		public Item(int id)
		{
			Id = id;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The item ID</param>
		/// <param name="difficultyToObtain">The difficulty to obtain this item</param>
		public Item(int id, ObtainingDifficulties difficultyToObtain)
		{
			Id = id;
			DifficultyToObtain = difficultyToObtain;
		}


		/// <summary>
		/// Returns the string version of this item to use in crafting recipes
		/// Will NOT return the same value each time this is called!
		/// </summary>
		/// <returns></returns>
		public string GetStringForCrafting()
		{
			return $"{Id} {GetAmountRequiredForCrafting()}";
		}

		/// <summary>
		/// Gets a randomly generated amount of this item required for a crafting recipe
		/// Will always return a value of at least 1
		/// </summary>
		/// <returns />
		private int GetAmountRequiredForCrafting()
		{
			int baseAmount = ItemsRequiredForRecipe.GetRandomValue();
			return Math.Max((int)(baseAmount * RequiredItemMultiplier), 1);
		}

		/// <summary>
		/// Gets the name of an item from
		/// The putLastWordOfNameInParens parameter won't work if not passed in, no matter the item
		/// </summary>
		/// <param name="id">The item id</param>
		/// <param name="putLastWordOfNameInParens">Whether the put the last part of the string in parens</param>
		/// <returns>Splits apart the name from the ObjectIndexes name - WildHorseradish -> Wild Horseradish</returns>
		public static string GetNameFromId(int id, bool putLastWordOfNameInParens = false)
		{
			string enumName = ((ObjectIndexes)id).ToString();
			if (enumName == "BrokenCD") { return "Broken CD"; }

			string separatedName = Regex.Replace(enumName, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").Trim();
			if (!putLastWordOfNameInParens)
			{
				return separatedName;
			}

			string[] splitName = separatedName.Split(' ');
			string output = "";
			for (int i = 0; i < splitName.Length; i++)
			{
				string namePart = splitName[i];
				if (i == splitName.Length - 1)
				{
					output += $"({namePart})";
				}
				else
				{
					output += $"{namePart} ";
				}
			}
			return output.Trim();
		}

		/// <summary>
		/// Gets what a price for an item might be just based on its difficulty to obtain
		/// </summary>
		/// <param name="multiplier">The multiplier for the price - will add or subtract this as a percentage</param>
		/// <returns>The computed price</returns>
		public int GetPriceForObtainingDifficulty(double multiplier)
		{
			int basePrice = 0;
			switch (DifficultyToObtain)
			{
				case ObtainingDifficulties.NoRequirements:
					basePrice = 1000;
					break;
				case ObtainingDifficulties.SmallTimeRequirements:
					basePrice = 5000;
					break;
				case ObtainingDifficulties.MediumTimeRequirements:
					basePrice = 7500;
					break;
				case ObtainingDifficulties.LargeTimeRequirements:
					basePrice = 10000;
					break;
				case ObtainingDifficulties.UncommonItem:
					basePrice = 2500;
					break;
				case ObtainingDifficulties.RareItem:
					basePrice = 20000;
					break;
				case ObtainingDifficulties.EndgameItem:
					basePrice = 20000;
					break;
				default:
					Globals.ConsoleWrite($"ERROR: Tried to get a base price for an item with an unrecognized ObtainingDifficulty: {Name}");
					return 100;
			}

			int smallerBasePrice = basePrice / 10; // Guarantees that the price will be an even number
			Range range = new Range(
				(int)(smallerBasePrice - (smallerBasePrice * multiplier)),
				(int)(smallerBasePrice * (multiplier + 1))
			);
			return range.GetRandomValue() * 10;
		}
	}
}
