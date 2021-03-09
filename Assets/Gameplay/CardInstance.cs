using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public TextMeshPro cardName;
    public SpriteRenderer cardRenderer;

    private ConversationGrid grid;
    private Card card;
    private bool isInInventory;
    private bool firstDrag;

    // How many pixels tall a grid square is
    private float gridSquarePixels;
    private Vector3 originalMousePosition;
    private Vector3 originalCardPosition;

    [HideInInspector]
    public int x, y;

    [HideInInspector]
    public bool beingDragged, draggable;

    [HideInInspector]
    public CardInstance draggedCard;

    public void Init(ConversationGrid grid, Card card, bool inInventory)
    {
        this.grid = grid;
        draggable = true;
        if (!inInventory)
        {
            gridSquarePixels = Screen.height / grid.GridSquaresVertical;
            transform.SetParent(grid.transform);
            transform.localScale = Vector3.one;
            transform.localPosition = new Vector3(
                (Input.mousePosition.x - Screen.width / 2) / gridSquarePixels,
                (Input.mousePosition.y - Screen.height / 2) / gridSquarePixels, 0
            );
            firstDrag = true;
            OnMouseDown();
        }
        isInInventory = inInventory;
        this.card = card;
        cardName.text = card.GetName();
        cardRenderer.color = card.GetColor();
    }

    private void OnMouseDown()
    {
        if (!draggable)
        {
            return;
        }
        if (isInInventory)
        {
            grid.SpawnCard(card, this);
            card.OnCooldown();
            cardRenderer.color = card.GetColor();
            return;
        }
        beingDragged = true;
        originalMousePosition = Input.mousePosition;
        originalCardPosition = transform.localPosition;
    }

    public void OnMouseDrag()
    {
        if (!draggable)
        {
            return;
        }
        if (isInInventory)
        {
            if(draggedCard != null)
            {
                draggedCard.OnMouseDrag();
            }
            return;
        }
        x = Mathf.RoundToInt((Input.mousePosition.x - Screen.width / 2) / gridSquarePixels);
        y = Mathf.RoundToInt((Input.mousePosition.y - Screen.height / 2) / gridSquarePixels);

        transform.localPosition = originalCardPosition +
            (Input.mousePosition - originalMousePosition) / gridSquarePixels;

        grid.OnCardDrag(this);
    }

    public void OnMouseUp()
    {
        if (!draggable)
        {
            return;
        }
        if (isInInventory)
        {
            if (draggedCard != null)
            {
                draggedCard.OnMouseUp();
                draggedCard = null;
            }
            return;
        }
        grid.OnCardRelease(this);
        beingDragged = false;
        firstDrag = false;
    }

    public void CancelMove()
    {
        if (firstDrag)
        {
            Destroy(gameObject);
            return;
        }
        transform.localPosition = originalCardPosition;
        x = Mathf.RoundToInt(originalCardPosition.x);
        y = Mathf.RoundToInt(originalCardPosition.y);
    }
}
