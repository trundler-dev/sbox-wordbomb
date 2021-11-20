using Sandbox;

namespace WordBomb.Logic
{
	/// <summary>
	/// WordSubmission is a wrapper for the submission string, and contains a method for evaluating a submission's validity.
	/// </summary>
	public class WordSubmission
	{
		public string Submission { get; private set; }

		public WordSubmission( string submission )
		{
			Submission = submission;
		}

		public bool IsValidWord()
		{
			var player = Game.Instance.ActivePlayer;
			var wordDictionary = Game.Instance.Dict;

			bool isValid = !wordDictionary.WordIsUsed( this ) && wordDictionary.WordIsValid( this ) && wordDictionary.PromptInWord( this, player );
			if ( isValid )
			{
				wordDictionary.AddSubmission( this );
				return true;
			}
			return false;
		}

		public bool SubmitWord()
		{
			if ( IsValidWord() )
			{
				Game.Instance.ActivePlayer.SubmittedWord = true;
				Game.Instance.ActivePlayer.LastSubmittedWord = Submission;
				return true;
			}
			return false;
		}
	}
}
