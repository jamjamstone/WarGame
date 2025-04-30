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
        GameManager.Instance.turnManager.OnChangeToBuyPhase += UnitDeactivate;
        
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
    public override void UnitDeactivate()
    {
        StopAllCoroutines();
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.None);
    }
    IEnumerator StateAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
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
            Debug.Log("detectingB");
            List<Unit> enemiesInRange = new List<Unit>();
            foreach (var d in detected)
            {
                Unit unit = d.GetComponent<Unit>();
                if (unit != null && unit.ownPlayerNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    enemiesInRange.Add(unit);
                }
            }

            if (enemiesInRange.Count > 0)
            {
                UnitAttack(enemiesInRange); // 자폭 공격
            }
            else
            {
                photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
            }




        }
    }
    public void UnitAttack(List<Unit> targets)//자폭 공격이라 특별 취급
    {
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
        //Debug.Log(targets.Length);
        //for(int i=0;i<targets.Length;i++)
        //{
        //    photonView.RPC("ShowExplosion", RpcTarget.All);
        //    targets[i].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
        //}
        foreach (Unit enemy in targets)
        {
            photonView.RPC("ShowExplosion", RpcTarget.All);
            enemy.GetHit(unitInfo.unitATK);
        }
        gameObject.SetActive(false);

    }
    [PunRPC]
    public void ShowExplosion()
    {
        explosion.Play();
    }
    public void UnitMove()
    {
        
        //Vector3 move = transform.forward; // XZ 방향 이동
        //
        //unitBody.MovePosition(transform.position + move * unitInfo.unitSpeed * Time.deltaTime);
        unitBody.velocity = transform.forward * unitInfo.unitSpeed*StaticField.speedModifieValue;
    }
    
    

    

    }
