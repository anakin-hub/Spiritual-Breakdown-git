using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door
{
    public string NextScene;
    public string KeyItemName;
    public bool locked;

    public void Initialize()
    {
        locked = false;
        KeyItemName = NextScene = "";
    }
}
