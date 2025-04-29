using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class BoomSpider : Unit,IDragHandler, IPointerDownHandler
{
    private float attackRadius = 5;
    
    

    public ParticleSystem explosion;
    private void Start()
    {
        //targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        //UnitInit();
        cam = Camera.main;
        GameManager.Instance.unitManager.AddMyUnits(this);
        GameManager.Instance.turnManager.OnChangeToBattlePhase += UnitActivate;
        
        //ChangeState(UnitStateName.Move);
    }
    private void Awake()
    {
        UnitInit();
    }

    public void SetDestination(Vector3 willDestination)
    {
        unitDestination = willDestination;
    }
    
    public void UnitActivate()
    {
        Debug.Log("act");
        
        StartCoroutine(StateAction());
        StartCoroutine(DetectEnemy());
        photonView.RPC("ChangeState",RpcTarget.All,UnitStateName.Move);
    }
    public void UnitDeactivate()
    {
        StopAllCoroutines();
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.None);
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
                    if (canMove)
                    {
                        UnitMove();
                    }
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

            if(detected.Length>1&&detected[1]?.tag == "EnemyUnit")
            {
                //Debug.Log("enemydetected");
                
                UnitAttack(detected);
            }
            else
            {
                photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
            }




        }
    }
    public void UnitAttack(Collider[] targets)//자폭 공격이라 특별 취급
    {
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
        //Debug.Log(targets.Length);
        for(int i=0;i<targets.Length;i++)
        {
            photonView.RPC("ShowExplosion", RpcTarget.All);
            targets[i].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
        }
    }
    [PunRPC]
    public void ShowExplosion()
    {
        explosion.Play();
    }
    public void UnitMove()
    {
        unitBody.velocity = transform.forward * unitInfo.unitSpeed;
    }
    
    

    

    }
