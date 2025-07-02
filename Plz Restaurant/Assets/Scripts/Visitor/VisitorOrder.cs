using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 사용 관련
 * Empty Object 로 생성하여 스크립트만 넣어줌
 *  GameManager나 OrderManager 아무튼 무언가 아래에 둘 예정
 */

public class VisitorOrder : MonoBehaviour
{
    // Vars related to FoodDB
    // 음식 번호, 이름, 가격 등등
    private FoodData foodData;
    private int foodNum; // FoodDB 상의 음식 index. 손님에게서 랜덤하게 생성됨.
    private string foodName;
    private int foodPrice;
    private int tableNum;

    private FoodDB foodDB;
    private OrderMemo orderMemo; // 플레이어 UI 받은 주문 목록

    private void Start()
    {
        // initialize
        orderMemo = GameObject.FindObjectOfType<OrderMemo>();
        foodDB = GameObject.FindObjectOfType<FoodDB>();
    }

    // setter, getter
    // 음식 정보 구성을 시작하는 함수 - 플레이어의 상호작용 E에서 출발
    public void SetFoodNumFromVisitor(int foodNum, int tableNum)
    {
        this.foodNum = foodNum;
        this.tableNum = tableNum;

        GetFoodInfoFromDB(foodNum);
        SendFoodInfo(foodData, foodNum);
    }

    // 데이터 전달 흐름
    // VistorOrder -> FoodDB -> VisitorOrder
    private void GetFoodInfoFromDB(int foodNum)
    {
        foodData = foodDB.GetFoodData(foodNum);
    }

    public void SendFoodInfo(FoodData foodData, int tableNum)
    {
        // OrderMemo 상에서 표시될 정보를 이 함수에서 초기화
        orderMemo.GetFoodInfo(foodData, tableNum);
    }
}
