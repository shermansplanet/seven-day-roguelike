using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public enum CardId
    {
        EXAMPLE,
        WHY,
        TEST,
    }

    public enum CardFamily 
    {
        QUESTION,
        ELABORATE,
        DIRECT,
    }

    [Serializable]
    public struct CardFamilyColor {
        public CardFamily cardFamily;
        public Color color;
    }

    [Serializable]
    public struct CardData
    {
        public CardId id;
        public string name;
        public CardFamily cardFamily;
    }

    public CardFamilyColor[] cardFamilyColors;
    public CardData[] cards;
    static Dictionary<CardId, CardData> cardIdToData;
    static Dictionary<CardFamily, Color> cardFamilyToColor;

    private void Awake() {
        cardIdToData = new Dictionary<CardId, CardData>();
        foreach (CardData card in cards) {
            cardIdToData[card.id] = card;
        }
        cardFamilyToColor = new Dictionary<CardFamily, Color>();
        foreach (CardFamilyColor family in cardFamilyColors) {
            cardFamilyToColor[family.cardFamily] = family.color;
        }
    }

    public static CardData GetDataById(CardId id) {
        return cardIdToData[id];
    }

    public static Color GetColorByFamily(CardFamily family) {
        return cardFamilyToColor[family];
    }
}
