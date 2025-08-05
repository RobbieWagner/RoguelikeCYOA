using RobbieWagnerGames.Audio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public class CharacterSelectionButton : MonoBehaviour, IPointerEnterHandler
    {
        public Button button;
        public Image characterDisplayImage;
        public TextMeshProUGUI characterNameText;

        [HideInInspector] public Character character;

        public void InitializeUI(Sprite characterSprite, string name)
        {
            characterDisplayImage.sprite = characterSprite;
            characterNameText.text = name;
        }

		public void OnPointerEnter(PointerEventData eventData)
		{
			BasicAudioManager.Instance?.Play(AudioSourceName.UINav);
		}
	}
}