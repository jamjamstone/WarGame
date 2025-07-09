using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBotWeapon : MonoBehaviour
{
    [SerializeField] UnitInfo rageBotInfo;
    [SerializeField] Unit ragebot;
    private float timer = 0;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        timer += Time.deltaTime;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit")&&other.GetComponent<Unit>().ownPlayerNumber!=ragebot.ownPlayerNumber)
        {
            if(timer>rageBotInfo.unitAttackSpeed)
            {
                other.GetComponent<Unit>().GetHit(rageBotInfo.unitATK);
                Debug.Log("rage hit");
                timer = 0;
            }
            
        }
    }
}
