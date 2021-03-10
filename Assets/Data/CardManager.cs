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
        ENTHUSIASM,
        SLEEPY,
        COUNT // number of different cards
    }

    public enum CardFamily 
    {
        QUESTION,
        ELABORATE,
        DIRECT,
        ENERGY,
        COOLDOWN,
    }

    public enum CardEdge 
    {
        NONE,
        QUESTION,
        INFO,
        ENERGY_HIGH,
        ENERGY_LOW,
    }

    [Serializable]
    public struct CardFamilyColor {
        public CardFamily family;
        public Color color;
    }

    [Serializable]
    public struct CardEdgeSprite {
        public CardEdge edge;
        public Sprite sprite;
    }

    [Serializable]
    public struct CardData
    {
        public CardId id;
        public string name;
        public CardFamily family;
        public int baseScore;
        public CardEdge[] edges;
    }

    public CardFamilyColor[] cardFamilyColors;
    public CardEdgeSprite[] cardEdgeSprites;
    public CardData[] cards;
    static Dictionary<CardId, CardData> cardIdToData;
    static Dictionary<CardFamily, Color> cardFamilyToColor;
    static Dictionary<CardEdge, Sprite> cardEdgeToSprite;

    private void Awake() {
        cardIdToData = new Dictionary<CardId, CardData>();
        foreach (CardData card in cards) {
            cardIdToData[card.id] = card;
        }
        cardFamilyToColor = new Dictionary<CardFamily, Color>();
        foreach (CardFamilyColor family in cardFamilyColors) {
            cardFamilyToColor[family.family] = family.color;
        }
        cardEdgeToSprite = new Dictionary<CardEdge, Sprite>();
        foreach (CardEdgeSprite edge in cardEdgeSprites) {
            cardEdgeToSprite[edge.edge] = edge.sprite;
        }
    }

    public static CardData GetDataById(CardId id) {
        return cardIdToData[id];
    }

    public static Color GetColorByFamily(CardFamily family) {
        return cardFamilyToColor[family];
    }

    public static Sprite GetSpriteByEdge(CardEdge edge) {
        return cardEdgeToSprite[edge];
    }

    public static CardId GetRandomCardId() {
        return (CardId)UnityEngine.Random.Range(0, (int)CardId.COUNT);
    }
}
