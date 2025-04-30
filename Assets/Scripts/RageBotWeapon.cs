using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBotWeapon : MonoBehaviour
{
    [SerializeField] UnitInfo rageBotInfo;
    [SerializeField] Unit ragebot;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit"&&other.GetComponent<Unit>().ownPlayerNumber!=ragebot.ownPlayerNumber)
        {
            other.GetComponent<Unit>().GetHit(rageBotInfo.unitATK);
        }
    }
}
