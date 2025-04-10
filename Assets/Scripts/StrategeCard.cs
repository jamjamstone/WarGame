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
    public RawImage[] cardCostList=new RawImage[StaticField.MaxCardCost];//코스트 이미지나 아이콘 가져와서 할당하기
    public RawImage cardIcon;
    public RawImage cardCost;
    

    
    void Start()
    {
       
    }
    public void SetCardInfo()//cardinfo에서 데이터를 가져와 작동
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
