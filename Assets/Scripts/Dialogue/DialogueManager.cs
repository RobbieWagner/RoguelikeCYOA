using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using RobbieWagnerGames.Utilities;
using RobbieWagnerGames.Dialogue;
using RobbieWagnerGames.Managers;
using UnityEngine.InputSystem;
using System.Linq;

namespace RobbieWagnerGames.RoguelikeCYOA
{
	public partial class DialogueManager : MonoBehaviourSingleton<DialogueManager>
	{
		private Story currentStory = null;
		private string currentSentence;
		private Coroutine typingCoroutine;
		[SerializeField] private float typeSpeed = 0.05f;

		protected override void Awake()
		{
			base.Awake();

			InputManager.Instance.GetAction(ActionMapName.DIALOGUE, "Select").performed += OnSelect;
		}

		#region story
		public bool StartStory(TextAsset storyText)
		{
			if (currentStory == null)
			{
				Debug.Log("starting story");
				currentStory = DialogueConfigurer.CreateStory(storyText);
				ContinueStory();
				InputManager.Instance.EnableActionMap(ActionMapName.DIALOGUE);
				return true;
			}
			return false;
		}

		private void ContinueStory()
		{
			if (currentStory.canContinue)
			{
				Debug.Log("line");
				currentSentence = currentStory.Continue();

				// Stop any existing typing coroutine
				if (typingCoroutine != null)
				{
					StopCoroutine(typingCoroutine);
				}

				// Start typing out the new sentence
				typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
				StartCoroutine(FinishTypingSentence());
			}
			else
			{
				EndStory();
			}
		}

		public void EndStory()
		{
			currentStory = null;
			dialogueText.text = "";

			InputManager.Instance.DisableActionMap(ActionMapName.DIALOGUE);
			ClearChoices();
		}
		#endregion

		#region choices
		// Call this when a choice is selected
		public void MakeChoice(int choiceIndex)
		{
			Choice choice = currentStory.currentChoices[choiceIndex];

			// Check for roll tags (e.g., #DESP3)
			if (choice.tags != null && choice.tags.Any())
			{
				foreach (string tag in choice.tags)
				{
					if (tag.StartsWith("#"))
					{
						string[] parts = tag.Substring(1).Split('#'); // Handle multiple tags if needed
						foreach (string part in parts)
						{
							if (part.StartsWith("ROLL"))
								ParseRollTag(part);
							else if (part.Length >= 4)
							{ // e.g., "DSP3"
								string stat = part.Substring(0, 3); // First 3 letters (e.g., "DSP")
								int threshold = int.Parse(part.Substring(4));
								HandleStatCheck(stat, threshold);
							}
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
			Debug.Log($"{stat}, {roll}, {success}");
			currentStory.variablesState["lastRollSuccess"] = success ? 1 : 0;
		}
		#endregion

		#region controls
		private void OnSelect(InputAction.CallbackContext context)
		{
			Debug.Log("hi");

			if (typingCoroutine == null)
			{
				if (currentStory.currentChoices.Count == 0)
					ContinueStory();
			}
			else
			{
				StopCoroutine(typingCoroutine);
				typingCoroutine = null;
			}
		}

		#endregion
	}
}