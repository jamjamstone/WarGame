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
public class InGameUIManager : MonoBehaviour
{
    public List<GameObject> sellUnits = new List<GameObject>();//�⺻������ ������ ������ �� ������ ���� ������ �����ұ� �ߴµ� ���Ϸ� �����ϴ°ɷ� ����

    [Header("Player")]
    public GameObject upperPanel;
    [SerializeField] TMP_Text playerLeftMoney;


    [Header("UnitInfo")]
    public GameObject unitInfoPanel;
    [SerializeField] TMP_Text unitInfoUnitName;
    [SerializeField] RawImage unitInfoUnitIcon;
    [SerializeField] TMP_Text unitInfoUnitStatus;
    

    [Header("Shop")]
    public GameObject shopPanel;
    [SerializeField] TMP_Text buyDenied;

    [SerializeField] GameObject baseUnitButton;



    private bool isUnitInfoActivate = false;
    private float colorAlpha = 1;
    

    public delegate void moneyChanged();
    public event moneyChanged OnMoneyChanged;

    // Start is called before the first frame update
    void Start()
    {
        unitInfoPanel.SetActive(false);
        OnMoneyChanged += SetMoneyToUI;
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
        
    }
    public void AddUnitToList(GameObject unit)
    {
        sellUnits.Add(unit);
    }
    public void BuyUnit(int index)
    {
        //Debug.Log("trbuy");
        if (GameManager.Instance.playerManager.money < sellUnits[index].GetComponent<Unit>().unitInfo.unitPrice)
        {
            Debug.Log(GameManager.Instance.playerManager.money);
            Debug.Log(sellUnits[index].GetComponent<Unit>().unitInfo.unitPrice);
            OnBuyDenied();
            return;
        }
        GameManager.Instance.unitManager.AddMyUnits(sellUnits[index].GetComponent<Unit>());
       // Debug.Log("trbuy2");
        var buyUnit = Instantiate(sellUnits[index].gameObject,GameManager.Instance.hostUnitSpawnPoint.transform.position, GameManager.Instance.hostUnitSpawnPoint.transform.rotation);
       // Debug.Log("trbuy3");
        GameManager.Instance.playerManager.money -= (int)sellUnits[index].GetComponent<Unit>().unitInfo.unitPrice;
        OnMoneyChanged?.Invoke();
    }
    public void BuyUnit(GameObject unit)
    {
        //Debug.Log("trbuy");
        if (GameManager.Instance.playerManager.money < unit.GetComponent<Unit>().unitInfo.unitPrice)
        {
            
            OnBuyDenied();
            return;
        }
        GameManager.Instance.unitManager.AddMyUnits(unit.GetComponent<Unit>());
        // Debug.Log("trbuy2");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(unit.name, GameManager.Instance.hostUnitSpawnPoint.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.InstantiateRoomObject(unit.name, GameManager.Instance.nonHostUnitSpawnPoint.transform.position, Quaternion.identity);
        }
        
        var buyUnit = Instantiate(unit.gameObject, GameManager.Instance.hostUnitSpawnPoint.transform.position, GameManager.Instance.hostUnitSpawnPoint.transform.rotation);
        // Debug.Log("trbuy3");
        GameManager.Instance.playerManager.money -= (int)unit.GetComponent<Unit>().unitInfo.unitPrice;
        OnMoneyChanged?.Invoke();
    }
    public void OnBuyDenied()
    {
        buyDenied.gameObject.SetActive(true);
        colorAlpha = 1;
        StartCoroutine(FadeOutBuyDenied());
    }


    IEnumerator FadeOutBuyDenied()
    {
        var tempColor=Color.white;
        while (true)
        {
            if (colorAlpha<0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1);
            colorAlpha -= Time.deltaTime;
            tempColor.a=colorAlpha;
            buyDenied.color = tempColor;

        }
    }
    public void ShowUnitInfoByPoint(Unit unit)//���콺�� ����Ű�� ���� ���� �˷��ִ� �޼ҵ�
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
    public void ShowStrategeInfo()// ī�� ����
    {

    }

    public void ShowShop()//���� �����ִ� �޼���
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

    public void MakeUnitBuyButton()
    {
        foreach(var unit in sellUnits)
        {
            var tempUnit = unit;
            var temp=Instantiate(baseUnitButton,shopPanel.transform);

            temp.GetComponent<Button>().onClick.AddListener(()=>BuyUnit(tempUnit));//addlistner�� ���� ���ؼ� ���ٽ� ���
                                                                                  
            EventTrigger.Entry entry = new EventTrigger.Entry();//�̺�Ʈ Ʈ���ſ� �Ҵ��ϱ� ���ؼ� ��Ʈ�� ���� ����
            entry.eventID = EventTriggerType.PointerEnter;//�̺�Ʈ ���̵� �Ҵ�
            entry.callback.AddListener((eventData) => ShowUnitInfoByPoint(tempUnit.GetComponent<Unit>())); //�Լ� �߰�
            temp.GetComponent<EventTrigger>().triggers.Add(entry);

            EventTrigger.Entry entryExit = new EventTrigger.Entry();//�̺�Ʈ Ʈ���ſ� �Ҵ��ϱ� ���ؼ� ��Ʈ�� ���� ����
            entryExit.eventID = EventTriggerType.PointerExit;//�̺�Ʈ ���̵� �Ҵ�
            entryExit.callback.AddListener((eventData) => DisableUnitInfo()); //�Լ� �߰�
            temp.GetComponent<EventTrigger>().triggers.Add(entryExit);
        }
    }
    
}
