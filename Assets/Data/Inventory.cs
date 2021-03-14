using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    const int INVENTORY_SIZE = 12;
    const int HAND_SIZE = 4;
    public Card[] inventory;
    //inventory but without cards on cooldown
    public List<Card> playableDeck;

    public void PopulateRandomInventory() {
        inventory = new Card[INVENTORY_SIZE];
        for (int i = 0; i < INVENTORY_SIZE; i++) {
            Card newCard = new Card();
            newCard.Init(CardManager.GetRandomCardId());
            inventory[i] = newCard;
        }
        playableDeck = new List<Card>(inventory);
    }

    public void PopulatePresetInventory(List<CardManager.CardId> preset) {
        inventory = new Card[INVENTORY_SIZE];
        for (int i = 0; i < INVENTORY_SIZE; i++) {
            Card newCard = new Card();
            newCard.Init(preset[UnityEngine.Random.Range(0, preset.Count)]);
            inventory[i] = newCard;
        }
        playableDeck = new List<Card>(inventory);
    }

    public void RefreshAllCooldowns() {
        Debug.Log(playableDeck.Count);
        foreach (Card card in inventory) card.OffCooldown();
        UpdatePlayableDeck();
        Debug.Log("Refreshing cooldowns");
        Debug.Log(playableDeck.Count);
    }

    public void UpdatePlayableDeck() {
        playableDeck = new List<Card>(Array.FindAll(inventory, card => !card.IsOnCooldown));
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

    public void ReplaceCard(Card oldCard, Card newCard) {
        for(int i=0; i<inventory.Length; i++)
        {
            if(inventory[i] == oldCard)
            {
                inventory[i] = newCard;
                return;
            }
        }
    }
}
