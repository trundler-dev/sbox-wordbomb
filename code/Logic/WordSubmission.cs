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
	}
}
