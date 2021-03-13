using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardGrid : MonoBehaviour {
    private ConversationGrid grid;
    public CardInstance cardInstance;
    public Card card;
    public bool firstDrag;
    private CardInventory parent;

    // How many pixels tall a grid square is
    private float gridSquarePixels;
    private Vector3 originalMousePosition;
    private Vector3 originalCardPosition;
    private Vector3 gridPixelPosition;

    [HideInInspector]
    public int x, y, rotation;

    [HideInInspector]
    public bool beingDragged;

    [HideInInspector]
    public bool confirmedOnBoard = false;

    public void Init(ConversationGrid grid, Card card, CardInventory parent) {
        cardInstance.Init(card, parent == null);
        this.parent = parent;
        this.grid = grid;
        gridPixelPosition = Camera.main.WorldToScreenPoint(grid.transform.position);
        this.card = card;
        gridSquarePixels = Screen.height / grid.GridSquaresVertical;
        transform.SetParent(grid.transform);
        transform.localScale = Vector3.one * 0.97f;
        transform.localPosition = new Vector3(
            (Input.mousePosition.x - gridPixelPosition.x) / gridSquarePixels,
            (Input.mousePosition.y - gridPixelPosition.y) / gridSquarePixels, 0
        );
        firstDrag = true;
        OnMouseDown();
    }

    public void SetCenterText(string text) {
        cardInstance.SetCenterText(text);
    }

    public void SetEdgeColor(int index, Color color) {
        cardInstance.SetEdgeColor(index, color);
    }

    public bool Draggable() {
        return !confirmedOnBoard;
    }

    private void OnMouseDown() {
        if (!Draggable()) return;
        ResetCenterText();
        beingDragged = true;
        originalMousePosition = Input.mousePosition;
        originalCardPosition = transform.localPosition;
    }

    public void OnMouseDrag() {
        if (!Draggable()) return;
        x = Mathf.RoundToInt((Input.mousePosition.x - gridPixelPosition.x) / gridSquarePixels);
        y = Mathf.RoundToInt((Input.mousePosition.y - gridPixelPosition.y) / gridSquarePixels);

        transform.localPosition = originalCardPosition +
            (Input.mousePosition - originalMousePosition) / gridSquarePixels;

        grid.OnCardDrag(this);
    }

    public void ResetCenterText() {
        cardInstance.ResetCenterText();
    }

    public void OnMouseUp() {
        if (!Draggable()) return;
        grid.OnCardRelease(this);
        beingDragged = false;
        firstDrag = false;
    }

    public void Confirm() {
        confirmedOnBoard = true;
        card.Confirm();
    }

    public void CancelMoveSnapBack() {
        transform.localPosition = originalCardPosition;
        x = Mathf.RoundToInt(originalCardPosition.x);
        y = Mathf.RoundToInt(originalCardPosition.y);
    }

    public void CancelMoveFromInventory() {
        if (parent) parent.ChildMoveCancelled();
        Destroy(gameObject);
    }
}
