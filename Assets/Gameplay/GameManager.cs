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

    private NPC currentNPC;

    private static NPC[] npcList;
    private static Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null)
        {
            inventory = new Inventory();
            inventory.PopulateInventory();
            npcList = new NPC[3];
            for (int i = 0; i < npcList.Length; i++) npcList[i] = new NPC();
        }
        inventoryUI.Init(inventory);
        currentNPC = npcList[Random.Range(0,npcList.Length)];
        grid.SetGameState(currentNPC.gameState);
        score = 0;
    }

    public void OnConfirm()
    {
        StartCoroutine(ConfirmLoop());
    }

    public void OnCardPlaced()
    {
        grid.ConfirmMove();
        bool fullBoard = grid.availableSpots.Count == 0;
        if (fullBoard)
        {
            grid.cards.Clear();
        }
        if (score >= ScoreToWin || fullBoard)
        {
            currentNPC.gameState = grid.GetGameState();
            SceneManager.LoadScene(0);
            StopAllCoroutines();
        }
    }

    public IEnumerator ConfirmLoop()
    {
        OnCardPlaced();
        playerTurn = false;
        yield return new WaitForSeconds(1);
        currentNPC.TakeTurn(grid);
        OnCardPlaced();
        playerTurn = true;
        inventoryUI.DrawHand();
    }

}
