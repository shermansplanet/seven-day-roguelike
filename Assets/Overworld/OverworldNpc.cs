using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldNpc : MonoBehaviour
{
    public Transform player;
    public Snowman snowman;
    public CharacterManager.Name characterName;

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < 1)
        {
            GameManager.npcIndex = (int)characterName;
            SceneManager.LoadScene(1);
        }
        snowman.SetSortingOrder(transform.position.y > player.position.y ? 1 : 3);
    }
}
