using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBotWeapon : MonoBehaviour
{
    [SerializeField] UnitInfo rageBotInfo;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyUnit")
        {
            other.GetComponent<Unit>().GetHit(rageBotInfo.unitATK);
        }
    }
}
