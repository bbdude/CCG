using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    //Prefabs
    public GameObject deckPrefab;
    public GameObject handPrefab;

    //Decks
    public GameObject pDeckObj;
    public GameObject eDeckObj;
    public DeckScript pDeck;
    public DeckScript eDeck;
    public HandScript pHand;
    public HandScript eHand;

    public List<CardScript> initPhaseCards = new List<CardScript>();

    public enum TurnPhases { INIT, DRAW, MAINPHASE, ATTACK, ENDPHASE, END };
    public TurnPhases currentPhase;
    public bool playerTurn = true;
    public int turns = 0;

    public bool enlarged = false;
    public CardScript enlargedCard;

    // Start is called before the first frame update
    void Start()
    {
        pDeckObj = Instantiate(deckPrefab, new Vector3(4.5f, -3.5f, 0), Quaternion.identity);
        pDeckObj.name = "pDeck";
        eDeckObj = Instantiate(deckPrefab, new Vector3(-4.5f, 3.5f, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
        eDeckObj.name = "eDeck";

        pDeck = pDeckObj.GetComponent<DeckScript>();
        eDeck = eDeckObj.GetComponent<DeckScript>();


        GameObject pHandObj = Instantiate(handPrefab, new Vector3(0, -3.5f, 0), Quaternion.identity);
        pHandObj.name = "pHand";
        GameObject eHandObj = Instantiate(handPrefab, new Vector3(0, 3.5f, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
        eHandObj.name = "eHand";

        pHand = pHandObj.GetComponent<HandScript>();
        eHand = eHandObj.GetComponent<HandScript>();

        currentPhase = TurnPhases.INIT;
    }

    // Update is called once per frame
    void Update()
    {
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
                            Debug.Log("Nothing happens");
                        }
                    }
                }
                break;
            case TurnPhases.MAINPHASE:
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "Card")
                        {
                            
                            Debug.Log("EnlargedCard");
                            enlargedCard.GetComponent<Renderer>().material = hit.transform.GetComponent<Renderer>().material;
                            enlargedCard.transform.gameObject.SetActive(true);
                        }
                        else
                        {
                            enlargedCard.transform.gameObject.SetActive(false);
                            //No card selected
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
}
