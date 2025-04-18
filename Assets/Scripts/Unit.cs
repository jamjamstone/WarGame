using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //public 
    public Animator unitAnimator;
    public UnitInfo unitInfo;
    public Rigidbody unitBody;
    public Collider unitCollider;
    public Vector3 unitDestination;
    public UnitStateName unitState;
    public LayerMask targetLayerMask;

    //public delegate void OnDead();
    public event EventHandler OnDead;

    private void Start()
    {
        targetLayerMask = LayerMask.NameToLayer("Unit");
    }
    public void UnitDie()
    {

    }

    public void GetHit(float dmg)
    {
        unitInfo.unitHP -= dmg;
        if (unitInfo.unitHP < 0)
        {
            UnitDie();
        }
    }
}
