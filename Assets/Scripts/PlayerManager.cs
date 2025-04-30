using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPun
{
    public int money=StaticField.startMoney;
    public float hostHP;
    public float guestHP;

    public delegate void PlayerHPChanged(float hp);
    public event PlayerHPChanged OnHostHPChanged;
    public event PlayerHPChanged OnGuestHPChanged;

    public Transform hostPlayerpos;
    public Transform guestPlayerpos;

    public void ResetPlayerManager()
    {
        GameManager.Instance.turnManager.OnChangeToBuyPhase += ()=>AddMoney(500);//매턴 500추가
        hostHP = StaticField.maxPlayerHp;
        guestHP = StaticField.maxPlayerHp;
        ResetMoney();
    }
    public void ResetMoney()
    {
        money=StaticField.startMoney;
    }
    public void SpawnPlayerCollider()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var tempHost = PhotonNetwork.Instantiate(GameManager.Instance.playerColliderPrefab.name, hostPlayerpos.position, Quaternion.identity);
            //tempHost.GetComponent<PlayerUnit>().ownPlayerNumber = photonView.Owner.ActorNumber;
        }
        else
        {
            var tempGuest=PhotonNetwork.Instantiate(GameManager.Instance.playerColliderPrefab.name, guestPlayerpos.position, Quaternion.identity);
            
        }
        
    }
    public void SetPlayerPosition()//카메라 컨트롤에서 따로 지정함 만들 필요 없다는 걸 알기 위해서 남겨둠 나중에는 지울것 
    {
       
    }
    public void AddMoney(int addedmoney)
    {
        money += addedmoney;
    }
    
    public void HostGetDemage(float dmg)
    {

       
        photonView.RPC("RPCHostHP", RpcTarget.All, hostHP - dmg);
        if (hostHP <= 0)
        {
            GameManager.Instance.GuestWinGame();
            
        }

        
    }

    public void RPCHostHP(float hp)
    {
        hostHP = hp;
        OnHostHPChanged?.Invoke(hostHP);
    }






    public void GuestGetDemage(float dmg)
    {
        
        photonView.RPC("RPCGuestHP", RpcTarget.All, guestHP-dmg);
        if (guestHP <= 0)
        {
            GameManager.Instance.HostWinGame();
        }
    }

    public void RPCGuestHP(float hp)
    {
        guestHP = hp;
        OnGuestHPChanged.Invoke(guestHP);
    }



}
