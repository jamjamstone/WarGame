using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public List<Unit> sellUnits = new List<Unit>();//�⺻������ ������ ������ �� ������ ���� ������ �����ұ� �ߴµ� ���Ϸ� �����ϴ°ɷ� ����
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
    public void ShowStrategeInfo()// ī�� ����
    {

    }

    public void ShowShop()//���� �����ִ� �޼���
    {

    }





}
