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
        BLUEBUB,
        LOREN,
        NONE,
    }

    public static List<Name> validNpcNames = new List<Name> { Name.STRAWBUB, Name.LAVENDER, Name.BLUEBUB, Name.LOREN, Name.NONE };

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
            return new List<CardId>{ CardId.WHY, CardId.NO, CardId.SHUSH, CardId.SILENCE, CardId.SLEEPY };
        }
        if (name == Name.LAVENDER) {
            return new List<CardId> { CardId.FUNFACT, CardId.ANECDOTE, CardId.FOLLOWUP, CardId.WOW, CardId.COOL };
        }
        if (name == Name.BLUEBUB) {
            return new List<CardId> { CardId.EXAMPLE, CardId.WHY, CardId.HOW, CardId.SAME, CardId.COOL };
        }
        if (name == Name.LOREN) {
            return new List<CardId> { CardId.FUNFACT, CardId.SILENCE, CardId.SLEEPY };
        }
        else {
            return null;
        }
    }

    public static Sprite GetSprite(Name name) {
        return characterToSprite[name];
    }

}
