using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> myUnits;
    public List<GameObject> enemyUnits;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddMyUnits(Unit unit)
    {
        myUnits.Add(unit.gameObject);
    }
    public void AddEnemyUnit(Unit unit)
    {
        enemyUnits.Add(unit.gameObject);
    }


}
