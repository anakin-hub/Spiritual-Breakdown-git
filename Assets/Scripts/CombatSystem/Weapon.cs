using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equip
{
    [SerializeField] 
    int _critrate;
    [SerializeField] protected int _damage;
    [SerializeField] protected bool _magicdmgType; //if true magic dmg, if false physical dmg

    public int GetCritrate()
    { return _critrate; }

    public int GetDamage()
    { return _damage; }

    public bool GetdmgType()
    { return _magicdmgType; }
}
