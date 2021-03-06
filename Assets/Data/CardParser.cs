using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardParser
{
    public static Dictionary<CardManager.CardId, CardManager.CardData> GetCardData()
    {
        TextAsset textFile = (TextAsset)Resources.Load("cardlist");
        string txt = textFile.text;
        Dictionary<string, CardManager.CardId> cardIds = new Dictionary<string, CardManager.CardId>();
        Dictionary<string, CardManager.CardFamily> cardFamilies = new Dictionary<string, CardManager.CardFamily>();
        Dictionary<string, CardManager.CardEdge> cardEdges = new Dictionary<string, CardManager.CardEdge>();

        for (int i = 0; i < (int)CardManager.CardId.COUNT; i++)
        {
            CardManager.CardId id = (CardManager.CardId)i;
            cardIds[id.ToString()] = id;
        }

        for (int i = 0; i < (int)CardManager.CardFamily.COUNT; i++)
        {
            CardManager.CardFamily family = (CardManager.CardFamily)i;
            cardFamilies[family.ToString()] = family;
        }

        for (int i = 0; i < (int)CardManager.CardFamily.COUNT; i++)
        {
            CardManager.CardEdge edge = (CardManager.CardEdge)i;
            cardEdges[edge.ToString()] = edge;
        }

        Dictionary<CardManager.CardId, CardManager.CardData> cardDict =
            new Dictionary<CardManager.CardId, CardManager.CardData>();
        foreach (string line in txt.Split('\n'))
        {
            string[] parts = line.Split(',');
            CardManager.CardData data = new CardManager.CardData
            {
                id = cardIds[parts[0].Trim()],
                name = parts[1].Trim(),
                family = cardFamilies[parts[2].Trim()],
                baseScore = int.Parse(parts[3].Trim()),
                edges = new CardManager.CardEdge[]
                {
                    cardEdges[parts[4].Trim()],
                    cardEdges[parts[5].Trim()],
                    cardEdges[parts[6].Trim()],
                    cardEdges[parts[7].Trim()]
                }
            };
            cardDict.Add(cardIds[parts[0].Trim()], data);
        }
        return cardDict;
    }
}
