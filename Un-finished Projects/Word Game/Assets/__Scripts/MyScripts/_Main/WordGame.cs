using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//The individual game states
public enum GameMode
{
    preGame, //Before the game starts
    loading, //The word list is laoding and being parsed
    makeLevel, //The individual WordGame level is created
    levelPrep, //The level visuals are Instantiated
    inLevel //The level is in progress
}

/// <summary>
/// Basically the game manager
/// </summary>
public class WordGame : MonoBehaviour
{
    static public WordGame S; //Singleton

    [Header("Set in Inspector")]
    public GameObject prefabLetter;
    public Rect wordArea = new Rect(-24, 19, 48, 28);
    public float letterSize = 1.5f;
    public bool showAllWyrds = true;
    public float bigLetterSize = 4;
    public Color bigColorDim = new Color(0.8f, 0.8f, 0.8f);
    public Color bigColorSelected = new Color(1f, 0.9f, 0.7f);
    public Vector3 bigLetterCenter = new Vector3(0, -16, 0);
    public Color[] wyrdPalette;

    [Header("Set dynamically")]
    public GameMode mode = GameMode.preGame; //Default state
    public WordLevel currLevel;
    public List<Wyrd> wyrds;
    public List<Letter> bigLetters;
    public List<Letter> bigLetterActive;
    public string testWord;

    private Transform letterAnchor, bigLetterAnchor;
    private string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    private void Awake()
    {
        S = this;
        letterAnchor = new GameObject("LetterAnchor").transform;
        bigLetterAnchor = new GameObject("BigLetterAnchor").transform;
    }

    private void Start()
    {
        mode = GameMode.loading;
        WordList.INIT(); //Call the static Init() method of WordList class
    }

    private void Update()
    {
        Letter ltr;
        char c;

        switch (mode)
        {
            case GameMode.inLevel:
                //Iterate through each char input by the player in this frame
                foreach (char cIt in Input.inputString)
                {
                    //Shift cIt to UPPERCASE
                    c = System.Char.ToUpperInvariant(cIt);

                    //Check to see it's an uppercase letter
                    if (upperCase.Contains(c))
                    {
                        //Find an available Letter in bigLetters with this char
                        ltr = FindNextLetterByChar(c);

                        //If a Letter was returned
                        if (ltr != null)
                        {
                            //...then add this char to the testWord and move
                            // the returned big letter to bigLetterActive
                            testWord += c.ToString();
                            //Move it from the inactive to the active List<>
                            bigLetterActive.Add(ltr);
                            bigLetters.Remove(ltr);
                            ltr.color = bigColorSelected; //Make it look active
                            ArrangeBigLetters(); //Rearrange the big letters
                        }
                    }

                    if (c == '\b') //If c is Backspace
                    {
                        //Remove the last Letter in bigLettersActive
                        if (bigLetterActive.Count == 0) 
                        {
                            return;
                        }

                        if (testWord.Length > 1)
                        {
                            //Clear the alst char of testWord
                            testWord = testWord.Substring(0, testWord.Length - 1);
                        }
                        else
                        {
                            testWord = "";
                        }

                        ltr = bigLetterActive[bigLetterActive.Count - 1];
                        //Move it fro the active to the inactive list
                        bigLetterActive.Remove(ltr);
                        bigLetters.Add(ltr);
                        ltr.color = bigColorDim;
                        ArrangeBigLetters(); //re-arrange the big letters
                    }

                    if (c == '\n' || c == '\r')
                    {
                        CheckWord(); //Test the testWord against the words in WordLevel
                    }

                    if (c == ' ') //Space
                    {
                        //Shuffle the letters...
                        bigLetters = ShuffleLetters(bigLetters);
                        //...And then display them.
                        ArrangeBigLetters();
                    }
                }
                break;
        }
    }

    /// <summary>
    /// This method finds the next available char (c) inputed by the player
    /// If there is none, it returns null.
    /// </summary>
    /// <param name="c">The inputed char by the keyboard.</param>
    /// <returns></returns>
    Letter FindNextLetterByChar(char c)
    {
        //Search through each Letter in bigLetters
        foreach (Letter ltr in bigLetters)
        {
            //If one has the same char as c, return it
            if (ltr.c == c)
            {
                return ltr;
            }
        }

        return null; //Otherwise return null
    }

