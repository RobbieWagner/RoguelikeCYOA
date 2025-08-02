using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using RobbieWagnerGames.Utilities;
using RobbieWagnerGames.Dialogue;
using RobbieWagnerGames.Managers;
using UnityEngine.InputSystem;
using System.Linq;
using System.Text.RegularExpressions;

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
			dialogueCanvas.enabled = false;
			InputManager.Instance.GetAction(ActionMapName.DIALOGUE, "Select").performed += OnSelect;
		}

		#region story
		public bool StartStory(TextAsset storyText)
		{
			if (currentStory == null)
			{
				currentStory = DialogueConfigurer.CreateStory(storyText);
				dialogueCanvas.enabled = true;
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
			dialogueCanvas.enabled = false;
		}
		#endregion

		#region choices
		public void MakeChoice(int choiceIndex, List<string> tags)
		{
			ClearChoices();

			foreach (string tag in tags)
			{
				// Process tag before making choice
				if (!string.IsNullOrEmpty(tag))
				{
					if (tag.StartsWith("ROLL"))
						ParseRollTag(tag);
				}
			}

			currentStory.ChooseChoiceIndex(choiceIndex);

			ContinueStory();
		}

		private void ParseTags(List<string> tags)
		{
			Debug.Log("has tags");
			Debug.Log($"{tags[0]}");
			foreach (string tag in tags)
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

		private void ParseRollTag(string tagContent)
		{
			// Extract parameters from "ROLL(STAT,value)" format
			var match = Regex.Match(tagContent, @"ROLL\((\w+),(\d+)\)");
			if (match.Success && match.Groups.Count == 3)
			{
				string stat = match.Groups[1].Value;
				int threshold = int.Parse(match.Groups[2].Value);
				HandleStatCheck(stat, threshold);
			}
		}

		private void HandleStatCheck(string stat, int threshold)
		{
			int playerStatValue = 0;

			if (stat.Equals("DSP"))
				playerStatValue = CharacterManager.Instance.currentCharacter.stats[CharacterStat.DESPERATION];
			else if (stat.Equals("SBJ"))
				playerStatValue = CharacterManager.Instance.currentCharacter.stats[CharacterStat.SUBJUGATION];
			else if (stat.Equals("SAN"))
				playerStatValue = CharacterManager.Instance.currentCharacter.stats[CharacterStat.SANITY];
			else if (stat.Equals("VIG"))
				playerStatValue = CharacterManager.Instance.currentCharacter.stats[CharacterStat.VIGILANCE];

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