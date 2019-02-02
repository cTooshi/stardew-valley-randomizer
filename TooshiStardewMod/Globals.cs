using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace Randomizer
{
	/// <summary>
	/// Used for any global access - USE SPARINGLY
	/// </summary>
	public class Globals
	{
		public static Mod ModRef { get; set; }
		public static Random RNG { get; set; }

		/// <summary>
		/// A shortcut to write to the console
		/// </summary>
		/// <param name="input">The input string</param>
		public static void ConsoleWrite(string input)
		{
			ModRef.Monitor.Log(input);
		}

		/// <summary>
		/// Gets a random boolean value
		/// </summary>
		/// <returns />
		public static bool RNGGetNextBoolean()
		{
			return RNG.Next(0, 2) == 0;
		}

		/// <summary>
		/// Gets a random value out of the given list
		/// </summary>
		/// <typeparam name="T">The type of the list</typeparam>
		/// <param name="list">The list</param>
		/// <returns />
		public static T RNGGetRandomValueFromList<T>(List<T> list)
		{
			if (list == null || list.Count == 0)
			{
				ConsoleWrite("ERROR: Attempted to get a random value out of an empty list!");
				return default(T);
			}

			return list[RNG.Next(list.Count)];
		}
	}
}
