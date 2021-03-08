using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public TextMeshPro cardName;

    private ConversationGrid grid;
    private Card card;

    // How many pixels tall a grid square is
    private float gridSquarePixels;
    private Vector3 originalMousePosition;
    private Vector3 originalCardPosition;

    [HideInInspector]
    public int x, y;

    public void Init(ConversationGrid grid, Card card)
    {
        this.grid = grid;
        gridSquarePixels = Screen.height / grid.GridSquaresVertical;
        transform.SetParent(grid.transform);
        transform.localScale = Vector3.one;
        this.card = card;
        cardName.text = card.GetName();
        
    }

    private void OnMouseDown()
    {
        originalMousePosition = Input.mousePosition;
        originalCardPosition = transform.localPosition;
    }

    private void OnMouseDrag()
    {
        x = Mathf.RoundToInt((Input.mousePosition.x - Screen.width / 2) / gridSquarePixels);
        y = Mathf.RoundToInt((Input.mousePosition.y - Screen.height / 2) / gridSquarePixels);

        transform.localPosition = originalCardPosition +
            (Input.mousePosition - originalMousePosition) / gridSquarePixels;

        grid.OnCardDrag(this);
    }

    private void OnMouseUp()
    {
        grid.OnCardRelease(this);
    }

    public void CancelMove()
    {
        transform.position = originalCardPosition;
    }
}
