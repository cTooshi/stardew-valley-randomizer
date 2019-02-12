﻿using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Randomizer
{
	/// <summary>The mod entry point</summary>
	public class ModEntry : Mod
	{
		public static Dictionary<string, bool> configDict;

		public PossibleSwap[] PossibleSwaps = {
			new PossibleSwap("Pierre", "Lewis"),
			new PossibleSwap("Wizard", "Sandy"),
			new PossibleSwap("Willy", "Pam"),
			new PossibleSwap("Abigail", "Marnie"),
			new PossibleSwap("MrQi", "Gunther"),
			new PossibleSwap("Marlon", "Governor"),
			new PossibleSwap("Caroline", "Evelyn"),
			new PossibleSwap("Pam", "Haley"),
			new PossibleSwap("Morris", "Krobus"),
			new PossibleSwap("Gus", "Elliott"),
			new PossibleSwap("Linus", "Pam"),
			new PossibleSwap("Kent", "Pierre"),
			new PossibleSwap("Sandy", "Maru"),
			new PossibleSwap("Sebastian", "Wizard"),
			new PossibleSwap("Jas", "Vincent"),
			new PossibleSwap("Krobus", "Dwarf"),
			new PossibleSwap("Leah", "Marnie"),
			new PossibleSwap("Henchman", "Bouncer"),
			new PossibleSwap("Harvey", "Gus"),
			new PossibleSwap("Bouncer", "Gunther"),
			new PossibleSwap("Gunther", "Governor"),
			new PossibleSwap("Evelyn", "Jodi"),
			new PossibleSwap("George", "Wizard"),
			new PossibleSwap("Emily", "Marnie"),
			new PossibleSwap("Sam", "Linus"),
			new PossibleSwap("Alex", "Gus"),
			new PossibleSwap("Penny", "Sandy"),
			new PossibleSwap("Morris", "Governor"),
			new PossibleSwap("Haley", "Alex"),
			new PossibleSwap("Harvey", "Maru"),
			new PossibleSwap("Abigail", "Sebastian"),
			new PossibleSwap("Penny", "Sam"),
			new PossibleSwap("Leah", "Elliott"),
			new PossibleSwap("Shane", "Emily"),
			new PossibleSwap("Shane", "Pam")
		};

		private AssetLoader _modAssetLoader;
		private AssetEditor _modAssetEditor;

		/// <summary>The mod entry point, called after the mod is first loaded</summary>
		/// <param name="helper">Provides simplified APIs for writing mods</param>
		public override void Entry(IModHelper helper)
		{
			Globals.ModRef = this;

			//config file read in
			string[] config = System.IO.File.ReadAllLines("Mods/Randomizer/RandomizerSettings.txt");
			configDict = new Dictionary<string, bool>();

			foreach (string line in config)
			{
				string[] tokens = line.Split('=');
				if (tokens.Length != 2 || line.Trim().StartsWith("//")) continue;
				configDict.Add(tokens[0].Trim().ToLower(), (tokens[1].Trim().ToLower() == "true"));
			}

			this._modAssetLoader = new AssetLoader(this);
			this._modAssetEditor = new AssetEditor(this);
			helper.Content.AssetLoaders.Add(this._modAssetLoader);
			helper.Content.AssetEditors.Add(this._modAssetEditor);

			this.PreLoadReplacments();
			SaveEvents.AfterLoad += (sender, args) => this.CalculateAllReplacements();

			bool canReplaceMusic = configDict.ContainsKey("music") ? configDict["music"] : true;
			if (canReplaceMusic) { GameEvents.UpdateTick += (sender, args) => this.TryReplaceSong(); }

			bool canReplaceRain = configDict.ContainsKey("rain") ? configDict["rain"] : true;
			if (!canReplaceRain) { helper.Events.GameLoop.DayEnding += _modAssetLoader.ReplaceRain; }
		}

		/// <summary>
		/// Loads the replacements that can be loaded before a game is selected
		/// </summary>
		public void PreLoadReplacments()
		{
			this._modAssetLoader.CalculateReplacementsBeforeLoad();
			this._modAssetEditor.CalculateEditsBeforeLoad();
		}

		/// <summary>
		/// Does all the randomizer replacements that take place after a game is loaded
		/// </summary>
		public void CalculateAllReplacements()
		{
			//Seed is pulled from farm name
			byte[] seedvar = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(Game1.player.farmName));
			int seed = BitConverter.ToInt32(seedvar, 0);

			this.Monitor.Log($"Seed Set: {seed}");

			Globals.RNG = new Random(seed);
			Globals.SpoilerLog = new SpoilerLogger(Game1.player.farmName);

			// Make replacements and edits
			this._modAssetLoader.CalculateReplacements();
			this._modAssetEditor.CalculateEdits();
			Globals.SpoilerLog.WriteFile();

			// Invalidate all replaced and edited assets so they are reloaded
			this._modAssetLoader.InvalidateCache();
			this._modAssetEditor.InvalidateCache();
		}

		/// <summary>
		/// The last song that played/is playing
		/// </summary>
		private string _lastCurrentSong { get; set; }

		/// <summary>
		/// Attempts to replace the current song with a different one
		/// If the song was barely replaced, it doesn't do anything
		/// </summary>
		public void TryReplaceSong()
		{
			//Game1.addHUDMessage(new HUDMessage(Game1.currentSong?.Name));

			string currentSong = Game1.currentSong?.Name;
			if (this._modAssetEditor.MusicReplacements.TryGetValue(currentSong?.ToLower() ?? "", out string value) && _lastCurrentSong != currentSong)
			{
				_lastCurrentSong = value;
				Game1.changeMusicTrack(value);
			}
		}
	}
}