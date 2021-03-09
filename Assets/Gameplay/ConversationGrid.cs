﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationGrid : MonoBehaviour
{
    // How many grid squares fit into the screen vertically
    public float GridSquaresVertical = 10f;
    public CardInstance cardPrefab;
    public SpriteRenderer sprite;
    public Transform highlight;
    public Button confirmButton, rotateButton;

    [HideInInspector]
    public float gridScale;

    private List<CardInstance> cards = new List<CardInstance>();
    private CardInstance activeCard;

    void Start()
    {
        gridScale = Camera.main.orthographicSize * 2 / GridSquaresVertical;
        transform.localScale = Vector3.one * gridScale;
        confirmButton.interactable = false;
    }

    public void SpawnCard(Card c, CardInstance parent)
    {
        CardInstance cardInstance = Instantiate(cardPrefab);
        parent.draggedCard = cardInstance;
        cards.Add(cardInstance);
        cardInstance.Init(this, c, false);
        if(activeCard != null)
        {
            Destroy(activeCard.gameObject);
        }
        activeCard = cardInstance;
        confirmButton.interactable = true;
    }

    public void OnCardRelease(CardInstance card)
    {
        if(GetCard(card.x, card.y) != null)
        {
            card.CancelMove();
            return;
        }
        card.transform.localPosition = new Vector3(card.x, card.y, 0);
        rotateButton.gameObject.SetActive(true);
        rotateButton.transform.position = 
            Camera.main.WorldToScreenPoint(card.transform.position) +
            new Vector3(80,80,0);
    }

    public void ConfirmMove()
    {
        activeCard.draggable = false;
        activeCard = null;
        confirmButton.interactable = false;
        rotateButton.gameObject.SetActive(false);
    }

    public void Rotate()
    {
        activeCard.transform.Rotate(0, 0, -90);
    }

    public void OnCardDrag(CardInstance card)
    {
        highlight.localPosition = new Vector3(card.x, card.y, 0);
        rotateButton.gameObject.SetActive(false);
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
