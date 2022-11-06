using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Update()
    {
        if(state == TURNSTATE.ACTION)
        {
            if (!turnStarted)
            {
                turnStarted = true;
                turn++;
            }
        }
        else if( state == TURNSTATE.WAITING)
        {
            if(turnStarted)
                turnStarted = false;
        }
        
        if(state == TURNSTATE.DEAD && _ameaca > 0)
            _ameaca = 0;
    }

    public int GetMenace()
    {
        int atkchance = -5;

        if( _ameaca > 0)
            atkchance += _ameaca * 10;
        else
            atkchance = _ameaca;

        return atkchance;
    }
}
