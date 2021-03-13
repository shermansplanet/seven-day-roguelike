using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public OverworldNpc npcPrefab;
    public CharacterColors colors;
    public LevelGenerator levelGen;
    public Player player;

    public void Start()
    {
        string[] charName = Enum.GetNames(typeof(CharacterManager.Name));
        levelGen.GenerateLevel();
        for (int i = 0; i < charName.Length; i++)
        {
            OverworldNpc npc = Instantiate(npcPrefab, new Vector3(i * 5, 5, 0), Quaternion.identity);
            CharacterManager.Name n = (CharacterManager.Name)Enum.Parse(typeof(CharacterManager.Name), charName[i]);
            npc.characterName = n;
            npc.player = player.transform;
            npc.snowman.SetColors(colors.GetColors(n));
        }
    }
}
