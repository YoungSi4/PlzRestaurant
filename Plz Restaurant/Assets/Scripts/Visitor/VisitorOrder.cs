using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorOrder : MonoBehaviour
{
    private int FoodNum; // FoodDB 상의 음식 index. 손님에게서 랜덤하게 생성됨.
    private OrderMemo orderMemo; // 플레이어 UI 받은 주문 목록

    // Vars related to FoodDB
    // 음식 번호, 이름, 가격 등등

    private void Start()
    {
        // initialize
        orderMemo = GameObject.FindObjectOfType<OrderMemo>();
    }



    // setter, getter
    // 음식 정보 구성을 시작하는 함수 - 플레이어의 상호작용 E에서 출발
    // SetFoodNum -> GetFoodInfo -> SendFoodInfo
    public void SetFoodNum(int foodNum)
    {
        FoodNum = foodNum;
        GetFoodInfoFromDB(FoodNum);
    }

    // VistorOrder -> FoodDB -> VisitorOrder
    private void GetFoodInfoFromDB(int  foodNum)
    {
        // out 이나 ref 를 활용해서 가져올 것
        // foodDB.getInfo(foodNum, &name, &price, &img, etc);
    }



    public void SendFoodInfo(int foodNum)
    {
        // OrderMemo 상에서 표시될 정보를 이 함수에서 초기화
        orderMemo.GetFoodInfoFromOrder(foodNum);
    }
}
