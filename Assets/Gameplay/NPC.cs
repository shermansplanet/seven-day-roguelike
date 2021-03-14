using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    private const int BlockedSpotCount = 3;

    public Inventory inventory;
    public ConversationGrid.GameState gameState;
    public CharacterManager.Name characterName;
    public HashSet<Vector2> blockedSpots = new HashSet<Vector2>();

    private struct Move
    {
        public Card card;
        public Vector2 spot;
        public int rotation;
    }

    private readonly Vector2[] directions = new[]
    {
        new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)

    };

    public NPC(CharacterManager.Name name = CharacterManager.Name.NONE)
    {
        characterName = name;
        inventory = new Inventory();
        if (name == CharacterManager.Name.NONE) inventory.PopulateRandomInventory();
        else inventory.PopulatePresetInventory(CharacterManager.GetCharacterCardPreset(name));
        gameState = new ConversationGrid.GameState
        {
            cards = new List<CardGrid>()
        };

        GetBlockedSpots();
    }

    private void GetBlockedSpots()
    {
        blockedSpots.Clear();
        while(blockedSpots.Count < BlockedSpotCount)
        {
            blockedSpots.Add(new Vector2(
                UnityEngine.Random.Range(0, ConversationGrid.BoardWidth),
                UnityEngine.Random.Range(0, ConversationGrid.BoardHeight)
            ));
        }

        Queue<Vector2> Q = new Queue<Vector2>(new Vector2[] { Vector2.zero });
        HashSet<Vector2> searched = new HashSet<Vector2>();
        while(Q.Count > 0)
        {
            Vector2 square = Q.Dequeue();
            searched.Add(square);
            foreach(var dir in directions)
            {
                Vector2 v = square + dir;
                if (searched.Contains(v) || blockedSpots.Contains(v)) continue;
                if (v.x < 0 || v.y < 0 || v.x >= ConversationGrid.BoardWidth || v.y >= ConversationGrid.BoardHeight) continue;
                Q.Enqueue(v);
            }
        }
        if(searched.Count < ConversationGrid.BoardHeight * ConversationGrid.BoardWidth - BlockedSpotCount)
        {
            GetBlockedSpots();
        }
    }

    public IEnumerator TakeTurn(ConversationGrid grid)
    {
        inventory.UpdatePlayableDeck();
        Card[] cards = inventory.GetHand();

        CardGrid tempCard = UnityEngine.Object.Instantiate(grid.cardGridPrefab);
        grid.activeCard = tempCard;

        Dictionary<int, List<Move>> possibleMoves = new Dictionary<int, List<Move>>();
        int maxScore = int.MinValue;
        foreach(Vector2 spotCandidate in grid.availableSpots)
        {
            foreach (Card cardCandidate in cards)
            {
                for (int i=0; i<4; i++)
                {
                    tempCard.card = cardCandidate;
                    tempCard.x = Mathf.RoundToInt(spotCandidate.x);
                    tempCard.y = Mathf.RoundToInt(spotCandidate.y);
                    tempCard.rotation = i;
                    int score = grid.GetCardScore(false);

                    if (score < maxScore) continue;

                    maxScore = Mathf.Max(maxScore, score);

                    if (!possibleMoves.ContainsKey(score))
                    {
                        possibleMoves.Add(score, new List<Move>());
                    }
                    possibleMoves[score].Add(
                        new Move { card = cardCandidate, spot = spotCandidate, rotation = i }
                    );
                }
            }
            yield return null;
        }
        UnityEngine.Object.Destroy(tempCard.gameObject);
        grid.activeCard = null;

        List<Move> moves = possibleMoves[maxScore];
        Move move = moves[UnityEngine.Random.Range(0, moves.Count)];

        grid.SpawnCard(move.card, null);

        grid.activeCard.x = Mathf.RoundToInt(move.spot.x);
        grid.activeCard.y = Mathf.RoundToInt(move.spot.y);

        grid.activeCard.transform.Rotate(0, 0, -90 * move.rotation);
        grid.activeCard.rotation = move.rotation;

        grid.OnCardRelease(grid.activeCard);
    }
}
