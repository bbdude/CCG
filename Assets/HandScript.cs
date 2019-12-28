using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{

    public int handLimit = 5;
    public List<CardScript> cards = new List<CardScript>();
    public List<float> cardPos = new List<float>();
    float cardSep = 1.6f;
    int cardsInHand = 0;
    void Start()
    {
        if (this.transform.position.y < 0)
        {
            for (int i = 0; i < 7; i++)
            {
                cardPos.Add(-4 + (cardSep * i));
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                cardPos.Add(-2 + (cardSep * i));
            }
        }
    }

    
    void Update()
    {
        
    }

    public bool AddCard(CardScript newCard)
    {
        if (cards.Count <= 4)
        {
            //Debug.Log("Hand added card");
            cardsInHand++;
            newCard.transform.localPosition = new Vector3(cardPos[cardsInHand - 1], this.transform.localPosition.y, newCard.transform.localPosition.z);
            newCard.transform.gameObject.SetActive(true);
            newCard.transform.parent = this.transform;
            cards.Add(newCard);
            return true;
        }
        else
            return false;
    }
    public void RemoveCard(int pos)
    {
        cardsInHand--;
        cards.RemoveAt(pos);
        for (int i = pos; i < cardsInHand + 1; i++)
        {
            cards[i].transform.localPosition = new Vector3(cardPos[i], cards[i].transform.localPosition.y, cards[i].transform.localPosition.z);
        }
    }
}
