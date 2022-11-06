using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    private Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }
    private void OnMouseEnter()
    {
        render.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        render.material.color = Color.white;
    }
}
