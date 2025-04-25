using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] float buyPhaseTime;
    [SerializeField] float battlePhaseTime;
    private bool isBuyPhase;

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
        RPCPhaseChange(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
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
            photonView.RPC("RPCPhaseChange", RpcTarget.All, false);//��Ʋ �������
        }
        if(battlePhaseTime >= StaticField.maximumBattleTime)
        {
            photonView.RPC("RPCPhaseChange", RpcTarget.All, true);//���� �������
            turnCount++;
            OnTurnChanged?.Invoke(turnCount);
        }



    }

    
    [PunRPC]
    public void RPCPhaseChange(bool isBuyPhase)
    {
        if (isBuyPhase)//����������� ����
        {
            isBuyPhase = true;
            buyPhaseTime = 0;
            OnChangeToBuyPhase?.Invoke();
           
        }
        else//���� ������� ����
        {
            isBuyPhase = false;
            battlePhaseTime = 0;
            OnChangeToBattlePhase?.Invoke();
        }
    }


}
