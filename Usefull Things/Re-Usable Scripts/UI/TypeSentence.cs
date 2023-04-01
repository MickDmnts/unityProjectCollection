using System;
using UnityEngine;
using System.Collections;

public class TypeSentence: MonoBehaviour
{
    public IEnumerator TypeSentence(DialogueBox dialogueBox, string sentenceToType)
    {
        dialogueBox.text = "";

        foreach (char letter in sentenceToType.ToCharArray())
        {
            dialogueBox.text += letter;
            yield return null;
        }
    }
}