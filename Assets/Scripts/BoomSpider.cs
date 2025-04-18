using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BoomSpider : Unit
{
    private float attackRadius = 5;
    public ParticleSystem explosion;
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
            if(detected[0].tag == "Unit")
            {
                UnitAttack(detected);
            }
            



        }
    }
    public void UnitAttack(Collider[] targets)//자폭 공격이라 특별 취급
    {
        for(int i=0;i<targets.Length;i++)
        {
            explosion.Play();
            targets[i].gameObject.GetComponent<Unit>().GetHit(unitInfo.unitATK);
        }
    }

    public void UnitMove()
    {

    }
    
    private void OnTriggerEnter(Collider other)
    {
        
    }

}
