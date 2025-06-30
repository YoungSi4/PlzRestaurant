using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderMemo : MonoBehaviour
{
    // 숫자가 낮을수록 아래
    public GameObject OrderMemoBlock1;
    public GameObject OrderMemoBlock2;
    public GameObject OrderMemoBlock3;
    public GameObject OrderMemoBlock4;
    private TextMeshPro TMP;

    private int tableNum;
    private string foodName;
    // food image?
    private int foodPrice;

    // FoodInfo
    private VisitorOrder order;
    private FoodData foodData;

    void Start()
    {
        TMP = OrderMemoBlock1.GetComponent<TextMeshPro>();
    }

    // FoodDB 객체에서 이 함수를 실행...?
    // Food Manager 같은 중간 매개체로 전달하는 게 안전해보인다.
    
    // 테이블 번호는 어디서 받아서 넘기지? -> visitor order 객체
    public void GetFoodInfo(ref FoodData foodData, int tableNum)
    {
        this.foodData = foodData;
        this.tableNum = tableNum;
        
    }

    private void SetData()
    {
        foodName = foodData.foodName;
        foodPrice = foodData.foodPrice; 
    }

    private void SetText()
    {
        
    }
}
