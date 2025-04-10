using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Scriptable Object/StrategeCardInfo", order = int.MaxValue)]
public class StrategeCardInfo : ScriptableObject
{
    public string cardName;
    public string cardDescription;
    public int cardCost;
    public RawImage cardIcon;
}
