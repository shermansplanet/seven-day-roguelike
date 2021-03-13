using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldNpc : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < 1)
        {
            SceneManager.LoadScene(1);
        }
    }
}
