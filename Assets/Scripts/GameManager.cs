using RobbieWagnerGames.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
		//TODO add character selection
        //[HideInInspector] 
		
        public List<TextAsset> randomScenarios;
		private TextAsset currentScenario;

		protected override void Awake()
		{
			base.Awake();

			CharacterManager.Instance.OpenCharacterSelectionScreen();
		}

		public void OnCharacterSelected()
		{
			PlayNextScenario();
		}

		private void PlayNextScenario()
		{
			currentScenario = randomScenarios[UnityEngine.Random.Range(0, randomScenarios.Count)];
		
			DialogueManager.Instance.StartStory(currentScenario);
		}
	}
}