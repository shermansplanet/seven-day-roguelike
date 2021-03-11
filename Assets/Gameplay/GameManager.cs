using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public ConversationGrid grid;
    public static bool playerTurn = true;

    private NPC npc;

    // Start is called before the first frame update
    void Start()
    {
        Inventory inventory = new Inventory();
        inventory.PopulateInventory();
        inventoryUI.Init(inventory);
        npc = new NPC();
    }

    public void OnConfirm()
    {
        StartCoroutine(ConfirmLoop());
    }

    public IEnumerator ConfirmLoop()
    {
        grid.ConfirmMove();
        playerTurn = false;
        yield return new WaitForSeconds(1);
        npc.TakeTurn(grid);
        playerTurn = true;
        inventoryUI.DrawHand();
    }

}
