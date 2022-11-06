using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skill : MonoBehaviour
{
    //Definição de alvo
    [SerializeField] private bool target;//faz um atk contra o alvo
    [SerializeField] private bool self;//nao precisa de teste, o alvo é você mesmo

    [SerializeField] private int numDices;//numero de dados a rolar
    [SerializeField] private int dmg;//valor dos dados //caso a skill seja cura, o dano será quanto ira curar

    void setTarget(bool t) { target = t; }
    void setSelf(bool s) { self = s; }

    void setDices(int d)
    {
        numDices = d;
    }
    void setDMG(int d)
    {
        dmg = d;
    }

    bool VerifyAttack(int bonusatk, int Def, TMP_Text text)
    {
        text.text = "Ataque: " + bonusatk + "\n Defesa: " + Def;

        if (bonusatk > Def)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int getDmg()
    { return dmg; }
    int getHeal()
    { return dmg; }
    

}
