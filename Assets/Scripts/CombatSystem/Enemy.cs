using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private void Update()
    {
        if (state == TURNSTATE.ACTION)
        {
            if (!turnStarted)
            {
                turnStarted = true;
                turn++;
            }
        }
        else if (state == TURNSTATE.WAITING)
        {
            if (turnStarted)
                turnStarted = false;
        }
    }

    public int DecideMenace(int p1, int p2, int p3, int p4)//valor das ameaças (em escala de 100) de cada personagem do jogador
    {
        int choosing, max, choose = 0;
        max = p1 + p2 + p3 + p4;
        choosing = RollDice(max);
        p2 += p1;
        p3 += p2;
        p4 += p3;
        
        if (choosing <= p1)
        {
            choose = 0;
        }
        else if (choosing > p1 && choosing <= p2)
        {
            choose = 1;
        }
        else if (choosing > p2 && choosing <= p3)
        {
            choose = 2;
        }
        else if (choosing > p3 && choosing <= p4)
        {
            choose = 3;
        }



        return choose;
    }
    

    private void OnMouseDown()
    {
        target = true;
    }
}
