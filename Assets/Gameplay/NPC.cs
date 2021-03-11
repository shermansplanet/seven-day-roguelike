using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public Inventory inventory;

    public NPC()
    {
        inventory = new Inventory();
        inventory.PopulateInventory();
    }

    public void TakeTurn(ConversationGrid grid)
    {
        inventory.UpdatePlayableDeck();
        Card[] cards = inventory.GetHand();
        Card card = cards[UnityEngine.Random.Range(0, cards.Length)];
        grid.SpawnCard(card, null);

        Vector2 spot = grid.availableSpots[UnityEngine.Random.Range(0, grid.availableSpots.Count)];
        grid.activeCard.x = Mathf.RoundToInt(spot.x);
        grid.activeCard.y = Mathf.RoundToInt(spot.y);
        grid.OnCardRelease(grid.activeCard);
    }
}
