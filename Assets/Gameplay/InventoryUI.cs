using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public CardInstance cardInstancePrefab;
    public TextMeshPro cooldownCount;
    public TextMeshPro deckCount;

    [HideInInspector]
    public ConversationGrid grid;
    public Inventory inventory;

    private List<CardInstance> inventoryCards = new List<CardInstance>();

    public void Init(Inventory inventory)
    {
        this.inventory = inventory;
        DrawHand();
    }

    public void DrawHand() {
        Card[] hand = inventory.GetHand();
        for (int i = 0; i < hand.Length; i++) {
            CardInstance cardInstance = Instantiate(cardInstancePrefab);
            cardInstance.transform.SetParent(transform);
            cardInstance.transform.localPosition = new Vector3((i - (hand.Length - 1) / 2f) * 1.1f, 0, 0);
            cardInstance.Init(grid, hand[i], true);
            inventoryCards.Add(cardInstance);
        }
        cooldownCount.text = inventory.GetCooldownCount().ToString();
        deckCount.text = inventory.GetDeckCount().ToString();
    }
}
