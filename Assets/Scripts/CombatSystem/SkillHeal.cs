using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillHeal : Skill
{
    public void UseSkill(int currentturn, Character caster, TMP_Text dialogue)
    {
        base.UseSkill(currentturn);
        StartCoroutine(Healing(caster, dialogue));
    }

    IEnumerator Healing(Character caster, TMP_Text dialogue)
    {
        for(int i = 0; i < _numDices; i++)
            caster.Heal(_damageDice);

        dialogue.text = "Vitalidade recuperada";

        yield return new WaitForSeconds(2f);

        _skillUsed = true;
    }
}
