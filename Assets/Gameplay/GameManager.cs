using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text goalText;
    public InventoryUI inventoryUI;
    public ConversationGrid grid;
    public static bool playerTurn = true;
    public static int score;
    public SpriteRenderer character;
    public static int npcIndex;

    private NPC currentNPC;

    private int goal = 30;
    private static NPC[] npcList;
    private static Inventory inventory;

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
        grid.SetGameState(currentNPC);
        character.sprite = CharacterManager.GetSprite(currentNPC.characterName);
        score = 0;
        goalText.text = "Goal: " + goal.ToString();
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
        if (score >= goal || fullBoard)
        {
            StopAllCoroutines();
            StartCoroutine(WinLoop(!fullBoard));
        }
    }

    public IEnumerator WinLoop(bool canTrade)
    {
        currentNPC.gameState = grid.GetGameState();

        if (canTrade)
        {
            inventoryUI.SetInstructionText("Choose a card to remove from your deck.");
            grid.SetWinStage(1);
            yield return new WaitUntil(() => grid.activeCard != null);
            var oldCard = grid.activeCard.card;

            inventoryUI.SetInstructionText("Choose a card to add to your deck.");
            grid.SetWinStage(2);
            yield return new WaitUntil(() => grid.activeCard != null);
            inventory.ReplaceCard(oldCard, grid.activeCard.card);
        }

        SceneManager.LoadScene(0);
        StopAllCoroutines();
    }

    public IEnumerator ConfirmLoop()
    {
        OnCardPlaced();
        playerTurn = false;
        yield return new WaitForSeconds(1);
        yield return currentNPC.TakeTurn(grid);
        OnCardPlaced();
        if (grid.winStage > 0) yield break;
        playerTurn = true;
        inventoryUI.DrawHand();
    }

    public void IncreaseGoal() {
        goal += 5;
        goalText.text = "Goal: " + goal.ToString();
    }

    public static void RefreshAllCooldowns() {
        inventory.RefreshAllCooldowns();
        foreach (NPC npc in npcList) npc.inventory.RefreshAllCooldowns();
    }
}
