using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrategeCard : MonoBehaviour
{
    public StrategeCardInfo cardInfo;
    public TMP_Text cardName;
    public TMP_Text cardDescription;
    public RawImage[] cardCostList=new RawImage[StaticField.MaxCardCost];//�ڽ�Ʈ �̹����� ������ �����ͼ� �Ҵ��ϱ�
    public RawImage cardIcon;
    public RawImage cardCost;
    

    
    void Start()
    {
       
    }
    public void SetCardInfo()//cardinfo���� �����͸� ������ �۵�
    {
        cardName.text=cardInfo.cardName;
        cardDescription.text=cardInfo.cardDescription;
        cardIcon=cardInfo.cardIcon;
        cardCost = cardCostList[cardInfo.cardCost-1];
    }
   
    void Update()
    {
        
    }
}
