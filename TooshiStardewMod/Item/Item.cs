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
			get { return GetNameFromId(Id); }
		}
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
		/// Gets a randomly generated amount of this item required for a crafting recipe
		/// Will always return a value of at least 1
		/// </summary>
		/// <returns />
		public int GetAmountRequiredForCrafting()
		{
			int baseAmount = ItemsRequiredForRecipe.GetRandomValue();
			return Math.Max((int)(baseAmount * RequiredItemMultiplier), 1);
		}

		/// <summary>
		/// Gets the name of an item from
		/// </summary>
		/// <param name="id">The item id</param>
		/// <returns>Splits apart the name from the ObjectIndexes name - WildHorseradish -> Wild Horseradish</returns>
		public static string GetNameFromId(int id)
		{
			string enumName = ((ObjectIndexes)id).ToString();
			return Regex.Replace(enumName, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
		}
	}
}
