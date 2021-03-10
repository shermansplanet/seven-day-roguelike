using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    CardManager.CardData cardData;
    public bool IsOnCooldown { get; private set; } = false;
    //displayed same as cooldown card
    public bool ChildBeingDragged { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(CardManager.CardId cardId) {
        cardData = CardManager.GetDataById(cardId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName() {
        return this.cardData.name;
    }

    public Color GetColor() {
        if (IsOnCooldown || ChildBeingDragged) return CardManager.GetColorByFamily(CardManager.CardFamily.COOLDOWN);
        return CardManager.GetColorByFamily(this.cardData.family);
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
