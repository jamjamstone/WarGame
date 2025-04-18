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

    public delegate void onDead(Unit unit);
    public event onDead OnDead;
    //public event Action OnDead;

    private void Start()
    {
        targetLayerMask = LayerMask.NameToLayer("Unit");

    }
    public void UnitDie()
    {
        
        OnDead.Invoke(this);
        Destroy(gameObject,2f);
    }

    public void GetHit(float dmg)
    {
        unitInfo.unitHP -= dmg;
        if (unitInfo.unitHP < 0)
        {
            unitBody.useGravity = false;
            unitCollider.enabled = false;
            unitInfo.unitHP = 0;
            UnitDie();
        }
    }
}
