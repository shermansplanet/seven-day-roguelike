using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public enum CardId
    {
        EXAMPLE
    }

    [Serializable]
    public struct CardData
    {
        public CardId id;
        public string name;
    }

    public CardData[] cards;
    static Dictionary<CardId, CardData> cardIdToData = new Dictionary<CardId, CardData>();

    private void Awake() {
        cardIdToData = new Dictionary<CardId, CardData>();
        foreach (CardData card in cards) {
            cardIdToData[card.id] = card;
        }
    }

    public static CardData GetDataById(CardId id) {
        return cardIdToData[id];
    }
}
