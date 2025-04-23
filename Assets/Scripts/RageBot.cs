using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

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
        UnitActivate();
    }
    public void ChangeState(UnitStateName stateName)
    {
        unitState = stateName;
    }

    public void SetDestination(Vector3 willDestination)
    {
        unitDestination = willDestination;
    }
    public void UnitActivate()
    {
        UnitInit();
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
                    //unitAnimator.SetBool(StaticField.hashIdle, false);
                    //unitAnimator.SetBool(StaticField.hashAttack, false);
                    //unitAnimator.SetBool(StaticField.hashMove, false);
                    //unitAnimator.SetBool(StaticField.hashDead, true);
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
            yield return new WaitForSeconds(0.1f);
            var detected = Physics.OverlapSphere(transform.position, attackRadius, targetLayerMask);

            if (detected.Length > 0 && detected[0]?.tag == "EnemyUnit")
            {
                //Debug.Log("enemydetected");
                UnitAttack(detected);
            }
            else
            {
                ChangeState(UnitStateName.Move);
            }




        }
    }
    public void UnitAttack(Collider[] targets)//
    {
        Vector3 direction = targets[0].transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1* Time.deltaTime);
        ChangeState(UnitStateName.Attack);
        flame1.SetActive(true);
        flame2.SetActive(true);
        //Debug.Log(targets.Length);
        //targets[0].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
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
