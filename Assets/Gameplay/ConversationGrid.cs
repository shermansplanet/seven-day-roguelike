﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationGrid : MonoBehaviour
{
    // How many grid squares fit into the screen vertically
    public float GridSquaresVertical = 10f;
    public CardInstance cardPrefab;
    public SpriteRenderer sprite;
    public Transform highlight;

    [HideInInspector]
    public float gridScale;

    private List<CardInstance> cards = new List<CardInstance>();

    void Start()
    {
        gridScale = Camera.main.orthographicSize * 2 / GridSquaresVertical;
        transform.localScale = Vector3.one * gridScale;

    }

    public void SpawnCard(Card c, CardInstance parent)
    {
        CardInstance cardInstance = Instantiate(cardPrefab);
        parent.draggedCard = cardInstance;
        cards.Add(cardInstance);
        cardInstance.Init(this, c, false);
    }

    public void OnCardRelease(CardInstance card)
    {
        if(GetCard(card.x, card.y) != null)
        {
            card.CancelMove();
            return;
        }
        card.transform.localPosition = new Vector3(card.x, card.y, 0);
    }

    public void OnCardDrag(CardInstance card)
    {
        highlight.localPosition = new Vector3(card.x, card.y, 0);
    }

    // Not optimal but like how many cards are gonna be on the field at once
    public CardInstance GetCard(int x, int y)
    {
        foreach(CardInstance card in cards)
        {
            if(card.x == x && card.y == y && card.beingDragged == false)
            {
                return card;
            }
        }

        return null;
    }
}
