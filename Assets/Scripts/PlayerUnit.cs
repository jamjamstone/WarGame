using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerUnit : MonoBehaviourPun
{
    public int ownPlayerNumber;


    // Start is called before the first frame update
    

    public void PlayerGetDemage(float dmg)
    {
        if(PhotonNetwork.IsMasterClient&&ownPlayerNumber==photonView.Owner.ActorNumber)
        {
            GameManager.Instance.playerManager.HostGetDemage(dmg);
        }
        else if(PhotonNetwork.IsMasterClient==false&&ownPlayerNumber==photonView.Owner.ActorNumber)
        {
            GameManager.Instance.playerManager.GuestGetDemage(dmg);
        }
    }

    private void OnEnable()
    {
        ownPlayerNumber=photonView.Owner.ActorNumber;
    }



}
