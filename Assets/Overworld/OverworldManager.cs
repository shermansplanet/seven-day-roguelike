using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public LevelGenerator levelGen;
    public static int level = 0;
    public TextMeshPro levelText;

    public void Awake()
    {
        levelGen.GenerateLevel();
        levelText.text = (level + 1).ToString();
    }

    public void NextLevel()
    {
        LevelGenerator.firstLoad = true;
        level++;
        GameManager.RefreshAllCooldowns();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnRestartClicked()
    {
        Restart();
    }

    public static void Restart()
    {
        LevelGenerator.firstLoad = true;
        GameManager.inventory = null;
        Player.firstSpawn = true;
        level = 0;
        SceneManager.LoadScene(0);
    }
}
