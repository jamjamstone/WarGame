using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BoomSpider : Unit
{
    private float attackRadius = 5;

    public void ChangeState(UnitStateName stateName)
    {
        unitState=stateName;
    }
    
    public void SetDestination(Vector3 willDestination)
    {
        unitDestination = willDestination;
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
            if(detected[0].tag == "Unit")
            {
                UnitAttack();
            }
            



        }
    }
    public void UnitAttack()
    {

    }

    public void UnitMove()
    {

    }
    public void UnitDie()
    {
        
    }

    public void GetHit(float dmg)
    {
        unitInfo.unitHP -= dmg;
        if(unitInfo.unitHP < 0)
        {
            UnitDie();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

}
