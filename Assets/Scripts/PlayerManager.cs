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
        GameManager.Instance.turnManager.OnChangeToBuyPhase += ()=>AddMoney(500);//���� 500�߰�
        hostHP = StaticField.maxPlayerHp;
        guestHP = StaticField.maxPlayerHp;
        ResetMoney();
    }
    public void ResetMoney()
    {
        money=StaticField.startMoney;
    }

    public void SetPlayerPosition()//ī�޶� ��Ʈ�ѿ��� ���� ������ ���� �ʿ� ���ٴ� �� �˱� ���ؼ� ���ܵ� ���߿��� ����� 
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
