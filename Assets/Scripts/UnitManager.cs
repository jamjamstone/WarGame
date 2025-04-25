using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<Unit> myUnits;
    public List<Unit> enemyUnits;



    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.turnManager.OnChangeToBattlePhase += StopUnitMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddMyUnits(Unit unit)
    {
        myUnits.Add(unit);
        unit.canMove = true;
    }
    public void AddEnemyUnit(Unit unit)
    {
        enemyUnits.Add(unit);
    }
    public void StopUnitMove()
    {
        foreach (var unit in myUnits)
        {
            unit.canMove = false;
        }
    }

}
