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
    
    /// <summary>
    /// 음식 정보를 담은 동적배열 + 리스트 2중 구조
    /// </summary>
    /// <description>
    /// 오픈 체인 형식으로 구성하여 음식 개수에 상관 없이 대응 가능
    /// 인덱스가 앉은 자리와 일치
    /// </description>
    private List<FoodData>[] foodDatas;
    private List<int> foodIDs; // FoodDB 상의 음식 index. 손님에게서 랜덤하게 생성됨.
    private string foodName;
    private int foodPrice;
    private int tableNum;

    private FoodDB foodDB;
    [SerializeField]
    private OrderMemo orderMemo; // 플레이어 UI 받은 주문 목록
    [SerializeField]
    private TableManager tableManager;

    private void Start()
    {
        // initialize
        // orderMemo = GameObject.FindObjectOfType<OrderMemo>();
        // do not use above ; OrderMemo Component does not activate.
        foodDB = GameObject.FindObjectOfType<FoodDB>();
    }

    // setter, getter
    // 음식 정보 구성을 시작하는 함수 - 플레이어의 상호작용 E에서 출발
    public void SetFoodNumFromPlayer(List<int>[] foodIDs, int tableNum)
    {
        int chairNum_ = tableManager.GetTable(tableNum - 1).chairNum;
        foodDatas = new List<FoodData>[chairNum_];
        for(int i = 0; i < chairNum_; i++)
        {
            foodDatas[i] = new List<FoodData>();
        }
        GetFoodInfoFromDB(foodIDs);
        SendFoodInfo(foodDatas, tableNum);
    }

    // 데이터 전달 흐름
    // VistorOrder -> FoodDB -> VisitorOrder
    private void GetFoodInfoFromDB(List<int>[] foodIDsArray)
    {
        int tempIdx = 0;
        foreach (var foodIDs in foodIDsArray)
        {
            // 빈 인덱스는 null 값이 들어있다.
            if (foodIDs == null)
            {
                tempIdx++;
                continue;
            }

            foreach(var foodID in foodIDs)
            {
                // foodDatas 배열에 손님이 앉은 위치와 동일한 인덱스에 음식 정보가 리스트로 들어있음
                foodDatas[tempIdx].Add(foodDB.GetFoodData(foodID));
            }
            tempIdx++;
        }
    }

    // 최종적으로 UI 상에 뜰 정보를 보내는 함수
    public void SendFoodInfo(List<FoodData>[] foodData, int tableNum)
    {
        // OrderMemo 상에서 표시될 정보를 이 함수에서 초기화
        orderMemo.GetFoodInfo(foodData, tableNum);
    }
    // OrderMemo에 어떤 식으로 넘기고 관리할지 회의 필요

}
