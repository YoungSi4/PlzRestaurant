using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderMemo : MonoBehaviour
{
    /* 제대로 구현할 목록
     *  1. OrderMemoBlock을 주문 수에 맞게 생성
     *  2. 수에 맞게 길이 조정 및 스크롤 가능하게 변경
     */

    // 숫자가 낮을수록 아래
    public GameObject OrderMemoBlock1;
    public GameObject OrderMemoBlock2;
    public GameObject OrderMemoBlock3;
    public GameObject OrderMemoBlock4;
    public Button accept;
    public Button reject;

    private int tableNum;
    private string foodName;
    // food image?
    private int foodPrice;

    // FoodInfo
    private VisitorOrder order;
    private FoodData foodData;

    void Start()
    {

    }

    // FoodDB 객체에서 이 함수를 실행...?
    // Food Manager 같은 중간 매개체로 전달하는 게 안전해보인다.
    
    // 테이블 번호는 어디서 받아서 넘기지? -> visitor order 객체
    public void GetFoodInfo(FoodData foodData, int tableNum)
    {
        this.foodData = foodData;
        this.tableNum = tableNum;
        
        SetData();
    }

    private void SetData()
    {
        foodName = foodData.foodName;
        foodPrice = foodData.foodPrice; 
    }

    private void SetText()
    {
        var tableNum = foodData.foodNum;
        var foodName = foodData.foodName;
        var foodPrice = foodData.foodPrice;

        var texts = OrderMemoBlock1.GetComponentsInChildren<TextMeshPro>();

        // 1. 테이블 번호, 2. 음식 이름, 3. 음식 가격
        texts[1].SetText(tableNum.ToString());
        texts[2].SetText(foodName.ToString());
        texts[3].SetText(foodPrice.ToString());
    }
}
