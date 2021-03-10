﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Inventory
{
    const int INVENTORY_SIZE = 12;
    const int HAND_SIZE = 4;
    public Card[] inventory;
    //inventory but without cards on cooldown
    public List<Card> playableDeck;

    public void PopulateInventory()
    {
        inventory = new Card[INVENTORY_SIZE];
        for (int i = 0; i < INVENTORY_SIZE; i++) {
            Card newCard = new Card();
            newCard.Init(CardManager.GetRandomCardId());
            inventory[i] = newCard;
        }
        playableDeck = new List<Card>(inventory);
    }

    public Card[] GetHand() {
        if (playableDeck.Count < 4) return playableDeck.ToArray();
        return playableDeck.GetRange(0, 4).ToArray();
    }

    public int GetCooldownCount() {
        return INVENTORY_SIZE - playableDeck.Count;
    }

    public int GetDeckCount() {
        return Mathf.Max(playableDeck.Count - HAND_SIZE, 0);
    }

    private void ReplaceCard(int index, Card newCard) {
        inventory[index] = newCard;
    }
}
