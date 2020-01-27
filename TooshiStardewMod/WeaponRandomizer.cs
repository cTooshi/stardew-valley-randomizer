using System;
using System.Collections.Generic;

namespace Randomizer
{
	/// <summary>
	/// Modifies weapons
	/// </summary>
	public class WeaponRandomizer
	{
		/// <summary>
		/// Returns the object use to modify the weapons
		/// </summary>
		/// <returns />
		public static Dictionary<int, string> Randomize()
		{
			Dictionary<int, WeaponItem> weaponDictionary = WeaponData.Items();
			Dictionary<int, string> stringReplacements = new Dictionary<int, string>();
			foreach (WeaponItem weapon in weaponDictionary.Values)
			{
				RandomizeWeapon(weapon);
				stringReplacements.Add(weapon.Id, weapon.ToString());
			}

			WriteToSpoilerLog(weaponDictionary);
			return stringReplacements;
		}

		/// <summary>
		/// Randomizes the values on the given weapon
		/// </summary>
		/// <param name="weapon">The weapon to randomize</param>
		private static void RandomizeWeapon(WeaponItem weapon)
		{
			//TODO: something meaningful!
			weapon.Description = $"{weapon.Name} is the best thing ever!!!";
		}

		/// <summary>
		/// Writes the changed weapon info to the spoiler log
		/// </summary>
		/// <param name="modifiedWeaponDictionary">The dictionary with changed info</param>
		private static void WriteToSpoilerLog(Dictionary<int, WeaponItem> modifiedWeaponDictionary)
		{
			Globals.SpoilerWrite("==== WEAPONS ====");
			foreach (int id in modifiedWeaponDictionary.Keys)
			{
				WeaponItem weapon = modifiedWeaponDictionary[id];

				Globals.SpoilerWrite($"{id}: {weapon.OverrideName}");
				Globals.SpoilerWrite($"Type: {Enum.GetName(typeof(WeaponType), weapon.Type)}");
				Globals.SpoilerWrite($"Damage: {weapon.Damage.MinValue} - {weapon.Damage.MaxValue}");
				Globals.SpoilerWrite($"Crit Chance / Multiplier: {weapon.CritChance} / {weapon.CritMultiplier}");
				Globals.SpoilerWrite($"Knockback / Speed / AOE: {weapon.Knockback} / {weapon.Speed} / {weapon.AddedAOE}");
				Globals.SpoilerWrite($"Added Precision / Defense: {weapon.AddedPrecision} / {weapon.AddedDefense}");
				Globals.SpoilerWrite($"Base / Min Mine Level Drop: {weapon.BaseMineLevelDrop} / {weapon.MinMineLevelDrop}");
				Globals.SpoilerWrite("---");
			}
			Globals.SpoilerWrite("");
		}
	}
}
