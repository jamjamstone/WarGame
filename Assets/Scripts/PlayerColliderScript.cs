
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerColliderScript : MonoBehaviourPun
{
    public int ownPlayerNumber;
    private void Start()
    {
        ownPlayerNumber = photonView.Owner.ActorNumber;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        var tempCollider = other.gameObject.GetComponent<Collider>();
        tempCollider.enabled = false;
        if (other.gameObject.layer==LayerMask.NameToLayer("Unit"))
        {
            Debug.Log("layer detected");
            var unit = other.GetComponent<Unit>();
            if (unit.ownPlayerNumber==ownPlayerNumber)
            {
                tempCollider.enabled = true;
                Debug.Log("동일한 플레이어의 유닛");
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (unit.ownPlayerNumber == PhotonNetwork.MasterClient.ActorNumber)
                    {
                        Debug.Log(unit.unitInfo.unitATK);
                        GameManager.Instance.playerManager.GuestGetDemage(unit.unitInfo.unitATK);
                        Debug.Log(GameManager.Instance.playerManager.guestHP);

                    }
                    else
                    {
                        Debug.Log(unit.unitInfo.unitATK);
                        GameManager.Instance.playerManager.HostGetDemage(unit.unitInfo.unitATK);
                        Debug.Log(GameManager.Instance.playerManager.hostHP);
                    }
                }
                tempCollider.enabled = true;
                unit.UnitDie();

                

            }
            
          
        }
    }

    





}
