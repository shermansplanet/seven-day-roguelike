using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public CardManager.CardData cardData;
    public bool IsOnCooldown { get; private set; } = false;
    //displayed same as cooldown card
    public bool ChildBeingDragged { get; private set; } = false;

    public void Init(CardManager.CardId cardId) {
        cardData = CardManager.GetDataById(cardId);
    }

    public string GetName() {
        return cardData.name;
    }

    public int GetScore() {
        return cardData.baseScore;
    }

    public Color GetRawColor()
    {
        return CardManager.GetColorByFamily(cardData.family);
    }

    public Color GetColor() {
        if (IsOnCooldown || ChildBeingDragged) return CardManager.GetColorByFamily(CardManager.CardFamily.COOLDOWN);
        return GetRawColor();
    }

    public Sprite GetEdgeSprite(int index) {
        if (index >= cardData.edges.Length) return null;
        CardManager.CardEdge edge = cardData.edges[index];
        return CardManager.GetSpriteByEdge(edge);
    }

    public void Select() {
        ChildBeingDragged = true;
    }

    public void Confirm() {
        ChildBeingDragged = false;
        IsOnCooldown = true;
    }

    public void OffCooldown() {
        ChildBeingDragged = false;
        IsOnCooldown = false;
    }
}
