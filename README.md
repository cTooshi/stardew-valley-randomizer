# Stardew Valley Randomizer (More Random Edition)

An update for cTooshi's Stardew Valley Randomizer to fix errors, add new features and make the existing features more random.

## Installation

Right now you have to build it yourself from the source code. You can find instructions on the Stardew Valley wiki on how to set up Visual Studio to do so: https://stardewvalleywiki.com/Modding:Modder_Guide/Get_Started

## Changes from Original Randomizer

* Bundle randomization
  * New bundles for each room with random items selected from themed pools and random number of those items required
  * Some bundles are completely random and select from most items in the game.
* Crafting recipe randomization
  * Recipes are now created based on randomly selected items from a pool (not randomly selected premade recipes)
  * Crafting difficulty is balanced based on necessity of the item and difficulty of crafting the item in vanilla
* Crop randomization
  * Crops, including fruits, vegetables, and flowers, have randomized (made-up) names, descriptions, prices (for both seeds and crops), and attributes (trellises, scythe needed, etc.)
  * This also includes custom images for all seeds and saplings to reduce confusion
* Fish randomization
  * Fish have randomized (made-up) names, difficulty, and behavior. 
  * Locations, time-of-day, weather, and seasons are swapped as well.
* Forageable randomization
  * Forageables for every season and location are now randomly selected from all forageables + fruit (normally from trees)
  * Every forageable appears at least once per year, and some may appear more than once
* Fruit tree randomization
  * Fruit tree saplings are now item saplings that grow a randomly selected item
* Blueprint randomization
  * Farm buildings that you get from Robin now choose from a more random pool of resources/items instead of a set list
  * This does not yet include anything you don't get from Robin (Obelisks, the Gold Clock, etc.)
* Music randomization
  * Most in-game songs and ambience are now randomly swapped 1 to 1 with another in-game song or ambience
* Quest randomization
  * Quest givers, required items, and rewards are randomly selected.
  * Help Wanted quests are unaffected, but the randomized item names should appear as expected.
* Spoiler log
  * A spoiler log can be generated to see info about what was randomized
  * You must turn on this option in the settings to generate the log
* Misc
  * Bug fixes to prevent game crashing
  * Different variants of randomized rain can now appear in one playthrough (previously only one type per playthrough)

## Possible Future Features
* Graphics changes
  * New sprites for crops (item and growing sprites)
  * New sprites for fish
* Palette randomization (if possible)
  * Randomly shift the color of the in-game graphics towards a different hue
* Randomize items that you dig up from the dirt
* Randomize weapon stats/names
* Randomize enemy stats
* Foragable and fish locations are a single setting rather than separate
* Crafting recipes and levels you learn them are in a single setting and can be separated
  
## Known Issues
* There are issues with bundles sometimes - when generating a new file, make sure you do so without first loading another file. If you have issues viewing a bundle due to this, it's fixable by editing the save file and putting more slots for the particiular bundle having the issue.
* Fruit tree prices seem to be hard-coded, so they're vanilla.
