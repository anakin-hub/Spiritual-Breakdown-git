using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleHUD : MonoBehaviour
{
    [SerializeField] private float _reduceSpeed;
    private float _maxHP;
    private float _currentHP;
    private float _target;
    private bool _isSet;
    private bool _readyHP;

    public TMP_Text nameText;
    public Image HPslider;

    public void SetHUD(Character unit)
    {
        nameText.text = unit.UnitName;
        _maxHP = unit.GetMaxHP();
        _currentHP = unit.GetCurrentHP();
        _target = _currentHP / _maxHP;
        _isSet = true;
        _readyHP = false;
    }

    public void setHP(int hp)
    {
        _currentHP = hp;
        _target = _currentHP / _maxHP;
        if(_target < 0)
            _target = 0;
        _readyHP = false;
    }

    public bool GetReady()
    { return _readyHP; }

    public float GetTarget()
    { return _target; }

    private void Update()
    {
        if (_isSet)
            HPslider.fillAmount = Mathf.MoveTowards(HPslider.fillAmount, _target, _reduceSpeed * Time.deltaTime);

        if(HPslider.fillAmount == _target && !_readyHP)
            _readyHP = true;
    }
}
