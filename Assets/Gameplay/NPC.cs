using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public Inventory inventory;
    public ConversationGrid.GameState gameState;

    private struct Move
    {
        public Card card;
        public Vector2 spot;
        public int rotation;
    }

    public NPC()
    {
        inventory = new Inventory();
        inventory.PopulateInventory();
        gameState = new ConversationGrid.GameState
        {
            cards = new List<CardGrid>()
        };
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
