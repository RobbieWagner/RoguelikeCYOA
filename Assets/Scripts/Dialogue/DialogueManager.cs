using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using RobbieWagnerGames.Utilities;
using RobbieWagnerGames.Dialogue;

namespace RobbieWagnerGames.RoguelikeCYOA
{
	public partial class DialogueManager : MonoBehaviourSingleton<DialogueManager>
	{
		private Story currentStory;
		private string currentSentence;

		protected override void Awake()
		{
			base.Awake();
		}

		public bool StartStory(TextAsset storyText)
		{
			if (currentStory == null)
			{
				currentStory = DialogueConfigurer.CreateStory(storyText);
				ContinueStory();
				return true;
			}
			return false;
		}

		private void ContinueStory()
		{
			if (currentStory.canContinue)
			{
				currentSentence = currentStory.Continue();
				Debug.Log(currentSentence);

				// yield return DisplaySentenceCharacterByCharacter();
			}
		}

		public void EndStory()
		{
			currentStory = null;
		}

		// Call this when a choice is selected
		public void MakeChoice(int choiceIndex)
		{
			Choice choice = currentStory.currentChoices[choiceIndex];

			// Check for roll tags (e.g., #DESP3)
			foreach (string tag in choice.tags)
			{
				if (tag.StartsWith("#"))
				{
					string[] parts = tag.Substring(1).Split('#'); // Handle multiple tags if needed
					foreach (string part in parts)
					{
						if (part.StartsWith("ROLL"))
						{
							ParseRollTag(part);
						}
						else if (part.Length >= 4)
						{ // e.g., "DSP3"
							string stat = part.Substring(0, 3); // First 3 letters (e.g., "DSP")
							int threshold = int.Parse(part.Substring(4));
							HandleStatCheck(stat, threshold);
						}
					}
				}
			}

			currentStory.ChooseChoiceIndex(choiceIndex);
			ContinueStory();
		}

		private void ParseRollTag(string tag)
		{
			// Extract stat and threshold (e.g., "ROLL(DESP,3)")
			string[] parts = tag.Split(new char[] { '(', ',', ')' });
			string stat = parts[1];
			int threshold = int.Parse(parts[2]);
			HandleStatCheck(stat, threshold);
		}

		private void HandleStatCheck(string stat, int threshold)
		{
			int playerStatValue = 0;

			if (stat.Equals("DSP"))
				playerStatValue = GameManager.Instance.currentCharacter.stats[CharacterStat.DESPERATION];
			else if (stat.Equals("SBJ"))
				playerStatValue = GameManager.Instance.currentCharacter.stats[CharacterStat.SUBJUGATION];
			else if (stat.Equals("SAN"))
				playerStatValue = GameManager.Instance.currentCharacter.stats[CharacterStat.SANITY];
			else if (stat.Equals("VIG"))
				playerStatValue = GameManager.Instance.currentCharacter.stats[CharacterStat.VIGILANCE];

			// Roll 2d6 (PbtA-style)
			int roll = Random.Range(1, 7) + Random.Range(1, 7);
			bool success = (playerStatValue + roll >= threshold);

			// Pass result back to Ink (using a global variable)
			currentStory.variablesState["lastRollSuccess"] = success ? 1 : 0;
		}
	}
}