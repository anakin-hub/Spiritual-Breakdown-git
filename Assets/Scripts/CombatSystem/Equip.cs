using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{

    [SerializeField] protected int _bonusMod;
    [SerializeField] protected string nome;
    [SerializeField] protected string tipo;
    [SerializeField] protected string descricao;

    public int GetBonus()
    { return _bonusMod; }

    public string GetNome()
    { return nome; }

    public string GetTipo()
    { return tipo; }

    public string GetDescription()
    { return descricao; }

}
