using RobbieWagnerGames.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public class CharacterManager : MonoBehaviourSingleton<CharacterManager>
    {
        private List<Character> allCharacterOptions
        {
            get
            {
				Character[] loadedCharacters = Resources.LoadAll<Character>("Characters");
				return new List<Character>(loadedCharacters);
			}
        }

		[HideInInspector] public Character currentCharacter;

		[SerializeField] private Canvas characterSelectionScreen;
        [SerializeField] private CharacterSelectionButton characterSelectionButton;
        [SerializeField] private LayoutGroup characterOptionsParent;
        private List<CharacterSelectionButton> currentCharacterOptions = new List<CharacterSelectionButton>();

        public void OpenCharacterSelectionScreen()
        {
            ClearCharacterOptions();

			List<Character> characterOptions = GetRandomCharacters(allCharacterOptions, 3);

			foreach (Character character in characterOptions)
            {
                CharacterSelectionButton newButton = Instantiate(characterSelectionButton, characterOptionsParent.transform);
                newButton.character = character;
                newButton.button.onClick.AddListener(() => MakeCharacterSelection(newButton.character));
                newButton.characterDisplayImage.sprite = character.characterSprite;
                newButton.characterNameText.text = character.characterName;
                currentCharacterOptions.Add(newButton);
            }
        }

		private List<Character> GetRandomCharacters(List<Character> sourceList, int count)
		{
			List<Character> randomCharacters = new List<Character>();
			List<Character> tempList = new List<Character>(sourceList);

			for (int i = 0; i < count && tempList.Count > 0; i++)
			{
				int randomIndex = Random.Range(0, tempList.Count);
				randomCharacters.Add(tempList[randomIndex]);
				tempList.RemoveAt(randomIndex);
			}

			return randomCharacters;
		}


		private void ClearCharacterOptions()
        {
            foreach(CharacterSelectionButton button in currentCharacterOptions)
                Destroy(button.gameObject);
            currentCharacterOptions.Clear();
        }

        public void MakeCharacterSelection(Character character)
        {
            currentCharacter = character;
			ClearCharacterOptions();
			GameManager.Instance.OnCharacterSelected();
        }
    }
}