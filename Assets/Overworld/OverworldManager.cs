using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public LevelGenerator levelGen;
    public static int level = 0;

    public void Awake()
    {
        levelGen.GenerateLevel();
    }

    public void NextLevel()
    {
        LevelGenerator.firstLoad = true;
        level++;
        SceneManager.LoadScene(0);
    }
}
