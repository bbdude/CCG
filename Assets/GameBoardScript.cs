using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    //Prefabs
    public GameObject deckPrefab;
    public GameObject handPrefab;
    public GameObject framePrefab;

    //Decks
    public GameObject pDeckObj;
    public GameObject eDeckObj;
    public DeckScript pDeck;
    public DeckScript eDeck;
    public HandScript pHand;
    public HandScript eHand;

    public List<CardScript> initPhaseCards = new List<CardScript>();
    public List<CardStruct> pPlayedCards = new List<CardStruct>();
    public List<CardStruct> ePlayedCards = new List<CardStruct>();


    public List<Vector3> pCardSlots = new List<Vector3>();
    public List<Vector3> eCardSlots = new List<Vector3>();
    public List<CardSlot> pSlots = new List<CardSlot>();
    public List<CardSlot> eSlots = new List<CardSlot>();

    public struct CardStruct
    {
        public Vector3 location;
        public int position;
        public CardScript card;

        public CardStruct(int pos, Vector3 loc, CardScript newCard)
        {
            location = loc;
            position = pos;
            card = newCard;
            card.transform.position = loc;
        }
    }

    public struct CardSlot
    {
        public Vector3 location;
        public GameObject slot;
        public bool occupied;
        public int position;

        public CardSlot(Vector3 loc, GameObject cardSlot, bool occ, int pos)
        {
            slot = cardSlot;
            location = loc;
            occupied = occ;
            position = pos;
        }
    }

    public enum TurnPhases { INIT, DRAW, MAINPHASE, ATTACK, ENDPHASE, END };
    public TurnPhases currentPhase;
    public bool playerTurn = true;
    public int turns = 0;
    public int timer = 0;

    public bool holdingCard = false;
    public CardScript heldCard;
    public CardScript enlargedCard;

    // Start is called before the first frame update
    void Start()
    {
        pDeckObj = Instantiate(deckPrefab, new Vector3(4.5f, -3.75f, 0), Quaternion.identity);
        pDeckObj.name = "pDeck";
        eDeckObj = Instantiate(deckPrefab, new Vector3(-4.5f, 3.75f, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
        eDeckObj.name = "eDeck";

        pDeck = pDeckObj.GetComponent<DeckScript>();
        eDeck = eDeckObj.GetComponent<DeckScript>();


        GameObject pHandObj = Instantiate(handPrefab, new Vector3(0, -3.75f, 0), Quaternion.identity);
        pHandObj.name = "pHand";
        GameObject eHandObj = Instantiate(handPrefab, new Vector3(0, 3.75f, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
        eHandObj.name = "eHand";

        pHand = pHandObj.GetComponent<HandScript>();
        eHand = eHandObj.GetComponent<HandScript>();

        currentPhase = TurnPhases.INIT;

        for (int i = 0; i < 6; i++)
        {
            eCardSlots.Add(new Vector3(-4 + (1.6f * i), 1.25f, -1.5f));

            eSlots.Add(new CardSlot(new Vector3(-4 + (1.6f * i), 1.25f, 0.5f), Instantiate(framePrefab, new Vector3(-4 + (1.6f * i), 1.25f, 0.5f), Quaternion.Euler(180,90,0), this.transform), false, i));
            eSlots[i].slot.transform.localScale = new Vector3(1.6f, 2.5f, 1.25f);
            eSlots[i].slot.name = "enemycardslot";

            pCardSlots.Add(new Vector3(-4 + (1.6f * i), -1.25f, -1.5f));

            pSlots.Add(new CardSlot(new Vector3(-4 + (1.6f * i), -1.25f, 0.5f), Instantiate(framePrefab, new Vector3(-4 + (1.6f * i), -1.25f, 0.5f), Quaternion.Euler(180, 90, 0), this.transform), false, i));
            pSlots[i].slot.transform.localScale = new Vector3(1.6f, 2.5f, 1.25f);
            pSlots[i].slot.name = "playercardslot";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            timer--;
        switch (currentPhase)
        {
            case TurnPhases.INIT:
                if (initPhaseCards.Count == 0 || turns == 0)
                {
                    currentPhase++;
                }
                break;
            case TurnPhases.DRAW:
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.name == "pDeck")
                        {
                            if (turns == 0)
                            {
                                pDeck.DrawHand(pHand);
                                eDeck.DrawHand(eHand);
                                currentPhase++;
                            }
                            else
                            {
                                pDeck.DrawCard(1, pHand);
                                currentPhase++;
                            }
                        }
                        else
                        {
                            //Debug.Log("Nothing happens");
                        }
                    }
                }
                break;
            case TurnPhases.MAINPHASE:
                {
                    if (holdingCard)
                    {
                        if (Input.GetMouseButtonDown(0) && timer == 0)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit))
                            {
                                if (hit.transform.tag == "Frame")
                                {
                                    MoveUpCard(heldCard, hit.transform.gameObject);
                                }
                            }
                            timer = 20;
                        }
                        else
                        {
                            heldCard.transform.parent = null;
                            Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                            heldCard.transform.position = new Vector3(temp.x, temp.y, -0.5f);
                        }
                    }
                    else
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.tag == "Card")
                            {

                                if (Input.GetMouseButtonDown(0) && timer == 0)
                                {
                                    PickUpCard(hit.transform.GetComponent<CardScript>());
                                    hit.transform.GetComponent<Collider>().enabled = false;
                                    enlargedCard.transform.gameObject.SetActive(false);
                                }
                                else
                                {
                                    //Debug.Log("EnlargedCard");
                                    enlargedCard.GetComponent<Renderer>().material = hit.transform.GetComponent<Renderer>().material;
                                    enlargedCard.transform.gameObject.SetActive(true);
                                }
                            }
                            else
                            {
                                enlargedCard.transform.gameObject.SetActive(false);
                                //No card selected
                            }
                        }
                    }
                }
                break;
            case TurnPhases.ATTACK:
                break;
            case TurnPhases.ENDPHASE:
                break;
            case TurnPhases.END:
                break;
        }
    }

    public void PickUpCard(CardScript movedCard)
    {
        holdingCard = true;
        heldCard = movedCard;
    }
    public void MoveUpCard(CardScript movedCard, GameObject target)
    {
        if (target.name == "playercardslot")
        {
            Debug.Log("Target Slot");
            foreach ( CardSlot slot in pSlots)
            {
                if (GameObject.ReferenceEquals(slot.slot, target))
                {// found the right slot
                    if (slot.occupied)
                    {
                        //do nothing right now
                        Debug.Log("Slot filled");
                    }
                    else
                    {
                        Debug.Log("Set card");
                        holdingCard = false;
                        movedCard.transform.SetParent(this.transform);
                        ePlayedCards.Add(new CardStruct(slot.position, pCardSlots[slot.position], movedCard));
                        heldCard.transform.GetComponent<Collider>().enabled = true;
                        heldCard = null;

                        //target.transform.GetComponent<Collider>().enabled = false;
                    }
                }
            }
        }
    }
}
