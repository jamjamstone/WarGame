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
    // Start is called before the first frame update
    private void Start()
    {
        //targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        //UnitInit();
        cam = Camera.main;
        GameManager.Instance.unitManager.AddMyUnits(this);
        GameManager.Instance.turnManager.OnChangeToBattlePhase += UnitActivate;
        GameManager.Instance.turnManager.OnChangeToBuyPhase += UnitDeactivate;

        //ChangeState(UnitStateName.Move);
        // UnitActivate();
    }
    private void FixedUpdate()
    {
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
    public void UnitActivate()
    {
        Debug.Log("act");
        
        
        StartCoroutine(StateAction());
        StartCoroutine(DetectEnemy());
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
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
            yield return new WaitForSeconds(1);
            var detected = Physics.OverlapSphere(transform.position, attackRadius, targetLayerMask);

            foreach (var d in detected)
            {
                float dist = Vector3.Distance(transform.position, d.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    targetCollider = d;
                }
            }





            if (targetCollider != null && attackDelayTime > unitInfo.unitAttackSpeed)
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
    public void UnitAttack(Collider[] targets)//
    {
        attackDelayTime = 0;
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
        transform.LookAt(targets[0].gameObject.transform);
        photonView.RPC("ShowMuzzleFlash", RpcTarget.All);
        //Debug.Log(targets.Length);
        targets[0].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
        
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


    public void UnitMove()
    {
       
        //Vector3 move = transform.forward; // XZ 방향 이동
        //
        //unitBody.MovePosition(transform.position + move * unitInfo.unitSpeed * Time.deltaTime);
        unitBody.velocity = transform.forward * unitInfo.unitSpeed * StaticField.speedModifieValue;
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("drag");
    //    Vector3 mousePosition = new Vector3(Input.mousePosition.x,transform.position.y,Input.mousePosition.y);
    //    transform.position = mousePosition;
    //}
}
