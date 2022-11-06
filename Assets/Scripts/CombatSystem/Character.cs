using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILLSTATUS { READY, RECHARGING, USED}
public enum TURNSTATE { WAITING, ACTION, DEAD}

public class Character : MonoBehaviour
{
    public string UnitName;
    public int unitLevel;
    public TURNSTATE state;
    public int turn;
    protected bool turnStarted;
    [SerializeField] protected bool target;

    //atributos
    [SerializeField] protected int _forca;
    [SerializeField] protected int _constituicao;
    [SerializeField] protected int _conhecimento;
    [SerializeField] protected int _vontade;
    [SerializeField] protected int _ameaca;//mecanica de se esconder/ser atacado
    //Status
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int _defesa; //indica o valor que precisa ser superado para ser atacado pelo adversario
    [SerializeField] protected int _defMente; //indica o valor que precisa ser superado para ser atacado por magias que mexem com a mente
    [SerializeField] protected int _atkfisico; //valor que é adicionado no sorteio do numero do ataque fisico (aumenta a chance de acerto)
    [SerializeField] protected int _atkmagico; //valor que é adicionado no sorteio do numero do ataque magico (aumenta a chance de acerto)
    [SerializeField] protected int _critRate;
    //Equipamentos
    public Weapon _arma;
    public Equip _armadura;
    [SerializeField] protected int _bonusArmadura;
    [SerializeField] protected List<Equip> equipamentos;

    Renderer _renderer;
    bool dmgTaken;

    protected void Awake()
    {
        turn = 0;
        turnStarted = false;
        dmgTaken = target = false;
        _bonusArmadura = 0;
        maxHP = 10 + _constituicao * 3;
        currentHP = maxHP;

        if (_armadura.GetBonus() > 0)
            _bonusArmadura = _armadura.GetBonus();

        _defesa = 10 + _conhecimento + _bonusArmadura;
        _defMente = 10 + _vontade * 2;
        _atkfisico = 2 + (_forca * 2);
        _atkmagico = 2 + (_conhecimento * 2);
        _critRate = 100 -_arma.GetCritrate();

        if (!_arma.GetdmgType())
            _atkfisico = 2 + (_forca * 2) + _arma.GetBonus();
        else
            _atkmagico = 2 + (_conhecimento * 2) + _arma.GetBonus();

        state = TURNSTATE.WAITING;
        
        _renderer = GetComponentInChildren<Renderer>();
    }

    public int RollDice(int _dNumber)
    {
        int rolled;

        rolled = Random.Range(1, _dNumber);

        return rolled;
    }

    public int GetMaxHP()
    { return maxHP;}

    public int GetCurrentHP()
    { return currentHP;}

    public int GetAtkFisico()
    {
        int atk = _atkfisico + RollDice(20);

        return atk;
    }

    public int GetAtkMagico()
    {
        int atk = _atkmagico + RollDice(20);

        return atk;
    }

    public int GetAtk()
    {
        if (_arma.GetdmgType())
        {
            return GetAtkMagico();
        }
        else
        {
            return GetAtkFisico();
        }
    }

    public bool Critic()
    {
        int dice = RollDice(100);
        if (dice > _critRate)
            return true;
        else
            return false;
    }

    public int GetDano()
    {
        if(_arma.GetdmgType())
        {
            return GetDanoMagico();
        }
        else
        {
            return GetDanoFisico();
        }
    }

    public int GetDanoFisico()
    {
        int dano = RollDice(_arma.GetDamage()) + _forca;

        return dano;
    }

    public int GetDanoMagico()
    {
        int dano = RollDice(_arma.GetDamage()) + _conhecimento;

        return dano;
    }

    public int GetDefesa()
    { return _defesa; }

    public int GetDefMental()
    { return _defMente; }

    public bool TakeDamage(int dmg)
    {
         currentHP-=dmg;

        StartCoroutine(FlashRed());

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    IEnumerator FlashRed()
    {
        for(int i = 0; i < 2; i++)
        { 
        _renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.3f);

        _renderer.material.color = Color.white;

        yield return new WaitForSeconds(0.3f);
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void SetTarget(bool b)
    {
        target = b;
    }
    public bool GetTarget()
    {
        return target;
    }
}
