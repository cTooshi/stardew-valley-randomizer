using System.Collections.Generic;

namespace Randomizer
{
	public class MonsterRandomizer
	{
		/// <summary>
		/// Randomizes monster stats/drops/etc.
		/// </summary>
		/// <returns />
		public static Dictionary<string, string> Randomize()
		{
			Dictionary<string, string> replacements = new Dictionary<string, string>();
			foreach (Monster monster in MonsterData.GetAllMonsters())
			{
				//TODO: actually randomize the monster stats!
				replacements.Add(monster.Name, monster.ToString());
			}
			return replacements;
		}
	}
}
