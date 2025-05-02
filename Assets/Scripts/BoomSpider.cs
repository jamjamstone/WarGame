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
            yield return new WaitForSeconds(0.05f);
            var detected = Physics.OverlapSphere(transform.position, attackRadius,targetLayerMask);
            Debug.Log("detectingB");
            List<Unit> enemiesInRange = new List<Unit>();
            targetCollider = null;
            minDist = float.MaxValue;
            foreach (var d in detected)
            {
                if (d.gameObject == this.gameObject)
                {
                    continue;
                }
                Unit unit = d.GetComponent<Unit>();
                if (unit != null && unit.ownPlayerNumber != ownPlayerNumber)
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


    private void OnTriggerEnter(Collider other)
    {
        var temp = other.GetComponent<PlayerUnit>();
        if (other.tag == "Player"&&temp.ownPlayerNumber!=ownPlayerNumber)
        {
            gameObject.SetActive(false);
            temp.PlayerGetDemage(unitInfo.unitATK+ unitInfo.unitHP);
            
        }
    }


}
