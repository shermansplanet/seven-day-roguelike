using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{

    public Transform player;
    public OverworldManager manager;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, transform.position) < 1)
        {
            manager.NextLevel();
        }
    }
}