    void CheckWord()
    {
        //Test the testWord against the level.subWords
        string subWord;
        bool foundTestWord = false;

        //Create a List<int> to hold the indices of other subWords that are
        //contained within testWord
        List<int> containedWords = new List<int>();

        //Iterate through each word in currLevel.subWords
        for (int i = 0; i < currLevel.subWords.Count; i++)
        {
            //Chech whether the wyrd has already been found
            if (wyrds[i].found)
            {
                continue;
            }

            subWord = currLevel.subWords[i];
            //Check whether this subWord is the testWord or is contained in it
            if (string.Equals(testWord,subWord))
            {
                HightlightWyrd(i);
                ScoreManager.SCORE(wyrds[i], 1); //Score the testWord
                foundTestWord = true;
            }
            else if (testWord.Contains(subWord))
            {
                containedWords.Add(i);
            }
        }

        if (foundTestWord) //If the test word was found in subWords...
        {
            //...Then hightlights the other words contained in testWord
            int numContained = containedWords.Count;
            int ndx;

            //Highlight the words in reverse order;
            for (int i = 0; i < containedWords.Count; i++)
            {
                ndx = numContained - i - 1;
                HightlightWyrd(containedWords[ndx]);
                ScoreManager.SCORE(wyrds[containedWords[ndx]], i + 2); //I + 2 is the number of this word in the combo.
                //Here SCORE is called to score any smaller words contained within the testWord.
            }
        }

        ClearBigLettersActive();
    }

    /// <summary>
    /// Hightlight a Wyrd
    /// </summary>
    /// <param name="ndx"></param>
    void HightlightWyrd(int ndx)
    {
        wyrds[ndx].found = true; //Activate the subWord and let it know it's been found
        wyrds[ndx].color = (wyrds[ndx].color + Color.white)/2f;
        wyrds[ndx].visible = true; //Make it's 3D Text visible
    }

    /// <summary>
    /// Remvoe all the Letters from bigLettersActive
    /// </summary>
    void ClearBigLettersActive()
    {
        testWord = ""; //Clear the testWord
        foreach (Letter ltr in bigLetterActive)
        {
            bigLetters.Add(ltr); //Add every big Letter in it origin List
            ltr.color = bigColorDim; //Dim it's color
        }

        bigLetterActive.Clear(); //Clear the List from Letters
        ArrangeBigLetters(); //Rearrange the Letters on screen
    }

    public void WordListParseComplete() //Called from the WordList class when parsing is complete
    {
        mode = GameMode.makeLevel;
        //Make a level and assign it to currLevel, the current WordLevel
        currLevel = MakeWordLevel();
    }

    public WordLevel MakeWordLevel(int levelNum = -1)
    {
        WordLevel level = new WordLevel();
        if (levelNum == -1)
        {
            //Pick a random level
            level.longWordIndex = Random.Range(0, WordList.LONG_WORD_COUNT);
        }
        else
        {
            //Added later in the chapter
        }

        level.levelNum = levelNum;
        level.word = WordList.GET_LONG_WORD(level.longWordIndex);
        level.charDict = WordLevel.MakeCharDict(level.word);

        StartCoroutine(FindSubWordsCoroutine(level));

        return level;
    }

    /// <summary>
    /// A coroutine that finds words that can be spelled in this level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public IEnumerator FindSubWordsCoroutine(WordLevel level)
    {
        level.subWords = new List<string>();
        string str;

        List<string> words = WordList.GET_WORDS();

        //Iterate through all the words in the WordList
        for (int i = 0; i < WordList.WORD_COUNT; i++)
        {
            str = words[i];
            //Check whether each one can be spelled using level.charDict
            if (WordLevel.CheckWordInLevel(str,level))
            {
                level.subWords.Add(str);
            }

            //Yield if we've parsed a lot of words this frame
            if (i % WordList.NUM_TO_PARSE_BEFORE_YIELD == 0)
            {
                //yield until next frame
                yield return null;
            }
        }

        level.subWords.Sort();
        level.subWords = SortWordByLength(level.subWords).ToList();

        //The coroutine is complete, so call SubWordSearchComplete()
        SubWordSearchComplete();
    }

    //Use LINQ to sort the array received and return a copy
    public static IEnumerable<string> SortWordByLength(IEnumerable<string> ws)
    {
        ws = ws.OrderBy(s => s.Length);
        return ws;
    }

    public void SubWordSearchComplete()
    {
        mode = GameMode.levelPrep;
        CreateLayout(); //Call the Layout() function once WordSearch is done
    }

