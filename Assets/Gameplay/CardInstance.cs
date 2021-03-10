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
    public bool firstDrag;
    private CardInstance parent;

    // How many pixels tall a grid square is
    private float gridSquarePixels;
    private Vector3 originalMousePosition;
    private Vector3 originalCardPosition;
    private Vector3 gridPixelPosition;

    [HideInInspector]
    public int x, y;

    [HideInInspector]
    public bool beingDragged, draggable;

    [HideInInspector]
    public CardInstance draggedCard;

    public void Init(ConversationGrid grid, Card card, bool inInventory, CardInstance parent = null)
    {
        if (inInventory && parent) Debug.LogError("card in inventory should not have parent!");
        this.parent = parent;
        this.grid = grid;
        draggable = true;
        gridPixelPosition = Camera.main.WorldToScreenPoint(grid.transform.position);
        if (!inInventory)
        {
            gridSquarePixels = Screen.height / grid.GridSquaresVertical;
            transform.SetParent(grid.transform);
            transform.localScale = Vector3.one * 0.95f;
            transform.localPosition = new Vector3(
                (Input.mousePosition.x - gridPixelPosition.x) / gridSquarePixels,
                (Input.mousePosition.y - gridPixelPosition.y) / gridSquarePixels, 0
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
        x = Mathf.RoundToInt((Input.mousePosition.x - gridPixelPosition.x) / gridSquarePixels);
        y = Mathf.RoundToInt((Input.mousePosition.y - gridPixelPosition.y) / gridSquarePixels);

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

    public void CancelMoveSnapBack()
    {
        transform.localPosition = originalCardPosition;
        x = Mathf.RoundToInt(originalCardPosition.x);
        y = Mathf.RoundToInt(originalCardPosition.y);
    }

    public void CancelMoveFromInventory() 
    {
        if (parent) parent.ChildMoveCancelled();
        Destroy(gameObject);
    }

    public void ChildMoveCancelled() 
    {
        card.OffCooldown();
        cardRenderer.color = card.GetColor();
    }
}
