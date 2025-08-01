using RobbieWagnerGames.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
		//TODO add character selection
        //[HideInInspector] 
		public Character currentCharacter;
        public List<TextAsset> randomScenarios;
		private TextAsset currentScenario;

		protected override void Awake()
		{
			base.Awake();

			PlayNextScenario();
		}

		private void PlayNextScenario()
		{
			currentScenario = randomScenarios[UnityEngine.Random.Range(0, randomScenarios.Count)];
		
			DialogueManager.Instance.StartStory(currentScenario);
		}
	}
}