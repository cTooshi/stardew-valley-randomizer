using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer
{
    public class CropNameRandomizer
    {
		public static List<string> GenerateVegetableNames(int numberOfNames)
		{
			List<string> adjectives = new List<string>
			{
				"Juicy",
				"Spicy",
				"Sweet",
				"Hot",
				"Cold",
				"Bright",
				"Moist",
				"Funky",
				"Bok",
				"Ancient",
				"Sour",
				"Miracle",
				"Pelican",
				"Stardew",
				"Prickly",
				"Sunny",
				"Salty",
				"Savory",
				"Wild",
				"Common",
				"Tiny",
				"Giant",
				"Young"
			};

			List<string> prefixes = new List<string>
			{
				"Mc",
				"Joja",
				"Pome",
				"Cauli",
				"Pota",
				"Star",
				"Drago",
				"Vege",
				"Rue",
				"Pyne",
				"Dew",
				"Choco",
				"Coffe",
				"Radi",
				"Cabba",
				"Toma",
				"Aarti",
				"Egg",
				"Pump",
				"Coco",
				"Cucu",
				"Alfa",
				"Carro",
				"Squa",
				"Zucchi",
				"Bana",
				"Apri",
				"Lemo",
				"Passion",
				"Huckle",
				"Kiwi",
				"Lime",
				"Boysen",
				"Crann",
				"Clementi",
				"Honey",
				"Pear",
				"Rasp",
				"Water",
				"Tange"
			};

			List<string> suffixes = new List<string>
			{
				"granite",
				"nana",
				"flour",
				"bean",
				"tato",
				"froot",
				"barb",
				"berry",
				"apple",
				"dew",
				"korn",
				"lait",
				"y",
				"cado",
				"to",
				"ranth",
				"choke",
				"trout",
				"yam",
				"lli",
				"iander",
				"onion",
				"snip",
				"melon",
				"plum",
				"ngo",
				"quat",
				"rind",
				"rillo",
				"rant",
				"pepper",
				"rry",
				"fig",
				"jube",
				"dropp",
				"loupe",
				"paya",
				"pear",
				"rene",
				"root"
			};

			return (CreateNameFromPieces(numberOfNames, adjectives, prefixes, suffixes));
		}

        public static List<string> GenerateFlowerNames(int numberOfNames)
        {
            List<string> adjectives = new List<string>
            {
				"Fragrant",
				"Ugly",
				"Sweet",
				"Fairy",
				"Morning",
				"Creeping",
				"Wild",
				"Giant",
				"Common",
				"Rough",
				"Field",
				"Lesser"
			};

            List<string> prefixes = new List<string>
            {
				"Daffo",
				"Mary",
				"Snap",
				"Vio",
				"Canna",
				"Aza",
				"Hibi",
				"Hya",
				"Cro",
				"Jasmi",
				"Bella",
				"Poi",
				"Olea",
				"Hem",
				"Night",
				"Rhodo",
				"Frangi",
				"Bell",
				"Forget-me-"
			};

            List<string> suffixes = new List<string>
            {
				"ster",
				"hock",
				"fodil",
				"lily",
				"iris",
				"cissus",
				"drop",
				"suckle",
				"mellia",
				"lilac",
				"rose",
				"synth",
				"bane",
				"laurel",
				"weed",
				"dendrite",
				"nettle",
				"flower",
				"wort"
			};

			return (CreateNameFromPieces(numberOfNames, adjectives, prefixes, suffixes));
		}

        private static List<string> CreateNameFromPieces(int numberOfNames, List<string> adjectives, List<string> prefixes, List<string> suffixes)
        {
			List<string> createdNames = new List<string>();
			string newName = "default name";

			for (int i = 0; i < numberOfNames; i++)
			{
				if (prefixes.Count > 0 && suffixes.Count > 0)
				{
					newName = $"{Globals.RNGGetAndRemoveRandomValueFromList(prefixes)}{Globals.RNGGetAndRemoveRandomValueFromList(suffixes)}";
					if (newName.StartsWith("Mc")) newName = $"Mc{newName.Substring(2, 1).ToUpper()}{newName.Substring(3)}";
				} 
				if (Globals.RNGGetNextBoolean(10) && adjectives.Count > 0) newName = $"{Globals.RNGGetAndRemoveRandomValueFromList(adjectives)} {newName}";
				createdNames.Add(newName);
				Globals.ConsoleWrite(createdNames[i]);
			}

			return createdNames;
        }

    }
}
