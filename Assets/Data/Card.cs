using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    CardManager.CardData cardData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(CardManager.CardId cardId) {
        cardData = CardManager.GetDataById(cardId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
