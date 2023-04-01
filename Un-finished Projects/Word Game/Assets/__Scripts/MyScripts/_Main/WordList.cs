using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordList : MonoBehaviour
{
    private static WordList S; //Singleton used only from instances of this class

    [Header("Set in inspector")]
    public TextAsset wordListText; //2of12inf asset
    public int numToParseBeforeYield = 10000; //number of words to parse before yielding the coroutine
    public int wordLengthMin = 3;
    public int wordLengthMax = 7;

    [Header("Set dynamically")]
    public int currLine = 0;
    public int totalLines;
    public int longWordCount;
    public int wordCount;

    //Privates\\
    private string[] lines;
    private List<string> longWords;
    private List<string> words;

    private void Awake()
    {
        S = this;
    }

    private void Init() //Replaced the "void Start()"
    {
        lines = wordListText.text.Split('\n'); //Separate every word
        totalLines = lines.Length;

        StartCoroutine(ParseLines());
    }

    static public void INIT()
    {
        S.Init();
    }

    public IEnumerator ParseLines()
    {
        string word;

        //Init the Lists to hold the longest words and all valid words
        longWords = new List<string>();
        words = new List<string>();

        for (int currLine = 0; currLine < totalLines; currLine++)
        {
            word = lines[currLine];

            //if the word is as long as the wordLengthMax store it in the longWords list
            if (word.Length == wordLengthMax)
            {
                longWords.Add(word);
            }

            //If it is between the max and min, store it in the list of valid words
            if (word.Length >= wordLengthMin && word.Length <= wordLengthMax)
            {
                words.Add(word);
            }

            //Determine whether the Coroutine should yield
            if (currLine % numToParseBeforeYield == 0)
            {
                //Count the words in each list to show the word progress
                longWordCount = longWords.Count;
                wordCount = words.Count;
                //Yields execution until next frame
                yield return null;

                //The yield will cause the execution of this method to wait here while other code executes
                //and then continue from this point to the next iteration of the for loop.
            }
        }
        longWordCount = longWords.Count;
        wordCount = words.Count;

        //Send a message to this GO to let it know the parse is complete
        gameObject.SendMessage("WordListParseComplete"); //Calls this method in EVERY script that's attached to the _MainCamera
    }

    //Getters - Can be called from anywhere
    static public List<string> GET_WORDS()
    {
        return S.words;
    }

    static public string GET_WORDS(int index)
    {
        return S.words[index];
    }

    static public List<string> GET_LONG_WORD()
    {
        return S.longWords;
    }

    static public string GET_LONG_WORD(int index)
    {
        return S.longWords[index];
    }

    //Read-Only static public properties
    static public int WORD_COUNT
    {
        get
        {
            return S.wordCount;
        }
    }

    static public int LONG_WORD_COUNT
    {
        get
        {
            return S.longWordCount;
        }
    }

    static public int NUM_TO_PARSE_BEFORE_YIELD
    {
        get
        {
            return S.numToParseBeforeYield;
        }
    }

    static public int WORD_LENGTH_MIN
    {
        get
        {
            return S.wordLengthMin;
        }
    }

    static public int WORD_LENGTH_MAX
    {
        get
        {
            return S.wordLengthMax;
        }
    }
}
