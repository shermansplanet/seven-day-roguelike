using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public ConversationGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        Inventory inventory = new Inventory();
        inventory.PopulateInventory();
        inventoryUI.Init(inventory);
    }

    public void OnConfirm()
    {
        grid.ConfirmMove();
        inventoryUI.DrawHand();
    }

}
