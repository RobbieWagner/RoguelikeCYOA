using RobbieWagnerGames.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public partial class DialogueManager : MonoBehaviourSingleton<DialogueManager>
    {
        public Canvas dialogueCanvas;
        public TextMeshProUGUI dialogueText;
        public DialogueChoiceButton choiceButtonPrefab;
        [HideInInspector] public List<DialogueChoiceButton> currentChoiceButtons = new List<DialogueChoiceButton>();
        public RectTransform choiceButtonParent;

		[SerializeField] private Animator animatedDie1;
		[SerializeField] private Animator animatedDie2;
		[SerializeField] private TextMeshProUGUI diceResultText;
		[SerializeField] private RectTransform diceRollParent;

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
			//Debug.Log(currentSentence);
			DisplayChoices();
		}

		private IEnumerator DisplayDiceRoll(int die1, int die2, int roll, bool success) 
		{
			animatedDie1.SetInteger("Value", 0);
			animatedDie2.SetInteger("Value", 0);
			diceResultText.text = "Rolling...";
			diceRollParent.anchoredPosition = new Vector2(diceRollParent.sizeDelta.x, diceRollParent.anchoredPosition.y);
			diceRollParent.gameObject.SetActive(true);
			diceRollParent.DOAnchorPos(Vector2.zero, .5f);
			animatedDie1.GetComponent<RectTransform>().DOShakeAnchorPos(1.5f, 10);
			yield return animatedDie2.GetComponent<RectTransform>().DOShakeAnchorPos(1.5f, 10).WaitForCompletion();
			animatedDie1.SetInteger("Value", die1);
			animatedDie2.SetInteger("Value", die2);
			yield return diceResultText.DOFade(0, .5f).WaitForCompletion();
			diceResultText.text = roll.ToString();
			
			if (success)
				yield return diceResultText.DOColor(Color.green, .5f).WaitForCompletion();
			else
				yield return diceResultText.DOColor(Color.red, .5f).WaitForCompletion();

			yield return new WaitForSeconds(.5f);
			diceRollParent.DOAnchorPos(new Vector2(diceRollParent.sizeDelta.x, diceRollParent.anchoredPosition.y), .5f).OnComplete(() => diceRollParent.gameObject.SetActive(false));
		}
	}
}