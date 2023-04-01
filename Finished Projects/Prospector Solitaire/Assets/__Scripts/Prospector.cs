using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prospector : MonoBehaviour
{
    static public Prospector S; //Singleton

    [Header("Set in Inspector")]
    public TextAsset deckXML;

    [Header("Set Dynamically")]
    public Deck deck;

    private void Awake()
    {
        S = this; //Singleton setup
    }

    private void Start()
    {
        deck = GetComponent<Deck>(); //Get the Deck 
        deck.InitDeck(deckXML.text); //Pass DeckXML to it
        Deck.Shuffle(ref deck.cards); //the ref keyword must be used here as well

        Card c;
        for (int cNum = 0; cNum < deck.cards.Count; cNum++)
        {
            c = deck.cards[cNum];
            c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        }
    }
}
