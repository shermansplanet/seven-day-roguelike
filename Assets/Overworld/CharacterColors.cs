using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColors : MonoBehaviour
{
    public Color[] lavenderColors;
    public Color[] strawbubColors;
    public Color[] noneColors;

    public Color[] GetColors(CharacterManager.Name name)
    {
        return
            name == CharacterManager.Name.LAVENDER ? lavenderColors :
            name == CharacterManager.Name.STRAWBUB ? strawbubColors :
            noneColors;
    }
}
