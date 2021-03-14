using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColors : MonoBehaviour
{
    public Color[] strawbubColors;
    public Color[] lavenderColors;
    public Color[] bluebubColors;
    public Color[] lorenColors;
    public Color[] guardColors;
    public Color[] noneColors;

    public Color[] GetColors(CharacterManager.Name name)
    {
        return
            name == CharacterManager.Name.STRAWBUB ? strawbubColors :
            name == CharacterManager.Name.LAVENDER ? lavenderColors :
            name == CharacterManager.Name.BLUEBUB ? bluebubColors :
            name == CharacterManager.Name.LOREN ? lorenColors :
            name == CharacterManager.Name.GUARD ? guardColors :
            noneColors;
    }
}
