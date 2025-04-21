using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public List<GameObject> sellUnits = new List<GameObject>();//기본적으로 유닛을 구매할 때 원래는 묶음 단위로 구매할까 했는데 단일로 구매하는걸로 변경

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
        Debug.Log("trbuy");
        if (GameManager.Instance.playerManager.money < sellUnits[index].GetComponent<Unit>().unitInfo.unitPrice)
        {
            Debug.Log(GameManager.Instance.playerManager.money);
            Debug.Log(sellUnits[index].GetComponent<Unit>().unitInfo.unitPrice);
            OnBuyDenied();
            return;
        }
        GameManager.Instance.unitManager.AddMyUnits(sellUnits[index].GetComponent<Unit>());
        Debug.Log("trbuy2");
        var buyUnit = Instantiate(sellUnits[index].gameObject,GameManager.Instance.unitSpawnPoint.transform.position, GameManager.Instance.unitSpawnPoint.transform.rotation);
        Debug.Log("trbuy3");
        GameManager.Instance.playerManager.money -= (int)sellUnits[index].GetComponent<Unit>().unitInfo.unitPrice;
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



}
