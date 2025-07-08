using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
public class InGameUIManager : MonoBehaviourPun
{
    public List<GameObject> sellUnits = new List<GameObject>();//기본적으로 유닛을 구매할 때 원래는 묶음 단위로 구매할까 했는데 단일로 구매하는걸로 변경

    [Header("Player")]
    public GameObject upperPanel;
    [SerializeField] TMP_Text playerLeftMoney;
    [SerializeField] TMP_Text hostHp;
    [SerializeField] TMP_Text guestHp;
    [SerializeField] TMP_Text nowTurnText;
    [SerializeField] TMP_Text leftTimeText;


    [Header("UnitInfo")]
    public GameObject unitInfoPanel;
    [SerializeField] TMP_Text unitInfoUnitName;
    [SerializeField] RawImage unitInfoUnitIcon;
    [SerializeField] TMP_Text unitInfoUnitStatus;
    

    [Header("Shop")]
    public GameObject shopPanel;
    [SerializeField] TMP_Text buyDenied;
    [SerializeField] GameObject baseUnitButton;

    [Header("Menu")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button surrenderButton;
    [SerializeField] Button quitButton;

    [SerializeField] GameObject winText;
    [SerializeField] GameObject defeatText;

    [SerializeField] Button readyButton;
    [SerializeField] TMP_Text readyButtonText;  

    private bool isUnitInfoActivate = false;
    private float colorAlpha = 1;
    private bool isGameEnd = false;
    

    public delegate void moneyChanged();
    public event moneyChanged OnMoneyChanged;

    public delegate void fadeOutEnd();
    public event fadeOutEnd OnFadeOutEnd;






    // Start is called before the first frame update
    void Start()
    {
        unitInfoPanel.SetActive(false);
        OnMoneyChanged += SetMoneyToUI;
        OnFadeOutEnd += FadeOutEnd;
        GameManager.Instance.OnGuestWin += GuestWinText;
        GameManager.Instance.OnHostWin += HostWinText;
        GameManager.Instance.turnManager.OnTurnChanged += SetTurnCount;
        GameManager.Instance.playerManager.OnHostHPChanged += SetHostHp;
        GameManager.Instance.playerManager.OnGuestHPChanged += SetGuestHP;
        SetHostHp(StaticField.maxPlayerHp);//오류
        SetGuestHP(StaticField.maxPlayerHp);
        SetTurnCount(0);
        MakeUnitBuyButton();
        SetMoneyToUI();
    }
    public void SetMoneyToUI()
    {
        playerLeftMoney.text= GameManager.Instance.playerManager.money.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.activeSelf==true)
            {
                menuPanel.SetActive(false);
            }
            else
            {
                menuPanel.SetActive(true);
            }
        }
        if (isGameEnd==true)
        {
            return;
        }
        if (GameManager.Instance.turnManager.isBuyPhase == true)
        {
            leftTimeText.text=(StaticField.maximumBuyTime-GameManager.Instance.turnManager.buyPhaseTime).ToString();
        }
        else
        {
            leftTimeText.text = (StaticField.maximumBattleTime - GameManager.Instance.turnManager.battlePhaseTime).ToString();
        }
    }
    public void SetTurnCount(int turn)
    {
        nowTurnText.text= turn.ToString();
    }
    public void SetHostHp(float hp)
    {
        hostHp.text= hp.ToString();//오류
    }
    public void SetGuestHP(float hp)
    {
        guestHp.text= hp.ToString();
    }
    public void AddUnitToList(GameObject unit)
    {
        sellUnits.Add(unit);
    }
    
    public void BuyUnit(GameObject unit)
    {
        //Debug.Log("trbuy");
        if (GameManager.Instance.playerManager.money < unit.GetComponent<Unit>().unitInfo.unitPrice)//돈이 모자라면
        {
            
            OnBuyDenied();
            return;
        }
        if (GameManager.Instance.turnManager.isBuyPhase ==false)//배틀패이즈이면
        {
            OnBuyDenied();
            return;
        }
        if (unit == null)
        {
            Debug.Log("unit=null");
            return;
        }
        // Debug.Log("trbuy2");
        if (PhotonNetwork.IsMasterClient)
        {
            var tempUnit=PhotonNetwork.Instantiate(unit.name, GameManager.Instance.hostUnitSpawnPoint.transform.position, GameManager.Instance.hostUnitSpawnPoint.transform.rotation);
            GameManager.Instance.unitManager.AddMyUnits(tempUnit.GetComponent<Unit>());
            //Debug.Log("spawnhostpos");
        }
        else
        {
           var tempUnit= PhotonNetwork.Instantiate(unit.name, GameManager.Instance.nonHostUnitSpawnPoint.transform.position, GameManager.Instance.nonHostUnitSpawnPoint.transform.rotation);
            if (tempUnit != null)
            {
                GameManager.Instance.unitManager.AddMyUnits(tempUnit.GetComponent<Unit>());
            }
            else
            {
                Debug.Log("unit null?");
            }
            //Debug.Log("spawnguestpos");
        }
        
        //var buyUnit = Instantiate(unit.gameObject, GameManager.Instance.hostUnitSpawnPoint.transform.position, GameManager.Instance.hostUnitSpawnPoint.transform.rotation);
        // Debug.Log("trbuy3");
        GameManager.Instance.playerManager.money -= (int)unit.GetComponent<Unit>().unitInfo.unitPrice;
        OnMoneyChanged?.Invoke();
    }
    public void MakeUnitBuyButton()
    {
        foreach (var unit in sellUnits)
        {
            
            var tempUnit = unit;
            var temp = Instantiate(baseUnitButton, shopPanel.transform);

            temp.GetComponent<Button>().onClick.AddListener(() => BuyUnit(unit));//addlistner를 쓰기 위해서 람다식 사용

            EventTrigger.Entry entry = new EventTrigger.Entry();//이벤트 트리거에 할당하기 위해서 엔트리 별도 생성
            entry.eventID = EventTriggerType.PointerEnter;//이벤트 아이디 할당
            entry.callback.AddListener((eventData) => ShowUnitInfoByPoint(tempUnit.GetComponent<Unit>())); //함수 추가
            temp.GetComponent<EventTrigger>().triggers.Add(entry);

            EventTrigger.Entry entryExit = new EventTrigger.Entry();//이벤트 트리거에 할당하기 위해서 엔트리 별도 생성
            entryExit.eventID = EventTriggerType.PointerExit;//이벤트 아이디 할당
            entryExit.callback.AddListener((eventData) => DisableUnitInfo()); //함수 추가
            temp.GetComponent<EventTrigger>().triggers.Add(entryExit);
        }
    }

    public void OnBuyDenied()
    {
        Debug.Log(GameManager.Instance.turnManager.isBuyPhase);
        buyDenied.gameObject.SetActive(true);
        colorAlpha = 1;
        StartCoroutine(FadeOutBuyDenied());
    }

    public void FadeOutEnd()
    {
        buyDenied.gameObject.SetActive(false);
        colorAlpha = 1;
        StopCoroutine(FadeOutBuyDenied());
    }
    public void GameEnd()
    {
        isGameEnd = true;
    }
    IEnumerator FadeOutBuyDenied()
    {
        var tempColor=Color.white;
        while (true)
        {
            if (colorAlpha<0)
            {

                yield return null;
                OnFadeOutEnd?.Invoke();
            }
            yield return new WaitForSeconds(0.2f);
            colorAlpha -= Time.deltaTime;
            tempColor.a=colorAlpha;
            buyDenied.color = tempColor;

        }
    }
    public void ShowUnitInfoByPoint(Unit unit)//마우스로 가리키면 유닛 정보 알려주는 메소드
    {
        if (isUnitInfoActivate == false)
        {
            isUnitInfoActivate = true;
        }
        else
        {
            return;
        }

        unitInfoPanel.SetActive(true);
        unitInfoUnitName.text = unit.unitInfo.unitName;
        unitInfoUnitStatus.text = "HP: " + unit.unitInfo.unitHP + "\nATK: " + unit.unitInfo.unitATK + "\nSpeed: " + unit.unitInfo.unitSpeed;
    }
    public void DisableUnitInfo()
    {
        //Debug.Log("disablepanel");
        if (isUnitInfoActivate == true)
        {
            isUnitInfoActivate = false;
        }
        else
        {
            return;
        }
        unitInfoPanel.SetActive(false);
    }
    public void ShowStrategeInfo()// 카드 인포
    {

    }
    public void ReadyButtonPressed()
    {
        readyButtonText.text = "Ready!";
        readyButton.enabled = false;
        if (PhotonNetwork.IsMasterClient)
        {
            
            photonView.RPC("RPCHostReady", RpcTarget.All);
        }
        else
        {
            
            photonView.RPC("RPCGuestReady", RpcTarget.All);
        }
    }
    [PunRPC]
    public void RPCHostReady()
    {
        GameManager.Instance.turnManager.isHostReady = true;
    }
    [PunRPC]
    public void RPCGuestReady()
    {
        GameManager.Instance.turnManager.isGuestReady = true;
    }


    public void ReadyButtonActivate()
    {
        readyButton.enabled = true;
        readyButtonText.text = "Not Ready!";
    }
    public void ShowShop()//상점 보여주는 메서드
    {
        shopPanel.SetActive(true);
    }
    public void DisableShop()
    {
        shopPanel.SetActive(false);
    }
    public void SetShopForUnit()
    {
        for(int i =0; i<sellUnits.Count; i++)
        {
            var tempButton = Instantiate(baseUnitButton, shopPanel.transform);
        }
    }

    
    public void QuitButtonPressed()
    {
        SurrenderButtonPressed();
        Application.Quit();
    }

    public void SurrenderButtonPressed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.GuestWinGame();
        }
        else
        {
            GameManager.Instance.HostWinGame();
        }
    } 
    
    public void HostWinText()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            winText.SetActive(true);
        }
        else
        {
            defeatText.SetActive(true);
        }
        isGameEnd = true;
    }


    public void GuestWinText()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            winText.SetActive(true);
        }
        else
        {
            defeatText.SetActive(true);
        }
        isGameEnd = true;
    }

}
