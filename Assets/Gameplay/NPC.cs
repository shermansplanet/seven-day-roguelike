using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public Inventory inventory;

    public NPC()
    {
        inventory = new Inventory();
        inventory.PopulateInventory();
    }
}
