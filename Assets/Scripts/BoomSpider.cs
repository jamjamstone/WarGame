using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class BoomSpider : Unit,IDragHandler
{
    private float attackRadius = 5;
    public ParticleSystem explosion;
    private void Start()
    {
        //UnitInit();
        GameManager.Instance.unitManager.AddMyUnits(this);
    }
    public void ChangeState(UnitStateName stateName)
    {
        unitState=stateName;
    }
    
    public void SetDestination(Vector3 willDestination)
    {
        unitDestination = willDestination;
    }
    public void UnitInit()
    {
        if (photonView.IsMine)
        {
            unitInfo.isMine = true;
        }
        else
        {
            unitInfo.isMine = false;
        }
        StartCoroutine(StateAction());
        StartCoroutine(DetectEnemy());
    }
    
    IEnumerator StateAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            switch (unitState)
            {
                case UnitStateName.Attack:
                    unitAnimator.SetBool(StaticField.hashIdle, false);
                    unitAnimator.SetBool(StaticField.hashAttack, true);
                    unitAnimator.SetBool(StaticField.hashMove, false);
                    break;
                case UnitStateName.Move:
                    unitAnimator.SetBool(StaticField.hashIdle, false);
                    unitAnimator.SetBool(StaticField.hashAttack, false);
                    unitAnimator.SetBool(StaticField.hashMove, true);
                    UnitMove();
                    break;

                case UnitStateName.Dead:
                    unitAnimator.SetBool(StaticField.hashIdle, false);
                    unitAnimator.SetBool(StaticField.hashAttack, false);
                    unitAnimator.SetBool(StaticField.hashMove, false);
                    unitAnimator.SetBool(StaticField.hashDead, true);
                    break;
                case UnitStateName.Idle:
                    unitAnimator.SetBool(StaticField.hashIdle, true);
                    unitAnimator.SetBool(StaticField.hashAttack, false);
                    unitAnimator.SetBool(StaticField.hashMove, false);
                    break;
                default:
                    break;
            }




        }
    }

    IEnumerator DetectEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            var detected = Physics.OverlapSphere(transform.position, attackRadius,targetLayerMask);

            if(detected.Length>1&&detected[1]?.tag == "Unit")
            {
                //Debug.Log("enemydetected");
                UnitAttack(detected);
            }
            



        }
    }
    public void UnitAttack(Collider[] targets)//자폭 공격이라 특별 취급
    {
        ChangeState(UnitStateName.Attack);
        Debug.Log(targets.Length);
        for(int i=0;i<targets.Length;i++)
        {   
            explosion.Play();
            targets[i].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
        }
    }

    public void UnitMove()
    {
        unitBody.velocity = transform.forward * unitInfo.unitSpeed;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x,transform.position.y,Input.mousePosition.z);
        transform.position = mousePosition;
    }
}
