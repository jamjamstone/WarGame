using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitStateName
{
    None, Dead, Move, Attack
}

public interface IUnitState
{
    public void StateAction();
    public void EndState();
    public void StartState();
    
   
}
