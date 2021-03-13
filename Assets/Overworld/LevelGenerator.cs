using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const int EncountersPerLevel = 2;

    public void GenerateLevel()
    {
        float[] breakpoints = new float[EncountersPerLevel];
        float sum = 0f;
        for(int i=0; i<EncountersPerLevel; i++)
        {
            breakpoints[i] = Random.value;
            sum += breakpoints[i];
        }

        string[] charNames = System.Enum.GetNames(typeof(CharacterManager.Name));

        int lastIndex = 0;
        float floatIndex = 0;
        List<List<int>>[] levelLayout = new List<List<int>>[EncountersPerLevel];
        for (int i = 0; i < EncountersPerLevel; i++)
        {
            floatIndex += breakpoints[i] * (charNames.Length) / sum;
            int index = Mathf.CeilToInt(breakpoints[i] * (charNames.Length) / sum);

            index = Mathf.Max(index, lastIndex + 1);
            index = Mathf.Min(index, charNames.Length - (EncountersPerLevel - 1 - i));

            lastIndex = index;
        }
    }
}