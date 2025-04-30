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

    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public bool canMove=true;

    public Collider targetCollider;
    public float minDist = Mathf.Infinity;

    public delegate void onDead(Unit unit);
    public event onDead OnDead;

    private float distance;
    private Vector3 offset;
    protected Camera cam;

    public int ownPlayerNumber;

    //public event Action OnDead;

    public void SetDontMove()
    {
        canMove = false;
    }
    public void UnitDie()
    {
        Debug.Log("dead");
        OnDead?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void GetHit(float dmg)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("GetHitRPC", RpcTarget.All, dmg);
        }
        else
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
    [PunRPC]
    public void ChangeState(UnitStateName stateName)
    {
        //Debug.Log("changeto"+stateName);
        unitState = stateName;
    }
    [PunRPC]
    public void GetHitRPC(float dmg)
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

    [PunRPC]
    public void SaveInitialPosition()
    {
        initialPosition=transform.position;
        initialRotation=transform.rotation;
    }


    [PunRPC]
    public void RPCSaveInitialPosition()
    {
        photonView.RPC("SaveInitialPosition", RpcTarget.All);
    }
    public void UnitInit()
    {
        if (PhotonNetwork.IsConnected==false)
        {
            
        }
        else
        {


            gameObject.layer = LayerMask.NameToLayer("Unit");
            ownPlayerNumber = photonView.Owner.ActorNumber;
            
        }
        targetLayerMask = LayerMask.NameToLayer("Unit");
        GameManager.Instance.turnManager.OnChangeToBattlePhase += SetDontMove;
        RPCSaveInitialPosition();



    }

    public virtual void UnitDeactivate()
    {

    }


    public void OnDrag(PointerEventData eventData)
    {
        if (canMove == true)
        {
            Ray ray = cam.ScreenPointToRay(eventData.position);
            Vector3 newPos = ray.GetPoint(distance);
            transform.position = newPos + offset;
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
            unitBody.velocity = Vector3.zero;
            RPCSaveInitialPosition();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (canMove == true)
        {
            Ray ray = cam.ScreenPointToRay(eventData.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                distance = Vector3.Distance(cam.transform.position, hit.point);
                offset = transform.position - hit.point;
            }
        }
    }
}