    private void CreateLayout()
    {
        wyrds = new List<Wyrd>(); //Place the letters for each subword of currLevel on screen

        //Declare all the variables needed for the function
        GameObject go;
        Letter lett;
        string word;
        Vector3 pos;
        float left = 0;
        float columnWidth = 3;
        char c;
        Color col;
        Wyrd wyrd;

        //Determine how many rows of Letters will fit on screen;
        int numOfRows = Mathf.RoundToInt(wordArea.height / letterSize);

        //Make a Wyrd of each level.subWord;
        for (int i = 0; i < currLevel.subWords.Count; i++)
        {
            wyrd = new Wyrd();
            word = currLevel.subWords[i];

            //If the word is longer than columnWidth, expand it
            columnWidth = Mathf.Max(columnWidth, word.Length);

            //Instantiate a PrefabLetter for each letter of the word
            for (int j = 0; j < word.Length; j++)
            {
                c = word[j];
                go = Instantiate<GameObject>(prefabLetter);
                go.transform.SetParent(letterAnchor);
                lett = go.GetComponent<Letter>();
                lett.c = c;

                //Position the Letter
                pos = new Vector3(wordArea.x + left + j * letterSize, wordArea.y, 0);

                //The % here makes multiple columns line up
                pos.y -= (i % numOfRows) * letterSize;

                //Move the lettimmediately to a position above the screen
                lett.posImmediate = pos + Vector3.up * (20 + i % numOfRows);

                //Then set the pos for it to interpolate to
                lett.pos = pos;
                //Increment lett.timeStart to move wyrds at different times
                lett.timeStart = Time.time + i * 0.05f;

                go.transform.localScale = Vector3.one * letterSize;

                wyrd.Add(lett);
            }

            if (showAllWyrds)
            {
                wyrd.visible = true;
            }

            //Color the wyrd based on length
            wyrd.color = wyrdPalette[word.Length - WordList.WORD_LENGTH_MIN];

            wyrds.Add(wyrd);

            //If we've gotten to the numRows(th) row, start a new column
            if (i%numOfRows == numOfRows - 1)
            {
                left += (columnWidth + 0.5f) * letterSize;
            }
        }

        //Place the big letters
        //Initialize the List<>s for big letters
        bigLetters = new List<Letter>();
        bigLetterActive = new List<Letter>();

        //Create a big Letter for each letter in the target word
        for (int i = 0; i < currLevel.word.Length; i++)
        {
            //This is similar to the process for a normal letter
            c = currLevel.word[i];
            go = Instantiate<GameObject>(prefabLetter);
            go.transform.SetParent(bigLetterAnchor);
            lett = go.GetComponent<Letter>();
            lett.c = c;
            go.transform.localScale = Vector3.one * bigLetterSize;

            //Set the initial position of the big Letter below screen
            pos = new Vector3(0, -100, 0);

            lett.posImmediate = pos;
            lett.pos = pos;
            //Increment lett.TimeStart to have big Letters come in last
            lett.timeStart = Time.time + currLevel.subWords.Count * 0.05f;
            lett.easingCurve = Easing.Sin + "-0.18"; //Bouncy easing

            col = bigColorDim;
            lett.color = col;
            lett.visible = true; //Always true for big letters
            lett.big = true;
            bigLetters.Add(lett);
        }

        //Shuffle the big letters
        bigLetters = ShuffleLetters(bigLetters);

        //Arrange them on screen
        ArrangeBigLetters();

        //Set the mode to GameMode.inLevel;
        mode = GameMode.inLevel;
    }
    List<Letter> ShuffleLetters(List<Letter> letts) //Shuffles the big letters
    {
        List<Letter> newL = new List<Letter>();
        int ndx;

        while (letts.Count > 0)
        {
            ndx = Random.Range(0, letts.Count);
            newL.Add(letts[ndx]);
            letts.RemoveAt(ndx);
        }
        return newL;
    }

    /// <summary>
    /// Arranges the big letters on screen
    /// </summary>
    private void ArrangeBigLetters()
    {
        //The halfWidth allows the big letters on screen
        float halfWidth = (bigLetters.Count) / 2f - 0.5f;
        Vector3 pos;
        for (int i = 0; i < bigLetters.Count; i++)
        {
            pos = bigLetterCenter;
            pos.x += (i - halfWidth) * bigLetterSize;
            bigLetters[i].pos = pos;
        }

        //Big letters size
        halfWidth = (bigLetterActive.Count) / 2f - 0.5f;
        for (int i = 0; i < bigLetterActive.Count; i++)
        {
            pos = bigLetterCenter;
            pos.x += (i - halfWidth) * bigLetterSize;
            pos.y += bigLetterSize * 1.25f;
            bigLetterActive[i].pos = pos;
        }
    }
}
