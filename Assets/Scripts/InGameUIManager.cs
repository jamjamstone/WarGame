using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public List<Unit> sellUnits = new List<Unit>();//�⺻������ ������ ������ �� ������ ���� ������ �����ұ� �ߴµ� ���Ϸ� �����ϴ°ɷ� ����

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



}
