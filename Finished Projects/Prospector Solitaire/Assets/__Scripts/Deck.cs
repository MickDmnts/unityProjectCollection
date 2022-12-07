using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    //Card Sprites
    [Header("Set in Inspector")]
    public bool startFaceUp = false;

    //Suits
    public Sprite suitClub;
    public Sprite suitDiamond;
    public Sprite suitHeart;
    public Sprite suitSpade;

    public Sprite[] faceSprites;
    public Sprite[] rankSprites;

    public Sprite cardBack;
    public Sprite cardBackGold;
    public Sprite cardFront;
    public Sprite cardFrontGold;

    //Prefabs
    [Header("Card Prefabs")]
    public GameObject prefabCard;
    public GameObject prefabSprite;

    [Header("Set Dynamically")]
    public PT_XMLReader xmlr;
    public List<string> cardNames;
    public List<Card> cards;
    public List<Decorator> decorators;
    public List<CardDefinition> cardDefs;
    public Transform deckAnchor;
    public Dictionary<string, Sprite> dictSuits;

    //InitDeck is called by Prospector when it is ready
    public void InitDeck(string deckXMLText)
    {
        //This creates an anchor for all the Card GOs in the hierarchy
        if (GameObject.Find("_Deck")==null)
        {
            GameObject anchorGO = new GameObject("_Deck");
            deckAnchor = anchorGO.transform;
        }

        //Initialize the Dictionary of SuitSprites with the necessary sprites
        dictSuits = new Dictionary<string, Sprite>()
        {
            {"C", suitClub },
            {"D", suitDiamond },
            {"H", suitHeart },
            {"S", suitSpade },
        };

        ReadDeck(deckXMLText);

        MakeCards();
    }

    //ReadDeck parses the XML file passed to it into CardDefinitions
    public void ReadDeck(string deckXMLText)
    {
        xmlr = new PT_XMLReader(); //Create a new PT_XMLReader();
        xmlr.Parse(deckXMLText); //Use the PT_XMLReader to parse DeckXML

        //Read decorators for all cards
        decorators = new List<Decorator>();
        //Grab an PT_XMLHashList of all <decorator>s in the XML file
        PT_XMLHashList xDecos = xmlr.xml["xml"][0]["decorator"];

        Decorator deco;
        for (int i = 0; i < xDecos.Count; i++)
        {
            //For each decorator in the XML
            deco = new Decorator(); //Make a new Deco

            //Copy the attributes from the <decorator> to the Decorator
            deco.type = xDecos[i].att("type");
            //bool deco.flip is true if the text of the flip attribute is "1"
            deco.flip = (xDecos[i].att("flip") == "1");
            //floats need to be parsed from the attribute strings
            deco.scale = float.Parse(xDecos[i].att("scale"));
            //Vector3 loc initializes to [0,0,0], we just need to modify it
            deco.spriteLoc.x = float.Parse(xDecos[i].att("x"));
            deco.spriteLoc.y = float.Parse(xDecos[i].att("y"));
            deco.spriteLoc.z = float.Parse(xDecos[i].att("z"));

            //add the temporary deco to the list of Decorators
            decorators.Add(deco);
        }

        //Read pip locations for each card number
        cardDefs = new List<CardDefinition>();
        //Grab an XMLHashList for every <card> in the XML file
        PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];
        for (int i = 0; i < xCardDefs.Count; i++)
        {
            //for each of the <card>s
            //Create a new CardDefinition
            CardDefinition cDef = new CardDefinition();
            //Parse the attribute values and add them to cDef
            cDef.rank = int.Parse(xCardDefs[i].att("rank"));
            //Grab a PT_HashList of all the <pip>s on THIS card
            PT_XMLHashList xPips = xCardDefs[i]["pip"];
            if (xPips != null)
            {
                for (int j = 0; j < xPips.Count; j++)
                {
                    //Iterate though all the <pip>s
                    deco = new Decorator();
                    // <pip>s on the <card> are handled via the Decorator Class
                    deco.type = "pip";
                    deco.flip = (xPips[j].att("flip") == "1");
                    deco.spriteLoc.x = float.Parse(xPips[j].att("x"));
                    deco.spriteLoc.y = float.Parse(xPips[j].att("y"));
                    deco.spriteLoc.z = float.Parse(xPips[j].att("z"));
                    if (xPips[j].HasAtt("scale"))
                    {
                        deco.scale = float.Parse(xPips[j].att("scale"));
                    }
                    cDef.pips.Add(deco);
                }
            }

            //Face cards have a face attribute
            if (xCardDefs[i].HasAtt("face"))
            {
                cDef.face = xCardDefs[i].att("face");
            }
            cardDefs.Add(cDef);
        }
    }

    //Get the proper CardDefinition based on Rank (1 to 14 is Ace to King)
    public CardDefinition GetCardDefinitionByRank(int cardRank)
    {
        foreach (CardDefinition card in cardDefs) //Search through all of the CardDefinitions
        {
            if (card.rank == cardRank) //If the rank is correct , return this definition
            {
                return card;
            }
        }
        return null;
    }

    //==================================== Card Names - Builders ==============================\\

    public void MakeCards() //NameBuilder
    {
        //cardNames will be the names of the cards to build
        //Each suit goes from 1 to 14 e.g. C1, C14...
        cardNames = new List<string>();
        string[] letters = new string[] { "C", "D", "H", "S" };
        foreach (string s in letters)
        {
            for (int i = 0; i < 13; i++)
            {
                cardNames.Add(s + (i + 1)); //Add the name of the card (e.g. C1, H4 etc.)
            }
        }

        //Make a list to hold all the cards
        cards = new List<Card>();

        //Iterate through all the cardNames that were just made
        for (int i = 0; i < cardNames.Count; i++)
        {
            cards.Add(MakeIndividualCard(i)); //Make the card and THEN add it to the card deck
        }
    }

    public Card MakeIndividualCard(int cardNumber)
    {
        //Instantiate a new Card GameObject
        GameObject cgo = Instantiate(prefabCard) as GameObject;

        //Set the transform.parent of the new card to the anchor
        cgo.transform.parent = deckAnchor;

        //Get the Card component
        Card card = cgo.GetComponent<Card>();

        //This line stacks the cards so that they are all in nice rows
        cgo.transform.localPosition = new Vector3((cardNumber % 13) * 3, cardNumber / 13 * 4, 0);

        //Assing basic values to the cards
        card.name = cardNames[cardNumber];
        card.suit = card.name[0].ToString();
        card.rank = int.Parse(card.name.Substring(1));
        if (card.suit == "D" || card.suit == "H")
        {
            card.colS = "Red";
            card.color = Color.red;
        }

        //Pull the CardDefinition from THIS card
        card.def = GetCardDefinitionByRank(card.rank);

        AddDecorators(card);
        AddPips(card);
        AddFace(card);
        AddBack(card);

        return card;
    }

    //============================= Card Makers ===============================\\

    //These private variables will be reused several times in helper methods
    private Sprite _tSp = null;
    private GameObject _tGO = null;
    private SpriteRenderer _tSR = null;

    /// <summary>
    /// Adds the decorators (Numbers and symbols below numbers) of the CARD passed to it - total 40
    /// </summary>
    /// <param name="card"></param>
    private void AddDecorators(Card card)
    {
        //Add decorators
        foreach (Decorator deco in decorators)
        {
            if (deco.type == "suit")
            {
                _tGO = Instantiate(prefabSprite) as GameObject; //Instantiate a Sprite GameObject
                _tSR = _tGO.GetComponent<SpriteRenderer>(); //Get the Sprite Renderer component
                _tSR.sprite = dictSuits[card.suit]; //Set the Sprite to the proper suit
            }
            else
            {
                _tGO = Instantiate(prefabSprite) as GameObject; //Instantiate the sprite prefab
                _tSR = _tGO.GetComponent<SpriteRenderer>();
                _tSp = rankSprites[card.rank]; //Get the proper Sprite to show this rank
                _tSR.sprite = _tSp; //Assign the sprite to the SpriteRenderer
                _tSR.color = card.color; //Set the color of the rank to match the suit
            }

            //Make the deco Sprites to render above the cards
            _tSR.sortingOrder = 1;
            //Make the deco object a child of the of the card
            _tGO.transform.SetParent(card.transform);
            //Set the local position based on the location from DeckXML
            _tGO.transform.localPosition = deco.spriteLoc;
            if (deco.flip)
            {
                _tGO.transform.rotation = Quaternion.Euler(0, 0, 180); //Flips it
            }
            //Set the scale to keep decos from being too high
            if (deco.scale != 1)
            {
                _tGO.transform.localScale = Vector3.one * deco.scale;
            }
            //Name this GameObject so it's easy to see
            _tGO.name = deco.type;
            //Add this deco GameObject to the List card.decoGOs
            card.decoGOs.Add(_tGO);
        }
    }

    /// <summary>
    /// Adds the pips (Symbols in the middle) of the CARD passed to it - total 40
    /// </summary>
    /// <param name="card"></param>
    private void AddPips(Card card)
    {
        //For each of the pips in the definition
        foreach (Decorator pip in card.def.pips)
        {
            //Instantiate a Sprite GameObject
            _tGO = Instantiate(prefabSprite) as GameObject;
            //Set the parent to be the card GameObject
            _tGO.transform.SetParent(card.transform);
            //Set the position to that specified from the XML file
            _tGO.transform.localPosition = pip.spriteLoc;
            //Flip it if necessary
            if (pip.flip)
            {
                _tGO.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            //Scale it if necessary (only for the Ace card)
            if (pip.scale != 1)
            {
                _tGO.transform.localScale = Vector3.one * pip.scale;
            }
            //Give the GameObject a name
            _tGO.name = "pip";

            //Get the tGO's SpriteRenderer component
            _tSR = _tGO.GetComponent<SpriteRenderer>();
            //Set the Sprite to the proper suit
            _tSR.sprite = dictSuits[card.suit];
            //Set sortingOrder to be above the Card_Front
            _tSR.sortingOrder = 1;
            //Ad this to the card's list of pips
            card.pipGOs.Add(_tGO);
        }
    }

    /// <summary>
    /// Add the faces to the Face Cards - total 12
    /// </summary>
    /// <param name="card"></param>
    private void AddFace(Card card)
    {
        if (card.def.face == "")
        {
            return; //If this is NOT a face card, no need to execute this method
        }

        _tGO = Instantiate(prefabSprite) as GameObject;
        _tSR = _tGO.GetComponent<SpriteRenderer>();
        //Generate the right name and pass it to GetFace()
        _tSp = GetFace(card.def.face + card.suit);
        _tSR.sprite = _tSp; //Assign this sprite to _tSR
        _tSR.sortingOrder = 1;
        _tGO.transform.SetParent(card.transform);
        _tGO.transform.localPosition = Vector3.zero;
        _tGO.name = "face";
    }

    /// <summary>
    /// When called from AddFace() it searches for the appropriate face to add to the card
    /// </summary>
    /// <param name="faceS"></param>
    /// <returns></returns>
    private Sprite GetFace(string faceS)
    {
        foreach (Sprite _tSP in faceSprites)
        {
            //If this Sprite has the right name
            if (_tSP.name == faceS)
            {
                return _tSP; //Return the Sprite
            }
        }
        return null; //Else return null
    }

    /// <summary>
    /// Adds the backCard to every card -- total 52
    /// </summary>
    /// <param name="card"></param>
    private void AddBack(Card card)
    {
        //Add Card Back
        //The Card_Back will be able to cover everything else on the Card
        _tGO = Instantiate(prefabSprite) as GameObject;
        _tSR = _tGO.GetComponent<SpriteRenderer>();
        _tSR.sprite = cardBack;
        _tGO.transform.SetParent(card.transform);
        _tGO.transform.localPosition = Vector3.zero;

        //Higher sorting layer than everything else
        _tSR.sortingOrder = 2;
        _tGO.name = "back";
        card.backOfTheCard = _tGO;

        //Default to faceUp
        card.faceUp = startFaceUp;
    }

    //=================================== Shuffling =========================================\\

    static public void Shuffle(ref List<Card> oCards) // pg 660 - ref keyword
    {
        //Create a new list to hold the shuffled cards
        List<Card> tCards = new List<Card>();

        int ndx; //This will hold the index of the card to be moved
        tCards = new List<Card>(); //Initialize the temporary list
        //Repeat as long as there are cards in the original list
        while (oCards.Count > 0)
        {
            //Pick a random card index
            ndx = Random.Range(0, oCards.Count);
            //Add that card to the temporary list
            tCards.Add(oCards[ndx]);
            //And remove that card from the original list
            oCards.RemoveAt(ndx);
        }
        //Replace the original list with the shuffled list
        oCards = tCards;
        //Because the original list is passed in as a (ref)erence
        //everythings that happens on the temporary one, happens in the origina too
    }
}