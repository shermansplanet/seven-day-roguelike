using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInventory : MonoBehaviour {
    private ConversationGrid grid;
    public CardInstance cardInstance;
    public Card card;

    // How many pixels tall a grid square is
    private float gridSquarePixels;
    private Vector3 originalMousePosition;
    private Vector3 originalCardPosition;
    private Vector3 gridPixelPosition;

    [HideInInspector]
    public int x, y, rotation;

    [HideInInspector]
    public CardGrid draggedCard;

    public void Init(ConversationGrid grid, Card card) {
        this.grid = grid;
        gridPixelPosition = Camera.main.WorldToScreenPoint(grid.transform.position);
        this.card = card;
        cardInstance.Init(card);
    }

    public bool Draggable() {
        return !card.IsOnCooldown;
    }

    private void OnMouseDown() {
        if (!Draggable()) return;
        if (card.ChildBeingDragged) return;
        grid.SpawnCard(card, this);
        card.Select();
        cardInstance.UpdateColor();
        return;
    }

    public void OnMouseDrag() {
        if (!Draggable()) return;
        if (draggedCard != null) {
            draggedCard.OnMouseDrag();
        }
    }

    public void OnMouseUp() {
        if (!Draggable()) return;
        if (draggedCard != null) {
            draggedCard.OnMouseUp();
            draggedCard = null;
        }
    }

    public void Confirm() {
        card.Confirm();
    }

    public void ChildMoveCancelled() {
        card.OffCooldown();
        cardInstance.UpdateColor();
    }
}
