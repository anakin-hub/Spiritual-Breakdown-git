using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skill : MonoBehaviour
{
    [SerializeField] protected string _skillName;
    [SerializeField] protected string _skillDescription;
    [SerializeField] protected bool _hasCritic;
    [SerializeField] protected int _rolledDice; //d20 rolled
    [SerializeField] protected int _bonusatk; //will be calculate with rolledDice
    [SerializeField] protected int _numDices; //number of dices to use with damage Dice
    [SerializeField] protected int _damageDice;//value of the damage dices (numDices = 2, damageDice = 10, Ex.: 2d10)
    [SerializeField] protected int _coolDown;
    [SerializeField] protected int _castedTurn;
    [SerializeField] protected bool _ready;
    [SerializeField] protected bool _skillUsed;
    [SerializeField] protected bool _isDead;


    public string GetSkillName()
    { return _skillName; }

    public string GetSkillDescription()
    { return _skillDescription; }

    protected void Start()
    {
        _ready = true;
        _isDead = _skillUsed = false;
        if(_numDices <= 0)
            _numDices = 1;
    }

    public bool GetSkillUsed()
    {
        if (_skillUsed)
            _skillUsed = false;
        else     
            return _skillUsed;

        return true;
    }

    protected virtual void UseSkill(int currentturn)
    {
        _castedTurn = currentturn;
        _ready = false;
    }

    public bool GetReady()
    { return _ready; }

    public virtual void UpdateSkill(int currentturn)
    {
        if (currentturn >= _castedTurn + _coolDown)
            _ready = true;
    }

    protected int RollDice(int _dNumber)
    {
        int rolled;

        rolled = Random.Range(1, _dNumber);

        return rolled;
    }

    public bool victory()
    { return _isDead; }
}
