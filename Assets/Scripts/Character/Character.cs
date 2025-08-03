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
        public Sprite characterSprite;

        public static CharacterStat ConvertStringToCharacterStat(string statName)
        {
			if (statName.Equals("DSP"))
				return CharacterStat.DESPERATION;
			else if (statName.Equals("SBJ"))
				return CharacterStat.SUBJUGATION;
			else if (statName.Equals("SAN"))
				return CharacterStat.SANITY;
			else if (statName.Equals("VIG"))
				return CharacterStat.VIGILANCE;
            return CharacterStat.NONE;
		}
	}
}