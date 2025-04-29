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


    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("RPCPhaseChange", RpcTarget.All, true);
      
    }

    
    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
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
                
                photonView.RPC("RPCPhaseChange", RpcTarget.All, false);//배틀 패이즈로
            }
            if (battlePhaseTime >= StaticField.maximumBattleTime)
            {
                photonView.RPC("RPCPhaseChange", RpcTarget.All, true);//구매 페이즈로
                turnCount++;
                OnTurnChanged?.Invoke(turnCount);
            }

        }
    }

    

    [PunRPC]
    public void RPCPhaseChange(bool isBuyPhase)
    {
        Debug.Log("turnrpc");
        if (isBuyPhase)//구매페이즈로 변경
        {
            Debug.Log("tobuy");
            this.isBuyPhase = true;
            Debug.Log(isBuyPhase);
            buyPhaseTime = 0;
            battlePhaseTime = 0;
            OnChangeToBuyPhase?.Invoke();
           
        }
        else//전투 페이즈로 변경
        {
            Debug.Log("tobattle");
            this.isBuyPhase = false;
            battlePhaseTime = 0;
            buyPhaseTime = 0;
            OnChangeToBattlePhase?.Invoke();
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
