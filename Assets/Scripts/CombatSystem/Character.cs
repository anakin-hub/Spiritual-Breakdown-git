using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TURNSTATE { WAITING, ACTION, DEAD}

public class Character : MonoBehaviour
{
    public string UnitName;
    public int unitLevel;
    public TURNSTATE state;
    public BattleHUD _hud;

    public int turn;
    protected bool turnStarted;

    [SerializeField] protected Animator _animator;

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

    public SkillDamage _ataque;
    public SkillDamage _skillDano;
    public SkillHeal _skillCura;

    //Equipamentos
    public Weapon _arma;
    public Equip _armadura;
    [SerializeField] protected int _bonusArmadura;
    [SerializeField] protected List<Equip> equipamentos;

    Renderer _renderer;

    protected void Awake()
    {
        turn = 0;
        turnStarted = false;
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

        _hud.SetHUD(this);
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
        _animator.SetTrigger("Attacking");
        if (_arma.GetdmgType())
        {
            return GetAtkMagico();
        }
        else
        {
            return GetAtkFisico();
        }
    }

    public int GetPureAtk()
    {
        _animator.SetTrigger("Attacking");
        if (_arma.GetdmgType())
        {
            return _atkmagico;
        }
        else
        {
            return _atkfisico;
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
        _animator.SetTrigger("Damaged");

        currentHP-=dmg;

        _hud.setHP(currentHP);
        StartCoroutine(FlashRed());

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    IEnumerator FlashRed()
    {
        do
        { 
        _renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.3f);

        _renderer.material.color = Color.white;

        yield return new WaitForSeconds(0.3f);
        }while (!_hud.GetReady());
    }

    IEnumerator FlashGreen()
    {
        do
        {
            _renderer.material.color = Color.green;

            yield return new WaitForSeconds(0.3f);

            _renderer.material.color = Color.white;

            yield return new WaitForSeconds(0.3f);
        } while (!_hud.GetReady());
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        StartCoroutine(FlashGreen());
        _hud.setHP(currentHP);
    }

    public void SetTarget(bool b)
    {
        target = b;
    }
    public bool GetTarget()
    {
        return target;
    }

    public bool Endturn()
    {
        if((_skillCura.GetSkillUsed() || _skillDano.GetSkillUsed() || _ataque.GetSkillUsed()) && _hud.GetReady())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Victory()
    {
        if (_skillCura.victory() || _skillDano.victory() || _ataque.victory())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
