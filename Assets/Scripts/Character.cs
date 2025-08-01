using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RoguelikeCYOA
{
    public enum CharacterStat
    {
        NONE = -1,
        DESPERATION = 0, 
        SUBJUGATION = 1, 
        SANITY = 2, 
        VIGILANCE = 3
	}

    [CreateAssetMenu(menuName = "RoguelikeCYOA/Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        [SerializedDictionary("Stat", "Modifier")] public SerializedDictionary<CharacterStat, int> stats;
    }
}