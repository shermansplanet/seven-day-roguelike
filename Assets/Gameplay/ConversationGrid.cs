using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationGrid : MonoBehaviour
{
    // How many grid squares fit into the screen vertically
    public float GridSquaresVertical = 10f;
    public CardGrid cardGridPrefab;
    public Transform highlight;
    public Button confirmButton, rotateButton;
    public Text scoreText;
    public const int BoardHeight = 4;
    public const int BoardWidth = 6;
    public Transform gridSquarePrefab;
    public GameObject outlinePrefab;
    public int winStage = 0;

    private HashSet<Vector2> blockedSquares;

    [HideInInspector]
    public float gridScale;

    [HideInInspector]
    public List<CardGrid> cards = new List<CardGrid>();

    [HideInInspector]
    public CardGrid activeCard;

    [HideInInspector]
    public List<Vector2> availableSpots = new List<Vector2>();

    private List<GameObject> spawnedOutlines = new List<GameObject>();

    private readonly Vector2[] directions = new[]
    {
        new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)

    };

    public struct GameState
    {
        public List<CardGrid> cards;
    }

    public GameState GetGameState()
    {
        return new GameState
        {
            cards = cards
        };
    }

    public void SetGameState(NPC npc)
    {
        GameState state = npc.gameState;
        foreach(CardGrid card in state.cards)
        {
            CardGrid newInstance = Instantiate(cardGridPrefab);
            newInstance.Init(this, card.card, null, CardGrid.CardSource.SAVED);
            newInstance.x = card.x;
            newInstance.y = card.y;
            newInstance.rotation = card.rotation;
            newInstance.transform.localPosition = new Vector3(card.x, card.y, 0);
            newInstance.transform.Rotate(0, 0, -90 * card.rotation);
            newInstance.confirmedOnBoard = true;
            cards.Add(newInstance);
        }

        blockedSquares = new HashSet<Vector2>(npc.blockedSpots);

        if (cards.Count > 0) UpdateAvailableSpots();
    
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
                if (blockedSquares.Contains(new Vector2(x, y))) continue;
                Transform gridSquare = Instantiate(gridSquarePrefab);
                gridSquare.SetParent(transform);
                gridSquare.localScale = Vector3.one;
                gridSquare.localPosition = new Vector3(x, y, 0);
                if(cards.Count == 0) availableSpots.Add(new Vector2(x, y));
            }
        }
    }

    public void SpawnCard(Card c, CardInventory parent)
    {
        CardGrid cardGrid = Instantiate(cardGridPrefab);
        if(parent) parent.draggedCard = cardGrid;
        cardGrid.Init(this, c, parent, parent == null ? CardGrid.CardSource.OTHER : CardGrid.CardSource.PLAYER);
        if(activeCard != null)
        {
            activeCard.CancelMoveFromInventory();
        }
        activeCard = cardGrid;
        confirmButton.interactable = true;
    }

    public void OnCardRelease(CardGrid card)
    {
        if (!CanPlaceCard(card.x,card.y))
        {
            if (card.firstDrag) {
                card.CancelMoveFromInventory();
                return;
            }
            card.CancelMoveSnapBack();
        }
        UpdateCardBonus(card);
        card.transform.localPosition = new Vector3(card.x, card.y, 0);
        rotateButton.gameObject.SetActive(true);
        rotateButton.transform.position = 
            Camera.main.WorldToScreenPoint(card.transform.position) +
            new Vector3(80,80,0);
    }

    private void UpdateCardBonus(CardGrid card)
    {
        int baseScore = card.card.GetScore();
        int extra = GetCardScore() - baseScore;
        card.SetCenterText(
            baseScore.ToString() +
            (extra < 0 ? "" : "+") +
            extra.ToString());
    }

    private bool CanPlaceCard(int x, int y)
    {
        return availableSpots.Contains(new Vector2(x, y));
    }

    public void ConfirmMove()
    {
        cards.Add(activeCard);
        activeCard.Confirm();
        GameManager.score += GetCardScore();
        scoreText.text = "Score: " + GameManager.score.ToString();
        activeCard.ResetCenterText();

        activeCard = null;

        confirmButton.interactable = false;
        rotateButton.gameObject.SetActive(false);

        UpdateAvailableSpots();
    }

    private void UpdateAvailableSpots()
    {
        // Recalculate available spots
        availableSpots.Clear();
        foreach (CardGrid card in cards)
        {
            foreach (Vector2 direction in directions)
            {
                int x = Mathf.RoundToInt(direction.x + card.x);
                int y = Mathf.RoundToInt(direction.y + card.y);
                if (x < 0 || y < 0 || x >= BoardWidth || y >= BoardHeight) continue;
                if (GetCard(x, y) != null) continue;
                Vector2 spot = new Vector2(x, y);
                if (blockedSquares.Contains(spot)) continue;
                if (availableSpots.Contains(spot)) continue;
                availableSpots.Add(spot);
            }
        }
        foreach (GameObject outline in spawnedOutlines) Destroy(outline);
        foreach (Vector2 spot in availableSpots)
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
        activeCard.rotation = (activeCard.rotation + 1) % 4;
        UpdateCardBonus(activeCard);
    }

    public void OnCardDrag(CardGrid card)
    {
        ResetEdgeAndNeighborsColor();
        highlight.localPosition = new Vector3(card.x, card.y, 0);
        highlight.gameObject.SetActive(CanPlaceCard(card.x, card.y));
        rotateButton.gameObject.SetActive(false);
    }

    // Not optimal but like how many cards are gonna be on the field at once
    public CardGrid GetCard(int x, int y)
    {
        foreach(CardGrid card in cards)
        {
            if(card.x == x && card.y == y)
            {
                return card;
            }
        }

        return null;
    }

    public bool IsNextToEdge(int edgeIndex) {
        Vector2 direction = directions[edgeIndex];
        int x = Mathf.RoundToInt(direction.x + activeCard.x);
        int y = Mathf.RoundToInt(direction.y + activeCard.y);
        if (x < 0 || y < 0 || x >= BoardWidth || y >= BoardHeight || blockedSquares.Contains(new Vector2(x,y))) return true;
        else return false;
    }

    public CardGrid GetAdjacentCard(int edgeIndex) {
        Vector2 direction = directions[edgeIndex];
        int x = Mathf.RoundToInt(direction.x + activeCard.x);
        int y = Mathf.RoundToInt(direction.y + activeCard.y);
        if (x < 0 || y < 0 || x >= BoardWidth || y >= BoardHeight) return null;
        return GetCard(x, y);
    }

    private void SetEdgeColorBonus(int edgeIndex) {
        CardGrid otherCard = GetAdjacentCard(edgeIndex);
        activeCard.SetEdgeColor((edgeIndex + 4 - activeCard.rotation) % 4, Color.green);
        otherCard?.SetEdgeColor((edgeIndex + 6 - otherCard.rotation) % 4, Color.green);
    }

    private void SetEdgeColorPenalty(int edgeIndex) {
        CardGrid otherCard = GetAdjacentCard(edgeIndex);
        activeCard.SetEdgeColor((edgeIndex + 4 - activeCard.rotation) % 4, Color.red);
        otherCard?.SetEdgeColor((edgeIndex + 6 - otherCard.rotation) % 4, Color.red);
    }

    private void ResetEdgeAndNeighborsColor() {
        for (int i = 0; i < 4; i++) {
            CardGrid otherCard = GetAdjacentCard(i);
            activeCard.SetEdgeColor((i + 4 - activeCard.rotation) % 4, Color.black);
            otherCard?.SetEdgeColor((i + 6 - otherCard.rotation) % 4, Color.black);
        }
    }

    public int GetCardScore(bool updateColors = true)
    {
        int score = activeCard.card.GetScore();
        if(updateColors) ResetEdgeAndNeighborsColor();
        for(int i = 0; i < 4; i++)
        {
            CardManager.CardEdge edge = activeCard.card.cardData.edges[(i + 4 - activeCard.rotation) % 4];
            CardGrid otherCard = GetAdjacentCard(i);
            if (!IsNextToEdge(i) && otherCard == null) continue;
            CardManager.CardEdge otherEdge = CardManager.CardEdge.NONE;
            if (otherCard != null) otherEdge = otherCard.card.cardData.edges[(i + 6 - otherCard.rotation) % 4];
            if(edge == otherEdge && edge != CardManager.CardEdge.NONE){
                if (edge == CardManager.CardEdge.QUESTION) {
                    score += -2;
                    if (updateColors) SetEdgeColorPenalty(i);
                }
                else if (edge != CardManager.CardEdge.ENERGY_HIGH && edge != CardManager.CardEdge.ENERGY_LOW) {
                    score += 2;
                    if (updateColors) SetEdgeColorBonus(i);
                }
            }
            if(edge != otherEdge)
            {
                if (edge == CardManager.CardEdge.INFO && otherEdge == CardManager.CardEdge.QUESTION) {
                    score += 2;
                    if (updateColors) SetEdgeColorBonus(i);
                }
                else if (
                    !(edge == CardManager.CardEdge.INFO && otherEdge == CardManager.CardEdge.NONE)
                    && !(edge == CardManager.CardEdge.NONE && otherEdge == CardManager.CardEdge.INFO)
                    ) {
                    score += -2;
                    if (updateColors) SetEdgeColorPenalty(i);
                }
            }
        }
        return score;
    }

    public void SetWinStage(int stage)
    {
        activeCard = null;
        foreach(var card in cards)
        {
            card.SetClickable(
                card.source == CardGrid.CardSource.PLAYER && stage == 1
                || card.source == CardGrid.CardSource.OTHER && stage == 2
            );
        }
        winStage = stage;
    }
}
