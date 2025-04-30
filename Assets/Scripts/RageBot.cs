using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class RageBot : Unit,IDragHandler, IPointerDownHandler
{
    // Start is called before the first frame update
    private float attackRadius = 7;
    [SerializeField] GameObject flame1;
    [SerializeField] GameObject flame2;
    //[SerializeField] ParticleSystem[] muzzleFlashs;

    // Start is called before the first frame update
    private void Start()
    {
        //targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        //UnitInit();
        cam = Camera.main;
        GameManager.Instance.unitManager.AddMyUnits(this);
        //ChangeState(UnitStateName.Move);
        GameManager.Instance.turnManager.OnChangeToBattlePhase += UnitActivate;
        GameManager.Instance.turnManager.OnChangeToBuyPhase += UnitDeactivate;


    }
    private void Awake()
    {
        UnitInit();
        DeactivateWeapon();
    }

    public void SetDestination(Vector3 willDestination)
    {
        unitDestination = willDestination;
    }
    public void UnitActivate()
    {
        
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
            yield return new WaitForSeconds(0.1f);
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



            
            if (targetCollider != null)
            {
                //Debug.Log("enemydetected");

                UnitAttack(detected);
            }
            else
            {
                photonView.RPC("DeactivateWeapon", RpcTarget.All);
                photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Move);
            }



        }
    }
    public void UnitAttack(Collider[] targets)//
    {
        Vector3 direction = targets[0].transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1* Time.deltaTime);
        photonView.RPC("ChangeState", RpcTarget.All, UnitStateName.Attack);
        photonView.RPC("ActivateWeapon", RpcTarget.All);
        //Debug.Log(targets.Length);
        //targets[0].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
    }
    [PunRPC]
    public void ActivateWeapon()
    {
        flame1.SetActive(true);
        flame2.SetActive(true);
    }

    [PunRPC]
    public void DeactivateWeapon()
    {
        flame1.SetActive(false);
        flame2.SetActive(false);
    }



    public void UnitMove()
    {
        Vector3 move = transform.forward; // XZ 방향 이동
        
        unitBody.MovePosition(transform.position + move * unitInfo.unitSpeed * Time.deltaTime);
        //unitBody.velocity = transform.forward * unitInfo.unitSpeed;
        Debug.Log("ragemove");
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("drag");
    //    Vector3 mousePosition = new Vector3(Input.mousePosition.x,transform.position.y,Input.mousePosition.y);
    //    transform.position = mousePosition;
    //}
}
