using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Activator : MonoBehaviour
{
    [SerializeField] Change_Scene scene;
    [SerializeField] protected string KeyName;
    [SerializeField] protected bool autorized;
    [SerializeField] protected UI_interaction dialogue;

    protected void Awake()
    {
        autorized = false;
    }

    public void ChangeScene()
    {
        if (autorized)
            scene.LoadNextLevel();
        else
            dialogue.Texting("Quem � aquele homem ali atr�s?...");
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out movement move))
        {
            autorized = move.Search(KeyName);
        }
    }
}
