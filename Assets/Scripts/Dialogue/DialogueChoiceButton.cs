using RobbieWagnerGames.Audio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public class DialogueChoiceButton : MonoBehaviour, IPointerEnterHandler
    {
        public Button button;
        public TextMeshProUGUI buttonText;

		public void OnPointerEnter(PointerEventData eventData)
		{
			BasicAudioManager.Instance?.Play(AudioSourceName.UINav);
		}
	}
}