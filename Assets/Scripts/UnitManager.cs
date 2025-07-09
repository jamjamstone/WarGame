using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<Unit> myUnits=new List<Unit>();
    //public List<Unit> enemyUnits= new List<Unit>();



    // Start is called before the first frame update
    void Start()
    {
        
        GameManager.Instance.turnManager.OnChangeToBuyPhase += ResetUnitPosition;
        GameManager.Instance.turnManager.OnChangeToBuyPhase += DeactivateUnitBattle;
        //GameManager.Instance.turnManager.OnChangeToBuyPhase += ReActiveUnitsModel;

        GameManager.Instance.turnManager.OnChangeToBuyPhase += UnitMoveActivate;


        GameManager.Instance.turnManager.OnChangeToBattlePhase+=UnitBattleActivate;
        GameManager.Instance.turnManager.OnChangeToBattlePhase+= StopUnitMove;
    }

    
    
    public void AddMyUnits(Unit unit)
    {
        myUnits.Add(unit);
        unit.SetCanMove();
        //unit.SetDontMove();
    }
    //public void AddEnemyUnit(Unit unit)//현재 사용x
    //{
    //    enemyUnits.Add(unit);
    //}
    public void StopUnitMove()// 드래그 이동 금지
    {
        foreach (var unit in myUnits)
        {
            unit.SetDontMove();
        }
    }
    public void UnitMoveActivate()//드래그 이동 허락
    {
        //Debug.Log("unit move activate");
        foreach (var unit in myUnits)
        {
            unit.SetCanMove();// = true;
            
        }
    }
    public void UnitBattleActivate()
    {
        foreach (var unit in myUnits)
        {
            unit.UnitActivate();// = true;

        }
       
    }

    public void ResetUnitPosition()
    {
        foreach (var unit in myUnits)
        {
            unit.gameObject.transform.position=unit.initialPosition;
            unit.gameObject.transform.rotation= unit.initialRotation;
        }
    }
    
    public void ReActiveUnitsModel()
    {
        foreach (var unit in myUnits)
        {
            unit.gameObject.SetActive(true);
        }
    }

    //public void DeactivateUnitMove()
    //{
    //    foreach (var unit in myUnits)
    //    {
    //        unit.SetDontMove();
    //        
    //        
    //    }
    //}
    public void DeactivateUnitBattle()
    {
        foreach (var unit in myUnits)
        {
            unit.UnitDeactivate();


        }
        ReActiveUnitsModel();
        Debug.Log("unit battle deactivate");
    }
}
