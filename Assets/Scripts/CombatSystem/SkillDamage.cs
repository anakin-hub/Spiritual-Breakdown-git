using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillDamage : Skill
{
    public void UseSkill(Character caster, Character target, TMP_Text dialogue)
    {
        StartCoroutine(PlayerAttack(caster, target, dialogue));
    }

    public void UseSkill(int currentturn, Character caster, Character target, TMP_Text dialogue)
    {
        base.UseSkill(currentturn);
        StartCoroutine(PlayerAttack(caster, target, dialogue));
    }

    bool VerifyAttack(int bonusatk, int Def, TMP_Text dialogue)
    {
        dialogue.text = "Dado rolado: " + _rolledDice + "\nAtaque: " + bonusatk + "\tDefesa: " + Def;
        
        if (bonusatk > Def)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator PlayerAttack(Character caster, Character target, TMP_Text dialogue)
    {
        if (caster.state != TURNSTATE.DEAD)
        {
            _bonusatk = caster.GetPureAtk();
            bool success = VerifyAttack(RollAtk(), target.GetDefesa(), dialogue);

            yield return new WaitForSeconds(2.5f);
            if (success)
            {
                int dmg;
                if (_damageDice == 0)
                    dmg = caster.GetDano();
                else
                    dmg = RollDMG();

                if (_hasCritic && _rolledDice >= 19)
                {
                    dialogue.text = "Acerto crítico!\n";

                    if (_damageDice == 0)
                        dmg += caster.GetDano();
                    else
                        dmg += RollDMG();
                }
                else
                {
                    dialogue.text = "O ataque foi um sucesso!\n";
                }

                yield return new WaitForSeconds(2f);

                _isDead = target.TakeDamage(dmg);

                dialogue.text += "Dano infligido: " + dmg;
            }
            else
            {
                dialogue.text = "O ataque falhou!";
            }

            yield return new WaitForSeconds(2f);
        }

        _skillUsed = true;
    }

    int RollAtk()
    {
        _rolledDice = RollDice(20);

        return _rolledDice + _bonusatk;
    }

    int RollDMG()
    {
        int dmg = 0;

        for(int i = 0; i < _numDices; i++)
        {
            dmg += RollDice(_damageDice);
        }

        return dmg;
    }
}
