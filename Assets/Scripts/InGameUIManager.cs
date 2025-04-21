using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public List<Unit> sellUnits = new List<Unit>();//기본적으로 유닛을 구매할 때 원래는 묶음 단위로 구매할까 했는데 단일로 구매하는걸로 변경
    public GameObject unitInfoPanel;


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

    public void BuyUnit(int index)
    {
        GameManager.Instance.unitManager.AddMyUnits(sellUnits[index]);
        var buyUnit = Instantiate(sellUnits[index],GameManager.Instance.unitSpawnPoint.transform.position, GameManager.Instance.unitSpawnPoint.transform.rotation);
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

    }
    public void DisableUnitInfo()
    {
        Debug.Log("disablepanel");
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

    }





}
