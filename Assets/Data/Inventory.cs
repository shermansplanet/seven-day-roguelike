using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Inventory
{
    const int INVENTORY_SIZE = 12;
    public Card[] inventory;

    public void PopulateInventory()
    {
        inventory = new Card[INVENTORY_SIZE];
        for (int i = 0; i < INVENTORY_SIZE; i++) {
            Card newCard = new Card();
            newCard.Init(CardManager.CardId.EXAMPLE);
        }
    }

    private void ReplaceCard(int index, Card newCard) {
        inventory[index] = newCard;
    }
}
