using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int ScoreToWin = 10;

    public InventoryUI inventoryUI;
    public ConversationGrid grid;
    public static bool playerTurn = true;
    public static int score;

    private NPC npc;

    private static Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null)
        {
            inventory = new Inventory();
            inventory.PopulateInventory();
        }
        inventoryUI.Init(inventory);
        npc = new NPC();
        score = 0;
    }

    public void OnConfirm()
    {
        StartCoroutine(ConfirmLoop());
    }

    public void OnCardPlaced()
    {
        grid.ConfirmMove();
        if(score >= ScoreToWin)
        {
            SceneManager.LoadScene(0);
            StopAllCoroutines();
        }
    }

    public IEnumerator ConfirmLoop()
    {
        OnCardPlaced();
        playerTurn = false;
        yield return new WaitForSeconds(1);
        npc.TakeTurn(grid);
        OnCardPlaced();
        playerTurn = true;
        inventoryUI.DrawHand();
    }

}
