using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks,IPunObservable
{
    public float buyPhaseTime;
    public float battlePhaseTime;
    public bool isBuyPhase=true;

    public delegate void ChangeToBuyPhase();
    public event ChangeToBuyPhase OnChangeToBuyPhase;

    public delegate void ChangeToBattlePhase();
    public event ChangeToBattlePhase OnChangeToBattlePhase;

    public int turnCount=0;
    public delegate void TurnChanged(int nowTurn);
    public event TurnChanged OnTurnChanged;

    public bool isHostReady = false;
    public bool isGuestReady=false;


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCPhaseChange", RpcTarget.All, true);
        }
      
    }

    
    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(isHostReady&&isGuestReady&&isBuyPhase)
            {
                Debug.Log("tobattle by ready");
                photonView.RPC("RPCPhaseChange", RpcTarget.All, false);
            }

            if (isBuyPhase)
            {
                buyPhaseTime += Time.fixedDeltaTime;
            }
            else
            {
                battlePhaseTime += Time.fixedDeltaTime;
            }
            if (buyPhaseTime >= StaticField.maximumBuyTime)
            {
                Debug.Log("tobattle");
                photonView.RPC("RPCPhaseChange", RpcTarget.All, false);//��Ʋ �������
            }
            if (battlePhaseTime >= StaticField.maximumBattleTime)
            {
                photonView.RPC("RPCPhaseChange", RpcTarget.All, true);//���� �������
                
               
            }

        }
    }

    

    [PunRPC]
    public void RPCPhaseChange(bool isBuyPhase)
    {
        Debug.Log("turnrpc");
        if (isBuyPhase)//����������� ����
        {
            turnCount++;
            OnTurnChanged?.Invoke(turnCount);
            Debug.Log("tobuy");
            this.isBuyPhase = true;
            Debug.Log(isBuyPhase);
            buyPhaseTime = 0;
            battlePhaseTime = 0;
            OnChangeToBuyPhase?.Invoke();//null2
           
        }
        else//���� ������� ����
        {
            Debug.Log("tobattle");
            
            this.isBuyPhase = false;
            battlePhaseTime = 0;
            buyPhaseTime = 0;
            OnChangeToBattlePhase?.Invoke();
            isGuestReady = false;
            isHostReady = false;
            
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(buyPhaseTime);
            stream.SendNext(battlePhaseTime);
            stream.SendNext(isBuyPhase);
            
        }
        else
        {
            buyPhaseTime = (float)stream.ReceiveNext();
            battlePhaseTime= (float)stream.ReceiveNext();   
            isBuyPhase= (bool)stream.ReceiveNext();
           

        }
       
    }
}
