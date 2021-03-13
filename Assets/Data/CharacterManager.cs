using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;

public class CharacterManager : MonoBehaviour
{
    public enum Name {
        STRAWBUB,
        LAVENDER,
        NONE,
    }

    [Serializable]
    public struct CharacterSprite {
        public Name character;
        public Sprite sprite;
    }

    public CharacterSprite[] characterSprites;
    static Dictionary<Name, Sprite> characterToSprite;

    private void Awake() {
        characterToSprite = new Dictionary<Name, Sprite>();
        foreach (CharacterSprite character in characterSprites) {
            characterToSprite[character.character] = character.sprite;
        }
    }

    public static List<CardId> GetCharacterCardPreset(Name name) {
        if (name == Name.STRAWBUB) {
            return new List<CardId>{ CardId.WHY, CardId.SAME, CardId.NO, CardId.SHUSH, CardId.SILENCE, CardId.SLEEPY };
        }
        if (name == Name.LAVENDER) {
            return new List<CardId> { CardId.FUNFACT, CardId.ANECDOTE, CardId.FOLLOWUP, CardId.TEST, CardId.WOW, CardId.COOL };
        }
        else {
            return null;
        }
    }

    public static Sprite GetSprite(Name name) {
        return characterToSprite[name];
    }

}
