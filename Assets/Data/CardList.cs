using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public enum CardID
    {
        EXAMPLE
    }

    [Serializable]
    public struct CardData
    {
        public CardID id;
        public string name;
    }

    public CardData[] cards;
}
