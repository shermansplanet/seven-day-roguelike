using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Inventory : MonoBehaviour
{
    const int INVENTORY_SIZE = 12;
    public Card[] inventory;

    // Start is called before the first frame update
    private void Start()
    {
        inventory = new Card[INVENTORY_SIZE];
        for (int i = 0; i < INVENTORY_SIZE; i++) {
            Card newCard = new Card();
            newCard.Init(EXAMPLE);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReplaceCard(int index, Card newCard) {
        inventory[index] = newCard;
    }
}
