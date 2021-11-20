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

		public bool PromptInWord( WordSubmission submission, Player player )
		{
			return submission.Submission.Contains( player.Prompt, StringComparison.OrdinalIgnoreCase );
		}

		public bool WordIsUsed( WordSubmission submission )
		{
			return UsedWordSubmissions.Contains( submission.Submission );
		}

		public bool WordIsValid( WordSubmission submission )
		{
			string response = DictionaryApi.GetWord( submission.Submission ).Result;

			if ( response.Equals( "404" ) ) return false;

			return response != null;
		}

		public void AddSubmission( WordSubmission submission )
		{
			UsedWordSubmissions.Add( submission.Submission );
		}

		public void ClearUsedWordSubmissions()
		{
			UsedWordSubmissions.Clear();
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
					new System.Uri( Consts.DICTIONARY_URI + word ) );
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
