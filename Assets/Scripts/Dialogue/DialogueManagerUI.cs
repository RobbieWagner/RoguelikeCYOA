using RobbieWagnerGames.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;

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

        private void AddChoices()
        {
			for (int i = 0; i < currentStory.currentChoices.Count; i++)
			{
				Choice choice = currentStory.currentChoices[i];
				DialogueChoiceButton choiceButton = Instantiate(choiceButtonPrefab, choiceButtonParent);

				// Set button text
				choiceButton.buttonText.text = choice.text;

				// Set button click handler
				int choiceIndex = i; // Capture the index for the closure
				choiceButton.button.onClick.AddListener(() => MakeChoice(choiceIndex));

				currentChoiceButtons.Add(choiceButton);
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
			AddChoices();
		}
	}
}