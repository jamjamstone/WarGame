using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int money=StaticField.startMoney;
    public float hostHP;
    public float guestHP;

    public delegate void PlayerHPChanged(float hp);
    public event PlayerHPChanged OnHostHPChanged;
    public event PlayerHPChanged OnGuestHPChanged;
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

    public void SetPlayerPosition()//카메라 컨트롤에서 따로 지정함 만들 필요 없다는 걸 알기 위해서 남겨둠 나중에는 지울것 
    {
       
    }
    public void AddMoney(int addedmoney)
    {
        money += addedmoney;
    }
    
    public void HostGetDemage(float dmg)
    {

        hostHP-=dmg;
        OnHostHPChanged?.Invoke(hostHP);
        if(hostHP <= 0)
        {
            GameManager.Instance.GuestWinGame();
            
        }

        
    }
    public void GuestGetDemage(float dmg)
    {
        guestHP-=dmg;
        OnGuestHPChanged.Invoke(guestHP);
        if (guestHP <= 0)
        {
            GameManager.Instance.HostWinGame();
        }
    }

}
