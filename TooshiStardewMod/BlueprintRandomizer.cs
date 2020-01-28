using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Randomizes the blueprints - that is, the buildings that Robin can build for you
	/// </summary>
	public class BlueprintRandomizer
	{
		/// <summary>
		/// Randomize the blueprints
		/// </summary>
		/// <returns>The dictionary to use to replace the assets</returns>
		public static Dictionary<string, string> Randomize()
		{
			Dictionary<string, string> blueprintChanges = new Dictionary<string, string>();

			List<Buildings> buildings = Enum.GetValues(typeof(Buildings)).Cast<Buildings>().ToList();
			Building currentBuilding = null;
			List<Building> buildingsToAdd = new List<Building>();

			List<int> idsToDisallowForAnimalBuildings = ItemList.GetAnimalProducts().Select(x => x.Id).ToList();
			idsToDisallowForAnimalBuildings.AddRange(new List<int>
			{
				(int)ObjectIndexes.GreenSlimeEgg,
				(int)ObjectIndexes.BlueSlimeEgg,
				(int)ObjectIndexes.RedSlimeEgg,
				(int)ObjectIndexes.PurpleSlimeEgg
			});

			Item resource1, resource2;
			ItemAndMultiplier itemChoice;
			List<ItemAndMultiplier> listChoice;

			foreach (Buildings buildingType in buildings)
			{
				resource1 = ItemList.GetRandomResourceItem();
				resource2 = ItemList.GetRandomResourceItem(new int[] { resource1.Id });

				switch (buildingType)
				{
					case Buildings.Silo:
						currentBuilding = new Building(
							"Silo",
							new List<ItemAndMultiplier>
							{
								new ItemAndMultiplier(resource1, Range.GetRandomValue(2, 3)),
								new ItemAndMultiplier(resource2),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
							},
							100,
							"3/3/-1/-1/-2/-1/null/Silo/Allows you to cut and store grass for feed./Buildings/none/48/128/-1/null/Farm"
						);
						break;
					case Buildings.Mill:
						currentBuilding = new Building(
							"Mill",
							new List<ItemAndMultiplier>
							{
								new ItemAndMultiplier(resource1, 3),
								new ItemAndMultiplier(resource2, 2),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
							},
							2500,
							"4/2/-1/-1/-2/-1/null/Mill/Allows you to create flour from wheat and sugar from beets./Buildings/none/64/128/-1/null/Farm"
						);
						break;
					case Buildings.ShippingBin:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 3),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
						});

						currentBuilding = new Building(
							"Shipping Bin",
							new List<ItemAndMultiplier>
							{
								itemChoice
							},
							250,
							"2/1/-1/-1/-1/-1/null/Shipping Bin/Throw items inside to sell them overnight./Buildings/none/48/80/-1/null/Farm",
							"false/0"
						);
						break;
					case Buildings.Coop:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 5),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});

						currentBuilding = new Building(
							"Coop",
							new List<ItemAndMultiplier>
							{
								itemChoice,
								new ItemAndMultiplier(resource2, Range.GetRandomValue(2, 3))
							},
							4000,
							"6/3/1/2/2/2/Coop/Coop/Houses 4 coop-dwelling animals./Buildings/none/64/96/4/null/Farm"
						);
						break;
					case Buildings.BigCoop:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 3),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});

						currentBuilding = new Building(
							"Big Coop",
							new List<ItemAndMultiplier>
							{
								itemChoice,
								new ItemAndMultiplier(resource2, 7)
							},
							10000,
							"6/3/1/2/2/2/Coop2/Big Coop/Houses 8 coop-dwelling animals. Comes with an incubator. Unlocks ducks./Upgrades/Coop/64/96/8/null/Farm"
						);
						break;
					case Buildings.DeluxeCoop:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 9),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.LargeTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});

						currentBuilding = new Building(
							"Deluxe Coop",
							new List<ItemAndMultiplier>
							{
								itemChoice,
								new ItemAndMultiplier(resource2, 4)
							},
							20000,
							"6/3/1/2/2/2/Coop3/Deluxe Coop/Houses 12 coop-dwelling animals. Comes with an auto-feed system. Unlocks rabbits./Upgrades/Big Coop/64/96/12/null/Farm"
						);
						break;
					case Buildings.Barn:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 5),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});

						currentBuilding = new Building(
							"Barn",
							new List<ItemAndMultiplier>
							{
								itemChoice,
								new ItemAndMultiplier(resource2, Range.GetRandomValue(2, 3))
							},
							6000,
							"7/4/1/3/3/3/Barn/Barn/Houses 4 barn-dwelling animals./Buildings/none/96/96/4/null/Farm"
						);
						break;
					case Buildings.BigBarn:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 3),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});

						currentBuilding = new Building(
							"Big Barn",
							new List<ItemAndMultiplier>
							{
								itemChoice,
								new ItemAndMultiplier(resource2, 7)
							},
							12000,
							"7/4/1/3/4/3/Barn2/Big Barn/Houses 8 barn-dwelling animals. Allows animals to give birth. Unlocks goats./Upgrades/Barn/96/96/8/null/Farm"
						);
						break;
					case Buildings.DeluxeBarn:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 9),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.LargeTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});

						currentBuilding = new Building(
							"Deluxe Barn",
							new List<ItemAndMultiplier>
							{
								itemChoice,
								new ItemAndMultiplier(resource2, 4)
							},
							25000,
							"7/4/1/3/4/3/Barn3/Deluxe Barn/Houses 12 barn-dwelling animals. Comes with an auto-feed system. Unlocks sheep and pigs./Upgrades/Big Barn/96/96/12/null/Farm"
						);
						break;
					case Buildings.SlimeHutch:
						currentBuilding = new Building(
							"Slime Hutch",
							new List<ItemAndMultiplier>
							{
								new ItemAndMultiplier(resource1, 9),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements), 2),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
							},
							10000,
							"11/6/5/5/-1/-1/SlimeHutch/Slime Hutch/Raise up to 20 slimes. Fill water troughs and slimes will create slime balls./Buildings/none/96/96/20/null/Farm"
						);
						break;
					case Buildings.Shed:
						listChoice = Globals.RNGGetRandomValueFromList(new List<List<ItemAndMultiplier>> {
							new List<ItemAndMultiplier> { new ItemAndMultiplier(resource1, 5) },
							new List<ItemAndMultiplier>
							{
								new ItemAndMultiplier(resource1, 3),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
							}
						});

						currentBuilding = new Building(
							"Shed",
							listChoice,
							15000,
							"7/3/3/2/-1/-1/Shed/Shed/An empty building. Fill it with whatever you like! The interior can be decorated./Buildings/none/96/96/20/null/Farm"
						);
						break;
					case Buildings.StoneCabin:
						currentBuilding = new Building(
							"Stone Cabin",
							GetRequiredItemsForCabin(),
							100,
							"5/3/2/2/-1/-1/Cabin/Cabin/A home for a friend! Subsidized by the town agricultural fund./Buildings/none/96/96/20/null/Farm",
							"false/0"
						);
						break;
					case Buildings.PlankCabin:
						currentBuilding = new Building(
							"Plank Cabin",
							GetRequiredItemsForCabin(),
							100,
							"5/3/2/2/-1/-1/Cabin/Cabin/A home for a friend! Subsidized by the town agricultural fund./Buildings/none/96/96/20/null/Farm",
							"false/0"
						);
						break;
					case Buildings.LogCabin:
						currentBuilding = new Building(
							"Log Cabin",
							GetRequiredItemsForCabin(),
							100,
							"5/3/2/2/-1/-1/Cabin/Cabin/A home for a friend! Subsidized by the town agricultural fund./Buildings/none/96/96/20/null/Farm",
							"false/0"
						);
						break;
					case Buildings.Well:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new ItemAndMultiplier(resource1, 3),
							new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
						});

						currentBuilding = new Building(
							"Well",
							new List<ItemAndMultiplier>
							{
								itemChoice
							},
							1000,
							"3/3/-1/-1/-1/-1/null/Well/Provides a place for you to refill your watering can./Buildings/none/32/32/-1/null/Farm"
						);
						break;
					case Buildings.FishPond:
						currentBuilding = new Building(
							"Fish Pond",
							new List<ItemAndMultiplier>
							{
								new ItemAndMultiplier(resource1, 2),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.SmallTimeRequirements), 2),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.SmallTimeRequirements), 2)
							},
							5000,
							"5/5/-1/-1/-2/-1/null/Fish Pond/Raise fish and harvest their produce. Fish multiply over time./Buildings/none/76/78/10/null/Farm",
							"false/2"
						);
						break;
					default:
						Globals.ConsoleWrite($"ERROR: Unhandled building: {buildingType.ToString()}");
						continue;
				}
				buildingsToAdd.Add(currentBuilding);
			}

			Globals.SpoilerWrite("==== BLUEPRINTS ====");
			foreach (Building building in buildingsToAdd)
			{
				blueprintChanges.Add(building.Name, building.ToString());

				Globals.SpoilerWrite($"{building.Name} - {building.Price}G");
				Globals.SpoilerWrite(GetRequiredItemsSpoilerString(building));
				Globals.SpoilerWrite("===");
			}
			Globals.SpoilerWrite("");

			return blueprintChanges;
		}

		/// <summary>
		/// Gets the required items for a cabin - applies to any cabin
		/// </summary>
		/// <returns />
		private static List<ItemAndMultiplier> GetRequiredItemsForCabin()
		{
			Item resource = ItemList.GetRandomResourceItem();
			Item easyItem = Globals.RNGGetRandomValueFromList(
				ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.MediumTimeRequirements, new int[resource.Id])
			);

			return Globals.RNGGetRandomValueFromList(new List<List<ItemAndMultiplier>> {
				new List<ItemAndMultiplier> { new ItemAndMultiplier(resource, 2) },
				new List<ItemAndMultiplier>
				{
					new ItemAndMultiplier(resource),
					new ItemAndMultiplier(easyItem)
				}
			});
		}

		/// <summary>
		/// Gets the required items string for use in the spoiler log
		/// </summary>
		/// <param name="building">The building</param>
		/// <returns />
		private static string GetRequiredItemsSpoilerString(Building building)
		{
			string requiredItemsSpoilerString = "";
			foreach (RequiredItem item in building.RequiredItems)
			{
				requiredItemsSpoilerString += $" - {item.Item.Name}: {item.NumberOfItems}";
			}
			return requiredItemsSpoilerString;
		}
	}
}
