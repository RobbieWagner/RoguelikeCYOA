using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public class CharacterSelectionButton : MonoBehaviour
    {
        public Button button;
        public Image characterDisplayImage;
        public TextMeshProUGUI characterNameText;

        [HideInInspector] public Character character;
    }
}