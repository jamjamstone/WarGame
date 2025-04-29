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
            yield return new WaitForSeconds(0.5f);
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
                    if (canMove)
                    {
                        UnitMove();
                    }
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
           
            if (detected.Length > 0 && detected[0]?.tag == "EnemyUnit")
            {
                
                if (attackDelayTime > unitInfo.unitAttackSpeed)
                {
                    UnitAttack(detected);
                }

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
        unitBody.velocity = transform.forward * unitInfo.unitSpeed;
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("drag");
    //    Vector3 mousePosition = new Vector3(Input.mousePosition.x,transform.position.y,Input.mousePosition.y);
    //    transform.position = mousePosition;
    //}
}
