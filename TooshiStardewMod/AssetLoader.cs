using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class AssetLoader : IAssetLoader
	{
		private readonly ModEntry _mod;
		private readonly Dictionary<string, string> _replacements = new Dictionary<string, string>();


		public AssetLoader(ModEntry mod)
		{
			this._mod = mod;
		}

		public bool CanLoad<T>(IAssetInfo asset)
		{
			// Check if the assets has a replacement in the dictionary
			foreach (KeyValuePair<string, string> replacement in this._replacements)
			{
				if (asset.AssetNameEquals(replacement.Key))
				{
					return true;
				}
			}

			return false;
		}

		public T Load<T>(IAssetInfo asset)
		{
			// Normalize the asset name


			string normalizedAssetName = this._mod.Helper.Content.NormaliseAssetName(asset.AssetName);

			// Try to get the replacement asset from the replacements dictionary
			if (this._replacements.TryGetValue(normalizedAssetName, out string replacementAsset))
			{
				return this._mod.Helper.Content.Load<T>(replacementAsset, ContentSource.ModFolder);

			}

			throw new InvalidOperationException($"Unknown asset: {asset.AssetName}.");

		}


		private void AddReplacement(string originalAsset, string replacementAsset)
		{
			// Normalize the asset name so the keys are consistent
			string normalizedAssetName = this._mod.Helper.Content.NormaliseAssetName(originalAsset);

			// Add the replacement to the dictionary
			this._replacements[normalizedAssetName] = replacementAsset;
		}


		public void InvalidateCache()
		{
			// Invalidate all replaced assets so that the changes are reapplied
			foreach (string assetName in this._replacements.Keys)
			{
				this._mod.Helper.Content.InvalidateCache(assetName);
			}
		}

		public void CalculateReplacementsBeforeLoad(Random placeHolderNumber)
		{
			// Replace title screen
			this.AddReplacement("Minigames/TitleButtons", "Assets/Minigames/TitleButtons");
		}

		public void CalculateReplacementsOnCreation(Random rng2)
		{
			/* Add palm trees to farm
            string[] possibleFarms = { "", "_Combat", "_Fishing", "_Foraging", "_Mining"};
            for (int i = 0; i < possibleFarms.Length; i++) { 
            this.AddReplacement($"Maps/Farm{possibleFarms[i]}", $"Assets/Maps/Farm{possibleFarms[i]}Palm");
                this._mod.Monitor.Log($"Farm{possibleFarms[i]} has palm trees!");
            }
           */
		}

		/// <summary>
		/// Asset replacements
		/// TODO: make a class for this so that it's not in one giant file
		/// </summary>
		public void CalculateReplacements()
		{
			// Clear any previous replacements
			this._replacements.Clear();

			//AddReplacement("Maps/springobjects", "Assets/Maps/springobjects");

			if (ModEntry.configDict.ContainsKey("animal skins") ? ModEntry.configDict["animal skins"] : true)
			{
				// Replace critters
				switch (Globals.RNG.Next(0, 4))
				{
					case 0:
						this.AddReplacement("TileSheets/critters", "Assets/TileSheets/crittersBears");
						break;
					case 1:
						this.AddReplacement("TileSheets/critters", "Assets/TileSheets/crittersseagullcrow");
						break;
					case 2:
						this.AddReplacement("TileSheets/critters", "Assets/TileSheets/crittersWsquirrelPseagull");
						break;
					case 3:
						this.AddReplacement("TileSheets/critters", "Assets/TileSheets/crittersBlueBunny");
						break;
				}

				//Change an animal to bear 
				int isPet = Globals.RNG.Next(1, 4);
				String[] Animal = new string[4]; Animal[0] = "Pig"; Animal[1] = "Goat"; Animal[2] = "Brown Cow"; Animal[3] = "White Cow";
				String[] Pet = new string[2]; Pet[0] = "cat"; Pet[1] = "dog";

				if (isPet == 1)
				{
					int petRng = Globals.RNG.Next(0, 2);
					this.AddReplacement($"Animals/{Pet[petRng]}", "Assets/Characters/BearDog");
					//this._mod.Monitor.Log($"Bear is {Pet[petRng]}");
				}
				if (isPet == 2)
				{
					this.AddReplacement($"Animals/horse", "Assets/Characters/BearHorse");
					//this._mod.Monitor.Log($"Bear is Horse");
				}
				else
				{
					int animalRng = Globals.RNG.Next(0, 5);
					this.AddReplacement($"Animals/{Animal[animalRng]}", "Assets/Characters/Bear");
					this.AddReplacement($"Animals/Baby{Animal[animalRng]}", "Assets/Characters/BabyBear");
					//this._mod.Monitor.Log($"Bear is {Animal[animalRng]}");

				}

			}

			//Randomize Schedules needs work
			this.AddReplacement("Characters/schedules/Linus", "Assets/Characters/schedules/LinusToSaloonSpring");
			this.AddReplacement("Characters/schedules/Caroline", "Assets/Characters/schedules/CarolineToEastTown");

			//Randomize Mines
			if (ModEntry.configDict.ContainsKey("mine layouts") ? ModEntry.configDict["mine layouts"] : true)
			{
				int mineSwapsRemaining = Globals.RNG.Next(20, 61);
				int mineLevel = Globals.RNG.Next(1, 41);
				int mineLevel1_29;
				int mineLevel31_39;


				while (mineSwapsRemaining > 0)
				{
					mineLevel1_29 = Globals.RNG.Next(1, 30);
					mineLevel31_39 = Globals.RNG.Next(31, 40);

					if (mineLevel < 30 && (mineLevel != 5 && mineLevel != 10 && mineLevel != 15 && mineLevel != 20 && mineLevel != 25))
					{
						if (mineLevel1_29 != 5 && mineLevel1_29 != 10 && mineLevel1_29 != 15 && mineLevel1_29 != 20 && mineLevel1_29 != 25)
						{
							this.AddReplacement($"Maps/Mines/{mineLevel}", $"Assets/Maps/Mines/{mineLevel1_29}New");
							mineLevel = Globals.RNG.Next(1, 41); //rng.Next(1, 40);
							mineSwapsRemaining--;
						}
						mineLevel = Globals.RNG.Next(1, 41); //rng.Next(1, 40);
						mineSwapsRemaining--;

					}

					else if (mineLevel > 30 && mineLevel < 40 && mineLevel != 35)
					{
						if (mineLevel31_39 != 35)
						{
							this.AddReplacement($"Maps/Mines/{mineLevel}", $"Assets/Maps/Mines/{mineLevel31_39}New");
							mineLevel = Globals.RNG.Next(1, 41); //rng.Next(1, 40);
							mineSwapsRemaining--;
						}
						mineLevel = Globals.RNG.Next(1, 41); //rng.Next(1, 40);
						mineSwapsRemaining--;

					}
					else
					{
						this.AddReplacement($"Maps/Mines/{mineLevel}", $"Assets/Maps/Mines/{mineLevel}New");

						mineLevel = Globals.RNG.Next(1, 41);
						mineSwapsRemaining--;
					}

				}
			}

			// Replace rain
			if (ModEntry.configDict.ContainsKey("rain") ? ModEntry.configDict["rain"] : true)
			{
				switch (Globals.RNG.Next(0, 6))
				{
					case 0:
						this.AddReplacement("TileSheets/rain", "Assets/TileSheets/PotatoRain");
						break;
					case 1:
						this.AddReplacement("TileSheets/rain", "Assets/TileSheets/EggplantRain");
						break;
					case 2:
						this.AddReplacement("TileSheets/rain", "Assets/TileSheets/CatsAndDogsRain");
						break;
					case 3:
						this.AddReplacement("TileSheets/rain", "Assets/TileSheets/JunimoRain");
						break;
					case 4:
						this.AddReplacement("TileSheets/rain", "Assets/TileSheets/SkullRain");
						break;
					case 5:
						break;
				}
			}

			// Character swaps
			if (ModEntry.configDict.ContainsKey("npc skins") ? ModEntry.configDict["npc skins"] : true)
			{
				// Keep track of all swaps made
				Dictionary<string, string> currentSwaps = new Dictionary<string, string>();

				// Copy the array of possible swaps to a new list
				List<PossibleSwap> possibleSwaps = this._mod.PossibleSwaps.ToList();

				// Make swaps until either a random number of swaps are made or we run out of possible swaps to make
				int swapsRemaining = Globals.RNG.Next(5, 11);
				while (swapsRemaining > 0 && possibleSwaps.Any())
				{
					// Get a random possible swap
					int index = Globals.RNG.Next(0, possibleSwaps.Count);
					PossibleSwap swap = possibleSwaps[index];

					// Remove it from the list so it isn't chosen again
					possibleSwaps.RemoveAt(index);

					// Check if the characters haven't been swapped yet
					if (currentSwaps.ContainsKey(swap.FirstCharacter) || currentSwaps.ContainsKey(swap.SecondCharacter))
					{
						continue;
					}

					// Add the swap to the dictionary
					currentSwaps[swap.FirstCharacter] = swap.SecondCharacter;
					currentSwaps[swap.SecondCharacter] = swap.FirstCharacter;
					this._mod.Monitor.Log($"Swapping {swap.FirstCharacter} and {swap.SecondCharacter}");

					// Add the replacements
					this.AddReplacement($"Characters/{swap.FirstCharacter}", $"Assets/Characters/{swap.SecondCharacter}");
					this.AddReplacement($"Characters/{swap.SecondCharacter}", $"Assets/Characters/{swap.FirstCharacter}");
					this.AddReplacement($"Portraits/{swap.FirstCharacter}", $"Assets/Portraits/{swap.SecondCharacter}");
					this.AddReplacement($"Portraits/{swap.SecondCharacter}", $"Assets/Portraits/{swap.FirstCharacter}");

					// Decrement the number of swaps remaining
					swapsRemaining--;
				}
			}
		}
	}
}