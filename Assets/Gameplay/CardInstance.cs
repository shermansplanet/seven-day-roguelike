using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInstance : MonoBehaviour
{
    Card card;

    public TextMeshPro cardName;
    public TextMeshPro baseScore;
    public SpriteRenderer cardRenderer;
    public SpriteRenderer[] edges;

    public void Init(Card card, bool forceColor = false)
    {
        this.card = card;
        cardName.text = card.GetName();
        baseScore.text = card.GetScore().ToString();

        cardRenderer.color = forceColor ? card.GetRawColor() : card.GetColor();

        for (int i = 0; i < 4; i++) {
            edges[i].sprite = card.GetEdgeSprite(i);
            edges[i].color = Color.black;
        }
    }

    public void SetCenterText(string text)
    {
        baseScore.text = text;
    }

    public void ResetCenterText() {
        baseScore.text = card.GetScore().ToString();
    }

    public void SetEdgeColor(int index, Color color) {
        edges[index].color = color;
    }
    
    public void ResetEdgeColor() {
        for (int i = 0; i < 4; i++) {
            edges[i].color = Color.black;
        }
    }

    public void UpdateColor() 
    {
        cardRenderer.color = card.GetColor();
    }
}
