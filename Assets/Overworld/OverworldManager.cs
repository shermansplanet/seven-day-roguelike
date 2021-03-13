using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public LevelGenerator levelGen;

    public void Awake()
    {
        levelGen.GenerateLevel();
    }
}
