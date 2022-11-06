using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string name;
    [TextArea(3, 10)]
    public string sentence;
    public string buttonText;

    public void Initialize()
    {
        name = sentence = buttonText = "";
    }
}
