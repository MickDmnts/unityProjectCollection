using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour 
{
    [Header("Set Dynamically")]
    public string suit; //Suit of the rank (C,D,H or S)
    public int rank; //Rank of the Card (1-14)
    public Color color = Color.black; //Color to ting pips
    public string colS = "Black"; //or "Red", Name of the Color

    //List that holds all the Decorator GOs of this card
    public List<GameObject> decoGOs = new List<GameObject>();
    //List that holds all the pip GOs of this card
    public List<GameObject> pipGOs = new List<GameObject>();

    public GameObject backOfTheCard; //The GO of the back of the card
    public CardDefinition def; //Parsed from DeckXML.xml

    public bool faceUp
    {
        get
        {
            return !backOfTheCard.activeSelf;
        }
        set
        {
            backOfTheCard.SetActive(!value);
        }
    }
}

[System.Serializable] //Serializable classes are able to be edited in the inspector
public class Decorator
{
	public string type; //For card pips, type="pip"
	public Vector3 spriteLoc; //The location of the Sprite in the card
	public bool flip = false; //Whether to flip the sprite verticaly
	public float scale = 1f; //Scale of the sprite
}


[System.Serializable]
public class CardDefinition //Stores information for each rank of card
{
    public string face; //Sprite to use for each face card
    public int rank; //The rank (1-13) of THIS card
    public List<Decorator> pips = new List<Decorator>(); //Pips used
}