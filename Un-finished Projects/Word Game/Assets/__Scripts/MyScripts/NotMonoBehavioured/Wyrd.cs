using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wyrd acts as a collection of Letters
/// </summary>
public class Wyrd //Does not extend MB
{
    public string str; //A string representation of the word
    public List<Letter> letters = new List<Letter>();
    public bool found = false;

    /// <summary>
    /// A property to set visibility of the 3D Text of each letter
    /// </summary>
    public bool visible
    {
        get
        {
            if (letters.Count == 0)
            {
                return false;
            }
            return letters[0].visible;
        }
        set
        {
            foreach (Letter l in letters)
            {
                l.visible = value;
            }
        }
    }

    /// <summary>
    /// A property to set each rectangle's color 
    /// </summary>
    public Color color
    {
        get
        {
            if (letters.Count == 0)
            {
                return Color.black;
            }
            return letters[0].color;
        }
        set
        {
            foreach (Letter letter in letters)
            {
                letter.color = value;
            }
        }
    }

    /// <summary>
    /// Adds a Letter to letters
    /// </summary>
    /// <param name="letter">The letter you want to add</param>
    public void Add(Letter letter)
    {
        letters.Add(letter);
        str += letter.c.ToString();
    }
}
