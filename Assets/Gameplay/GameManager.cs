using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int ScoreToWin = 30;

    public InventoryUI inventoryUI;
    public ConversationGrid grid;
    public static bool playerTurn = true;
    public static int score;
    public SpriteRenderer character;
    public static int npcIndex;

    private NPC currentNPC;

    private static NPC[] npcList;
    private static Inventory inventory;

    public static void RefreshAllCooldowns() {
        inventory.RefreshAllCooldowns();
        foreach (NPC npc in npcList) npc.inventory.RefreshAllCooldowns();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null)
        {
            inventory = new Inventory();
            inventory.PopulateRandomInventory();
            npcList = new NPC[CharacterManager.validNpcNames.Count];
            for (int i = 0; i < npcList.Length; i++) npcList[i] = new NPC(CharacterManager.validNpcNames[i]);
        }
        inventoryUI.Init(inventory);
        currentNPC = npcList[npcIndex];
        grid.SetGameState(currentNPC.gameState);
        character.sprite = CharacterManager.GetSprite(currentNPC.characterName);
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
        yield return currentNPC.TakeTurn(grid);
        OnCardPlaced();
        playerTurn = true;
        inventoryUI.DrawHand();
    }

}
