using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public List<Unit> sellUnits = new List<Unit>();//기본적으로 유닛을 구매할 때 원래는 묶음 단위로 구매할까 했는데 단일로 구매하는걸로 변경

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
    // Start is called before the first frame update
    void Start()
    {
        unitInfoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddUnitToList(Unit unit)
    {
        sellUnits.Add(unit);
    }
    public void BuyUnit(int index)
    {
        if (GameManager.Instance.playerManager.money < sellUnits[index].unitInfo.unitPrice)
        {
            OnBuyDenied();
            return;
        }
        GameManager.Instance.unitManager.AddMyUnits(sellUnits[index]);
        var buyUnit = Instantiate(sellUnits[index],GameManager.Instance.unitSpawnPoint.transform.position, GameManager.Instance.unitSpawnPoint.transform.rotation);
    }
    public void OnBuyDenied()
    {
        buyDenied.gameObject.SetActive(false);
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
