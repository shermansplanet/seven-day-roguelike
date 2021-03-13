using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const int PathSeparation= 3;
    private const int EncounterLength = 5;
    private const float GridSize = 2.5f;
    private const int EncounterCount = 2;

    public SpriteRenderer tilePrefab;
    public SpriteRenderer blockerPrefab;
    public OverworldNpc npcPrefab;
    public CharacterColors colors;
    public Player player;
    public Transform nextLevel;

    private static HashSet<Vector2> levelTiles;
    public static Dictionary<CharacterManager.Name, Vector2> npcPositions;
    public static bool firstLoad = true;
    public static Vector2 playerPos;

    public void GenerateLevel()
    {
        if (firstLoad)
        {
            NewLevel();
            firstLoad = false;
        }
        ConstructLevel();
    }

    private void NewLevel() {
        string[] charNames = System.Enum.GetNames(typeof(CharacterManager.Name));
        int pathCount = Random.Range(1, 1 + Mathf.FloorToInt(charNames.Length * 1f / EncounterCount));
        List<int>[,] encounters = new List<int>[pathCount, EncounterCount];

        List<int> sourceList = new List<int>();
        for (int i = 0; i < charNames.Length; i++)
        {
            sourceList.Add(i);
        }

        int[] pathWidths = new int[pathCount];

        for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
        {
            for (int encounterIndex = 0; encounterIndex < EncounterCount; encounterIndex++)
            {
                int i = Random.Range(0, sourceList.Count);
                encounters[pathIndex, encounterIndex] = new List<int> { sourceList[i] };
                sourceList.RemoveAt(i);
            }
            pathWidths[pathIndex] = 1;
        }

        while (sourceList.Count > 0)
        {
            int pathIndex = Random.Range(0, pathCount);
            int encounterIndex = Random.Range(0, EncounterCount);
            encounters[pathIndex, encounterIndex].Add(sourceList[0]);
            sourceList.RemoveAt(0);
            pathWidths[pathIndex] = Mathf.Max(pathWidths[pathIndex], encounters[pathIndex, encounterIndex].Count);
        }

        levelTiles = new HashSet<Vector2>();
        npcPositions = new Dictionary<CharacterManager.Name, Vector2>();
        int pathPosition = 0;

        for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
        {
            for (int encounterIndex = 0; encounterIndex < EncounterCount; encounterIndex++)
            {
                List<int> npcs = encounters[pathIndex, encounterIndex];
                for (int i = 0; i < npcs.Count; i++)
                {
                    Vector2 pos = Vector2.zero;
                    int baseX = EncounterLength * encounterIndex;
                    int baseY = (pathPosition + i) * PathSeparation;
                    for (int x = 0; x < EncounterLength; x++)
                    {
                        pos = new Vector2(baseX + x, baseY) * GridSize;
                        levelTiles.Add(pos);
                    }

                    if (i > 0)
                    {
                        for (int y = 0; y <= PathSeparation; y++)
                        {
                            pos = new Vector2(baseX, baseY - y) * GridSize;
                            if (!levelTiles.Contains(pos)) levelTiles.Add(pos);
                            pos = new Vector2(baseX + EncounterLength, baseY - y) * GridSize;
                            if (!levelTiles.Contains(pos)) levelTiles.Add(pos);
                        }
                    }

                    Vector2 npcPos = new Vector2(baseX + Random.Range(1, EncounterLength), baseY) * GridSize;
                    CharacterManager.Name n = (CharacterManager.Name)System.Enum.Parse(typeof(CharacterManager.Name), charNames[npcs[i]]);
                    npcPositions.Add(n, npcPos);
                }
            }
            if (pathIndex < pathCount - 1) pathPosition += pathWidths[pathIndex];
        }

        playerPos = new Vector2(0, pathPosition * PathSeparation * GridSize);

        for (int y = 0; y <= pathPosition * PathSeparation; y++)
        {
            Vector2 pos = new Vector2(0, y) * GridSize;
            if (!levelTiles.Contains(pos)) levelTiles.Add(pos);
            pos = new Vector2(EncounterLength * EncounterCount, y) * GridSize;
            if (!levelTiles.Contains(pos)) levelTiles.Add(pos);
        }
    }

    private void ConstructLevel() {
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        HashSet<Vector2> blockerTiles = new HashSet<Vector2>();

        foreach (Vector2 pos in levelTiles)
        {
            var tile = Instantiate(tilePrefab, pos, Quaternion.identity);
            tile.transform.localScale = Vector3.one * GridSize;
            foreach(var dir in directions)
            {
                Vector2 blockerPos = dir * GridSize + pos;
                if (blockerTiles.Contains(blockerPos) || levelTiles.Contains(blockerPos)) continue;
                blockerTiles.Add(blockerPos);
                var blocker = Instantiate(blockerPrefab, blockerPos, Quaternion.identity);
                blocker.transform.localScale = Vector3.one * GridSize;
            }
        }

        foreach(var npcPos in npcPositions)
        {
            OverworldNpc npc = Instantiate(npcPrefab, npcPos.Value, Quaternion.identity);
            npc.characterName = npcPos.Key;
            npc.player = player.transform;
            npc.snowman.SetColors(colors.GetColors(npcPos.Key));
        }

        player.transform.position = playerPos;
        nextLevel.transform.position = new Vector2(EncounterLength * EncounterCount * GridSize, 0);
    }
}