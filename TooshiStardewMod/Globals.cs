using StardewModdingAPI;
using System;

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
	}
}
