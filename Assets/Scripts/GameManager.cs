using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager gameManager;
    public InGameUIManager uiManager;
    public TurnManager turnManager;
    public UnitManager unitManager;
    public StrategeManager strategeManager;
    public PlayerManager playerManager;

    public List<GameObject> unitList=new List<GameObject>();//프리팹을 저장하는 방식으로
    public List<GameObject> strategeList=new List<GameObject>();//프리팹을 저장하는 방식으로 -> 전술카드는 구현 후순위로

    public GameObject hostUnitSpawnPoint;
    public GameObject nonHostUnitSpawnPoint;

    public delegate void HostWin();
    public event HostWin OnHostWin;

    public delegate void GuestWin();
    public event GuestWin OnGuestWin;


    public static GameManager Instance
    {
        get {
              if (gameManager == null)
            { 
                return null;
            }
            else
            {
                return gameManager;
            }

        }
        
    }
    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerManager.ResetMoney();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnitToList(GameObject unit)
    {
        unitList.Add(unit);
        //uiManager.AddUnitToList(unit.GetComponent<Unit>());
    }
    public void AddStrategeToList(GameObject stratege)
    {
        strategeList.Add(stratege);
    }
    public void HostWinGame()
    {
        photonView.RPC("RPCHostWin", RpcTarget.All);
        
    }
    [PunRPC]
    public void RPCHostWin()
    {
        OnHostWin?.Invoke();
    }

    public void GuestWinGame()
    {
        photonView.RPC("RPCGuestWin", RpcTarget.All);
    }
    [PunRPC]
    public void RPCGuestWin()
    {
        OnGuestWin?.Invoke();
    }
}
