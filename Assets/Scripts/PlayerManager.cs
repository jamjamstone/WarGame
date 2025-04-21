using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int money=StaticField.startMoney;



    public void ResetPlayerManager()
    {
        ResetMoney();
    }
    public void ResetMoney()
    {
        money=StaticField.startMoney;
    }
}
