using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitStateName
{
    None, Dead, Move, Attack,Idle
}

public interface IUnitState
{
    
    
   
}


public class IUnitStateDead : IUnitState
{
    
}
public class IUnitStateMove : IUnitState
{
    
}
public class IUnitStateAttack : IUnitState
{
    
}
