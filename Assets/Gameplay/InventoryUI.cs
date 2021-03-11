using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public CardInventory cardInventoryPrefab;
    public TextMeshPro cooldownCount;
    public TextMeshPro deckCount;

    [HideInInspector]
    public ConversationGrid grid;
    public Inventory inventory;

    private List<CardInventory> inventoryHandCards = new List<CardInventory>();

    public void Init(Inventory inventory)
    {
        this.inventory = inventory;
        DrawHand();
    }

    public void DrawHand() {
        foreach (CardInventory inventoryHandCard in inventoryHandCards) Destroy(inventoryHandCard.gameObject);
        inventoryHandCards = new List<CardInventory>();
        inventory.UpdatePlayableDeck();
        Card[] hand = inventory.GetHand();
        for (int i = 0; i < hand.Length; i++) {
            CardInventory cardInventory = Instantiate(cardInventoryPrefab);
            cardInventory.transform.SetParent(transform);
            cardInventory.transform.localPosition = new Vector3((i - (hand.Length - 1) / 2f) * 1.1f, 0, 0);
            cardInventory.Init(grid, hand[i]);
            inventoryHandCards.Add(cardInventory);
        }
        cooldownCount.text = inventory.GetCooldownCount().ToString();
        deckCount.text = inventory.GetDeckCount().ToString();
    }
}
