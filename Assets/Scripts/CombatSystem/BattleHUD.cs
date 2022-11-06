using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleHUD : MonoBehaviour
{
    [SerializeField] private float _reduceSpeed;
    private float _target;
    private bool _isSet;

    public TMP_Text nameText;
    public Slider HPslider;

    public void SetHUD(Character unit)
    {
        nameText.text = unit.UnitName;
        HPslider.maxValue = unit.GetMaxHP();
        HPslider.value = _target = unit.GetCurrentHP();
        _isSet = true;
    }

    public void setHP(int hp)
    {
        _target = hp;
    }

    private void Update()
    {
        if (_isSet)
            HPslider.value = Mathf.MoveTowards(HPslider.value, _target, _reduceSpeed * Time.deltaTime);

    }
}
