using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door
{
    public string NextScene;
    public string KeyItemName;

    public void Initialize()
    {
        KeyItemName = NextScene = "";
    }
}
