using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    //Prefabs
    public GameObject masterCard;
    public GameObject handPrefab;

    //Card List
    //Temporary mats
    public Material[] randomMaterials;



    public List<CardScript> cards = new List<CardScript>();
    //public HandScript hand;
    int deckLimit = 20;


    // Start is called before the first frame update
    void Start()
    {
        GenerateDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDeck()
    {
        cards.Add(Instantiate(masterCard.GetComponent<CardScript>()));
        cards.Add(Instantiate(masterCard.GetComponent<CardScript>()));
        cards.Add(Instantiate(masterCard.GetComponent<CardScript>()));
        cards.Add(Instantiate(masterCard.GetComponent<CardScript>()));
        cards.Add(Instantiate(masterCard.GetComponent<CardScript>()));

        foreach(CardScript card in cards)
        {
            card.transform.GetComponent<Renderer>().material = randomMaterials[Random.Range(0, randomMaterials.Length)];
        }
    }

    public bool DrawCard(int amount, HandScript hand)
    {
        for (int i = 0; i < amount - 1; i++)
        {
            if (cards.Count > 0)
            {
                //draw
                Debug.Log("You draw a card");
                if(hand.AddCard(cards[0]))
                {
                    //full hand
                    return true;
                }
                cards.RemoveAt(0);
                return true;
            }
            else
            {
                //No cards available
                return false;
            }
        }
        return false;
    }

    public bool DrawHand(HandScript hand)
    {
        for (int i = 0; i <= 4; i++)
        {
            if (cards.Count > 0)
            {
                //draw
                Debug.Log("You draw a card");
                if (!hand.AddCard(cards[0]))
                {
                    //full hand
                    return true;
                }
                cards.RemoveAt(0);
            }
            else
            {
                //No cards available
                return false;
            }
        }
        return false;
    }
}
