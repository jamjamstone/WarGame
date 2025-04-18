using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Create UnitInfo", fileName = "UnitData", order = 1)]
public class UnitInfo :ScriptableObject
{
    public string unitName;
    public float unitHP;
    public float unitATK;
    public float unitSpeed;
    public string unitDescription;
    public int unitNumber;
    public bool isMine;


}
