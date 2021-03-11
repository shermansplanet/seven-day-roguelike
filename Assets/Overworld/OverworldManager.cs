using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public void StartConversation()
    {
        SceneManager.LoadScene(1);
    }
}
