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
    //[SerializeField] ParticleSystem[] muzzleFlashs;

    // Start is called before the first frame update
    private void Start()
    {
        //targetLayerMask = LayerMask.NameToLayer("EnemyUnit");
        //UnitInit();
        cam = Camera.main;
        GameManager.Instance.unitManager.AddMyUnits(this);
        //ChangeState(UnitStateName.Move);
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
            var detected = Physics.OverlapSphere(transform.position, attackRadius, targetLayerMask);

            if (detected.Length > 1 && detected[1]?.tag == "EnemyUnit")
            {
                //Debug.Log("enemydetected");
                UnitAttack(detected);
            }




        }
    }
    public void UnitAttack(Collider[] targets)//
    {
        ChangeState(UnitStateName.Attack);
        
        //Debug.Log(targets.Length);
        targets[0].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
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
