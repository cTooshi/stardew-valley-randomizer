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
		public bool ShouldBeForagable = false;
		public bool IsForagable
		{
			get { return ShouldBeForagable || ForagableLocationData.HasData(); }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The item ID</param>
		public Item(int id)
		{
			Id = id;
		}

		public static string GetNameFromId(int id)
		{
			string enumName = ((ObjectIndexes)id).ToString();
			return Regex.Replace(enumName, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
		}
	}
}
