using RobbieWagnerGames.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public partial class DialogueManager : MonoBehaviourSingleton<DialogueManager>
    {
        public Canvas dialogueCanvas;
        public TextMeshProUGUI dialogueText;
        public DialogueChoiceButton choiceButtonPrefab;
    }
}