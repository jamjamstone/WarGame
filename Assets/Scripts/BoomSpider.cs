using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class BoomSpider : Unit,IDragHandler, IPointerDownHandler
{
    private float attackRadius = 5;


    public GameObject selfRenderer;
    public GameObject explosion;
    private void Start()
    {
        //targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        //UnitInit();
        cam = Camera.main;
        GameManager.Instance.unitManager.AddMyUnits(this);
        //GameManager.Instance.turnManager.OnChangeToBattlePhase += UnitActivate;
        //GameManager.Instance.turnManager.OnChangeToBuyPhase += UnitDeactivate;
        
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
    
    public override void UnitActivate()
    {
        // Debug.Log("act");
        gameObject.SetActive(true);
        stateActionCo= StartCoroutine(StateAction());
        detectCo = StartCoroutine(DetectEnemy());
        photonView.RPC("ChangeState",RpcTarget.All,UnitStateName.Move);
        selfRenderer.SetActive(true);
    }
    public override void UnitDeactivate()
    {
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Idle);
        StopAllCoroutines();
        selfRenderer.SetActive(true);
        unitBody.velocity = Vector3.zero;
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
        StopCoroutine(detectCo);
        StopCoroutine(stateActionCo);
        //Debug.Log("spider Unit deactivated");
        
    }
    IEnumerator StateAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            switch (unitState)
            {
                case UnitStateName.Attack:
                    unitBody.velocity = Vector3.zero;
                    transform.LookAt(targetCollider.transform);
                    unitAnimator.SetBool(StaticField.hashIdle, false);
                    unitAnimator.SetBool(StaticField.hashAttack, true);
                    unitAnimator.SetBool(StaticField.hashMove, false);
                    break;
                case UnitStateName.Move:
                    unitAnimator.SetBool(StaticField.hashIdle, false);
                    unitAnimator.SetBool(StaticField.hashAttack, false);
                    unitAnimator.SetBool(StaticField.hashMove, true);
                    if (targetCollider == null)
                    {
                        if (PhotonNetwork.MasterClient.ActorNumber == ownPlayerNumber)
                        {
                            transform.rotation = Quaternion.LookRotation(Vector3.forward);
                        }
                        else
                        {
                            transform.rotation = Quaternion.LookRotation(Vector3.back);

                        }

                    }
                    UnitMove();
                    
                    break;

                case UnitStateName.Dead:
                    unitAnimator.SetBool(StaticField.hashIdle, false);
                    unitAnimator.SetBool(StaticField.hashAttack, false);
                    unitAnimator.SetBool(StaticField.hashMove, false);
                    unitAnimator.SetBool(StaticField.hashDead, true);
                    break;
                case UnitStateName.Idle:
                    unitBody.velocity= Vector3.zero;
                    gameObject.transform.position = initialPosition;
                    gameObject.transform.rotation = initialRotation;
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
            //Debug.Log("detectingB");
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
                photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
                StopCoroutine(detectCo);
            }
            else
            {
                photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
            }




        }
    }
    public void UnitAttack(List<Unit> targets)//자폭 공격이라 특별 취급
    {
        unitBody.velocity = Vector3.zero;

        //StopCoroutine(detectCo);
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
        photonView.RPC("ShowExplosion", RpcTarget.All);
        targetCollider = targets[0].GetComponent<Collider>();
        // 폭발 효과를 위해 잠시 대기
        foreach (Unit enemy in targets)
        {
            
            enemy.GetHit(unitInfo.unitATK);
        }
        StartCoroutine(DelayCoroutine(0.5f));
       
        

    }

    
    IEnumerator DelayCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);

    }
    IEnumerator ExplosionCoroutine()
    {
        var temp = PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
        if (temp == null)
        {
            Debug.Log("생성 안됨");
            yield break;
        }
        else
        {
            Debug.Log("생성");
        }
            var tempParticle = temp.GetComponent<ParticleSystem>();
        tempParticle.Play();
        while (tempParticle.IsAlive())
        {
            yield return null;
        }
        PhotonNetwork.Destroy(temp);
        
    }

    [PunRPC]
    public void ShowExplosion()
    {
        selfRenderer.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ExplosionCoroutine());
        }
        else
        {
            return;
        }
        //gameObject.SetActive(false);
        //explosion.gameObject.transform.SetParent(null); 
    }
    


}
