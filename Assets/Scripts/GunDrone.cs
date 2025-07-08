using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunDrone : Unit,IDragHandler, IPointerDownHandler
{
    private float attackRadius=7;
    [SerializeField] ParticleSystem[] muzzleFlashs;
    private float attackDelayTime = 0;
    //Coroutine stateActionCo;
    //Coroutine detectCo;
    // Start is called before the first frame update
    private void Start()
    {
        //targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        //UnitInit();
        cam = Camera.main;
        GameManager.Instance.unitManager.AddMyUnits(this);
        //GameManager.Instance.turnManager.OnChangeToBattlePhase += UnitActivate;
        //GameManager.Instance.turnManager.OnChangeToBuyPhase += UnitDeactivate;

        //ChangeState(UnitStateName.Move);
        // UnitActivate();
    }
    private void FixedUpdate()
    {
        //if(unitState == UnitStateName.Attack)
        //{
        //    attackDelayTime += Time.fixedDeltaTime;
        //}
        //else
        //{
        //    attackDelayTime = 0;
        //}
        attackDelayTime += Time.fixedDeltaTime;
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

        stateActionCo = StartCoroutine(StateAction());
        detectCo = StartCoroutine(DetectEnemy());

        //StartCoroutine(StateAction());
        //StartCoroutine(DetectEnemy());
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
    }
    public override void UnitDeactivate()
    {
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Idle);
        Debug.Log("gun Unit deactivated");
        StopAllCoroutines();
        StopCoroutine(detectCo);
        StopCoroutine(stateActionCo);
        //photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Idle);
    }
    

    IEnumerator StateAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            switch (unitState)
            {
                case UnitStateName.Attack:
                    //unitAnimator.SetBool(StaticField.hashIdle, false);
                    //unitAnimator.SetBool(StaticField.hashAttack, true);
                    //unitAnimator.SetBool(StaticField.hashMove, false);
                    break;
                case UnitStateName.Move:
                    //unitAnimator.SetBool(StaticField.hashIdle, false);
                    //unitAnimator.SetBool(StaticField.hashAttack, false);
                    //unitAnimator.SetBool(StaticField.hashMove, true);
                    if (targetCollider == null)
                    {
                        if (PhotonNetwork.MasterClient.ActorNumber==ownPlayerNumber)//호스트의 공격 유닛
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
                    //unitAnimator.SetBool(StaticField.hashIdle, false);
                    //unitAnimator.SetBool(StaticField.hashAttack, false);
                    //unitAnimator.SetBool(StaticField.hashMove, false);
                    //unitAnimator.SetBool(StaticField.hashDead, true);
                    break;
                case UnitStateName.Idle:
                    //unitAnimator.SetBool(StaticField.hashIdle, true);
                    //unitAnimator.SetBool(StaticField.hashAttack, false);
                    //unitAnimator.SetBool(StaticField.hashMove, false);
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
            var detected = Physics.OverlapSphere(transform.position, attackRadius, targetLayerMask);
            //Debug.Log("detectingG");
            targetCollider = null;
            minDist = float.MaxValue;
            foreach (var d in detected)
            {
                if (d.gameObject == this.gameObject)
                {
                    continue;
                }
                float dist = Vector3.Distance(transform.position, d.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    targetCollider = d;
                }
            }




            Debug.Log(attackDelayTime);
            if (targetCollider != null && attackDelayTime > unitInfo.unitAttackSpeed && targetCollider.GetComponent<Unit>().ownPlayerNumber != ownPlayerNumber)
            {
                //Debug.Log("enemydetected");
                Debug.Log("건드론 공격");
                UnitAttack(targetCollider);
            }
            else
            {
                photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
            }




        }
    }
    public void UnitAttack(Collider target)//
    {
        attackDelayTime = 0;
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
        transform.LookAt(target.gameObject.transform);
        photonView.RPC("ShowMuzzleFlash", RpcTarget.All);
        //Debug.Log(targets.Length);
        target.gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
        
    }
    [PunRPC]
    public void ShowMuzzleFlash()
    {
        foreach (var muzzleFlash in muzzleFlashs)
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
        }
    }


    //public void UnitMove()
    //{
    //   
    //    //Vector3 move = transform.forward; // XZ 방향 이동
    //    //
    //    //unitBody.MovePosition(transform.position + move * unitInfo.unitSpeed * Time.deltaTime);
    //    if(canMove == false)
    //    {
    //        return;
    //    }
    //    unitBody.velocity = transform.forward * unitInfo.unitSpeed * StaticField.speedModifieValue;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("drag");
    //    Vector3 mousePosition = new Vector3(Input.mousePosition.x,transform.position.y,Input.mousePosition.y);
    //    transform.position = mousePosition;
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    
    //    var temp = other.GetComponent<PlayerUnit>();
    //    if (other.tag == "Player" && temp.ownPlayerNumber != ownPlayerNumber)
    //    {
    //        gameObject.SetActive(false);
    //        temp.PlayerGetDemage(unitInfo.unitATK+ unitInfo.unitHP);
    //    }
    //}
}
