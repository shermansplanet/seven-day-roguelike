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

    public void Init(Card card)
    {
        this.card = card;
        cardName.text = card.GetName();
        baseScore.text = card.GetScore().ToString();
        cardRenderer.color = card.GetColor();
        for (int i = 0; i < 4; i++) {
            edges[i].sprite = card.GetEdgeSprite(i);
        }
    }

    public void SetCenterText(string text)
    {
        baseScore.text = text;
    }
    
    public void ResetCenterText()
    {
        baseScore.text = card.GetScore().ToString();
    }

    public void UpdateColor() 
    {
        cardRenderer.color = card.GetColor();
    }
}
