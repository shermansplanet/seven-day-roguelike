using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public CardInstance cardInstancePrefab;

    private List<CardInstance> inventoryCards = new List<CardInstance>();

    public void SpawnFromInventory(Inventory inventoryObj)
    {
        Card[] inventory = inventoryObj.inventory;
        for(int i=0; i<inventory.Length; i++)
        {
            CardInstance card = Instantiate(cardInstancePrefab);
            card.transform.SetParent(transform);
            card.transform.localPosition = new Vector3(i - (inventory.Length-1) / 2f, 0, 0);
            inventoryCards.Add(card);
        }
    }
}
