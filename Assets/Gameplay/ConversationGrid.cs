using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationGrid : MonoBehaviour
{
    // How many grid squares fit into the screen vertically
    public float GridSquaresVertical = 10f;
    public CardInstance cardPrefab;
    public Transform highlight;
    public Button confirmButton, rotateButton;
    public int BoardHeight = 4;
    public int BoardWidth = 6;
    public Transform gridSquarePrefab;
    public GameObject outlinePrefab;

    [HideInInspector]
    public float gridScale;

    private List<CardInstance> cards = new List<CardInstance>();
    private CardInstance activeCard;
    private List<Vector2> availableSpots = new List<Vector2>();
    private List<GameObject> spawnedOutlines = new List<GameObject>();

    private readonly Vector2[] directions = new[]
    {
        new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)

    };

    void Start()
    {
        gridScale = Camera.main.orthographicSize * 2 / GridSquaresVertical;
        transform.localScale = Vector3.one * gridScale;
        transform.position = new Vector3(
            (1-BoardWidth) * gridScale / 2 + 2, 
            (1-BoardHeight) * gridScale / 2 + 1,
            0);
        confirmButton.interactable = false;
        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                Transform gridSquare = Instantiate(gridSquarePrefab);
                gridSquare.SetParent(transform);
                gridSquare.localScale = Vector3.one;
                gridSquare.localPosition = new Vector3(x, y, 0);
                availableSpots.Add(new Vector2(x, y));
            }
        }
    }

    public void SpawnCard(Card c, CardInstance parent)
    {
        CardInstance cardInstance = Instantiate(cardPrefab);
        parent.draggedCard = cardInstance;
        cardInstance.Init(this, c, false, parent);
        if(activeCard != null)
        {
            activeCard.CancelMoveFromInventory();
        }
        activeCard = cardInstance;
        confirmButton.interactable = true;
    }

    public void OnCardRelease(CardInstance card)
    {
        if (!CanPlaceCard(card.x,card.y))
        {
            if (card.firstDrag) card.CancelMoveFromInventory();
            else card.CancelMoveSnapBack();
            return;
        }
        card.transform.localPosition = new Vector3(card.x, card.y, 0);
        rotateButton.gameObject.SetActive(true);
        rotateButton.transform.position = 
            Camera.main.WorldToScreenPoint(card.transform.position) +
            new Vector3(80,80,0);
    }

    private bool CanPlaceCard(int x, int y)
    {
        return availableSpots.Contains(new Vector2(x, y));
    }

    public void ConfirmMove()
    {
        cards.Add(activeCard);
        activeCard.Confirm();
        activeCard = null;
        confirmButton.interactable = false;
        rotateButton.gameObject.SetActive(false);

        // Recalculate available spots
        availableSpots.Clear();
        foreach (CardInstance card in cards)
        {
            foreach(Vector2 direction in directions)
            {
                int x = Mathf.RoundToInt(direction.x + card.x);
                int y = Mathf.RoundToInt(direction.y + card.y);
                if (x < 0 || y < 0 || x >= BoardWidth || y >= BoardHeight) continue;
                if (GetCard(x, y) != null) continue;
                Vector2 spot = new Vector2(x, y);
                if (availableSpots.Contains(spot)) continue;
                availableSpots.Add(spot);
            }
        }
        foreach (GameObject outline in spawnedOutlines) Destroy(outline);
        foreach(Vector2 spot in availableSpots)
        {
            GameObject outline = Instantiate(outlinePrefab);
            outline.transform.SetParent(transform);
            outline.transform.localPosition = spot;
            outline.transform.localScale = Vector3.one;
            spawnedOutlines.Add(outline);
        }
    }

    public void Rotate()
    {
        activeCard.transform.Rotate(0, 0, -90);
    }

    public void OnCardDrag(CardInstance card)
    {
        highlight.localPosition = new Vector3(card.x, card.y, 0);
        highlight.gameObject.SetActive(CanPlaceCard(card.x, card.y));
        rotateButton.gameObject.SetActive(false);
    }

    // Not optimal but like how many cards are gonna be on the field at once
    public CardInstance GetCard(int x, int y)
    {
        foreach(CardInstance card in cards)
        {
            if(card.x == x && card.y == y)
            {
                return card;
            }
        }

        return null;
    }

    public int GetCardScore()
    {
        int score = activeCard.card.GetScore();
        for(int i=0; i<4; i++)
        {
            CardManager.CardEdge edge = activeCard.card.cardData.edges[i];
            if (edge == CardManager.CardEdge.NONE) continue;
            Vector2 direction = directions[i];
            int x = Mathf.RoundToInt(direction.x + activeCard.x);
            int y = Mathf.RoundToInt(direction.y + activeCard.y);
            if (x < 0 || y < 0 || x >= BoardWidth || y >= BoardHeight) continue;
            CardInstance otherCard = GetCard(x, y);
            if (otherCard == null) continue;

        }
        return score;
    }
}
