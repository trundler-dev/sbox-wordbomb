using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordBomb.Helpers;

namespace WordBomb.Logic
{
	/// <summary>
	/// WordDictionary contains words already used this game, and methods for checking the validity of word submission.
	/// </summary>
	public class WordDictionary
	{
		public List<string> UsedWordSubmissions;

		public WordDictionary()
		{
			UsedWordSubmissions = new();
		}
	}

	/// <summary>
	/// DictionaryApi wraps Sandbox.Internal.Http and exposes GetWord() which calls a Dictionary API.
	/// </summary>
	public class DictionaryApi
	{
		public async static Task<string> GetWord( string word )
		{
			try
			{
				Sandbox.Internal.Http http = new(
					new Uri( Consts.DICTIONARY_URI + word ) );
				string message = await http.GetStringAsync();

				Log.Info( "DictionaryApi: " + message );

				http.Dispose();
				return message;
			}
			catch ( Exception )
			{
				return "404";
			}
		}

	}
}
