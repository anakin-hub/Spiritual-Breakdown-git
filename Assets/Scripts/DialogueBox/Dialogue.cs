using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public List<Sentence> sentences;

    public void Initialize()
    {
        sentences = new List<Sentence>();
    }
}
