using RobbieWagnerGames.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Linq;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public partial class DialogueManager : MonoBehaviourSingleton<DialogueManager>
    {
        public Canvas dialogueCanvas;
        public TextMeshProUGUI dialogueText;
        public DialogueChoiceButton choiceButtonPrefab;
        [HideInInspector] public List<DialogueChoiceButton> currentChoiceButtons = new List<DialogueChoiceButton>();
        public RectTransform choiceButtonParent;

		private void ClearChoices()
		{
			foreach (DialogueChoiceButton button in currentChoiceButtons)
			{
				Destroy(button.gameObject);
			}
			currentChoiceButtons.Clear();
		}

        private void DisplayChoices()
        {
			for (int i = 0; i < currentStory.currentChoices.Count; i++)
			{
				Choice choice = currentStory.currentChoices[i];

				// Extract display text and tag
				List<string> parts = choice.text.Split(new[] { '_' }, 2).ToList();
				string displayText = parts[0].Trim();
				List<string> tags = parts.Count > 1 ? parts.GetRange(1, parts.Count-1) : null;

				// Create button with clean text
				DialogueChoiceButton choiceButton = Instantiate(choiceButtonPrefab, choiceButtonParent);
				choiceButton.buttonText.text = displayText;
				currentChoiceButtons.Add(choiceButton);

				// Store tag with button's listener
				int choiceIndex = i;
				choiceButton.button.onClick.AddListener(() => MakeChoice(choiceIndex, tags));
			}
		}

		private IEnumerator TypeSentence(string sentence)
		{
			dialogueText.text = "";
			foreach (char letter in sentence.ToCharArray())
			{
				dialogueText.text += letter;
				yield return new WaitForSeconds(typeSpeed);
			}

			typingCoroutine = null;
		}

		private IEnumerator FinishTypingSentence()
		{
			// Wait for typing to complete
			while (typingCoroutine != null)
				yield return null;

			yield return null;
			dialogueText.text = currentSentence;
			Debug.Log(currentSentence);
			DisplayChoices();
		}
	}
}