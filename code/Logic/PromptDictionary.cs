using System;
using System.Collections.Generic;

namespace WordBomb.Logic
{
	public class PromptDictionary
	{
		public static List<string> TwoLetterPrompts = new()
		{
			"AC",
			"AR",
			"CE",
			"CR",
			"DA",
			"DR",
			"EX",
			"FR",
			"GU",
			"HE",
			"IL",
			"JO",
			"KE",
			"LI",
			"LO",
			"ME",
			"MO",
			"NI",
			"ND",
			"ON",
			"OV",
			"PE",
			"RA",
			"RE",
			"SA",
			"SE",
			"SO",
			"SH",
			"TR",
			"TO",
			"UR",
			"VE",
			"WO",
			"YA",
			"ZA"
		};

		public static List<string> ThreeLetterPrompts = new()
		{
			"ARE",
			"BRE",
			"DIS",
			"ECK",
			"FAR",
			"GLE",
			"HIS",
			"IDE",
			"LTS",
			"MAR",
			"NER",
			"OPE",
			"PLE",
			"RAN",
			"RES",
			"SIN",
			"STR",
			"TRE",
			"WAR",
			"YOU"
		};

		public static string GetRandomTwoLetterPrompt()
		{
			var random = new Random();
			int r = random.Next( TwoLetterPrompts.Count );
			return TwoLetterPrompts[r];
		}

		public static string GetRandomThreeLetterPrompt()
		{
			var random = new Random();
			int r = random.Next( ThreeLetterPrompts.Count );
			return ThreeLetterPrompts[r];
		}
	}
}
