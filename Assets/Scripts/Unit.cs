using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
public class Unit : MonoBehaviourPun
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

    private float distance;
    private Vector3 offset;
    protected Camera cam;
    //public event Action OnDead;

    private void Start()
    {
        targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        
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
    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            distance = Vector3.Distance(cam.transform.position, hit.point);
            offset = transform.position - hit.point;
        }
    }
    public void UnitInit()
    {
        if (unitInfo.isMine == true)
        {
            gameObject.tag = "MyUnit";
        }
        else
        {
            gameObject.tag = "EnemyUnit";
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(eventData.position);
        Vector3 newPos = ray.GetPoint(distance);
        transform.position = newPos + offset;
        transform.position = new Vector3(transform.position.x,0.1f,transform.position.z);
        unitBody.velocity = Vector3.zero;
    }
}
