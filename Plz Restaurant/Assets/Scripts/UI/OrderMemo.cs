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

    private List<FoodData>[] foodDatas;

    // FoodInfo
    private VisitorOrder order;
    private FoodData foodData;

    [SerializeField]
    private HeadChef chef;

    // FoodDB 객체에서 이 함수를 실행...?
    // Food Manager 같은 중간 매개체로 전달하는 게 안전해보인다.
    
    // 테이블 번호는 어디서 받아서 넘기지? -> visitor order 객체
    public void GetFoodInfo(List<FoodData>[] foodData, int tableNum)
    {
        // 여기서 어디에 뭘 넣을지 모르겠다
        // 이건 UI가 나와야 가능
        this.foodDatas = foodData;
        this.tableNum = tableNum;

        //this.foodData = foodData;
        //this.tableNum = tableNum;
        
        // 디버깅용
        Debug.Log("Memo - 받은 테이블 번호 : " +  tableNum);
        string message = "";
        message += "Memo - 받은 음식 번호 : ";
        foreach(var dataList in foodDatas)
        {
            foreach(var data in dataList)
            {
                message += data.foodNum;
                message += " ";
            }
        }
        Debug.Log(message);

        SetData();

        // 임시 테스트용 호출
        SendFoodDataToChef();
    }

    private void SetData()
    {
        //foodName = foodData.foodName;
        //foodPrice = foodData.foodPrice;

        foreach (var foodDataList in foodDatas)
        {

        }
            SetText();
    }



    private void SetText()
    {
        var tableNum = foodData.foodNum;
        var foodName = foodData.foodName;
        var foodPrice = foodData.foodPrice;

        var texts = OrderMemoBlock1.GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void SendFoodDataToChef()
    {
        // 쉐프 쪽 함수 호출
        chef.GetFoodDataFromOrderMemo(foodDatas, tableNum);
    }
}
