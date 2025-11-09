using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    // private FoodData foodData;

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


    // 2025-11-09 추가함
    Dictionary<string, int[]> receiptText; //string : foodname, int[] : 개수, sub total


    private void SetData()
    {
        //foodName = foodData.foodName;
        //foodPrice = foodData.foodPrice;

        if (receiptText == null)
            receiptText = new Dictionary<string, int[]>();

        foreach (var foodDataList in foodDatas)
        {
            foreach( var foodData in foodDataList)
            {
                if(receiptText.TryGetValue(foodData.foodName,out int[] value))
                {
                    //이미 한 번 이상 나온 음식이면
                    value[0] += 1; //개수칸에 개수 1개늘리고
                    value[1] += foodData.foodPrice; //sub total칸에 총 가격도 더해줌
                }
                else
                {
                    //처음 나온 음식이라면
                    receiptText.Add(foodData.foodName, new int[] { 1, foodData.foodPrice });
                }
            }
        }
            SetText();
    }

    public GameObject firstpanelPrefab; //처음에 테이블 번호있는 패널 
    public GameObject panelPrefab;      // Panel (메뉴 이름, 메뉴 개수 담긴) 프리팹
    public Transform contentArea;      // ScrollView의 Content 오브젝트

    public TextMeshProUGUI tableCountText;

    private void SetText()
    {
        // var tableNum = foodData.foodNum;
        // var foodName = foodData.foodName;
        // var foodPrice = foodData.foodPrice;

        // var texts = OrderMemoBlock1.GetComponentsInChildren<TextMeshProUGUI>();


        

        tableCountText.text = tableNum.ToString();

        //기존 패널 전부 삭제
        foreach (Transform child in contentArea)
        {
            if (child.name == "First Panel")
                continue;
            Destroy(child.gameObject);
        }

        

        foreach (var item in receiptText) { 

            string foodName = item.Key;
            int count = item.Value[0];
            int subtotal = item.Value[1];

            // Panel 생성
            GameObject newPanel = Instantiate(panelPrefab, contentArea);

            //내부 텍스트 가져오기 
            var texts = newPanel.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = $"{foodName} {count}";
            texts[1].text = $"{subtotal}";
        }

    }

    private void SendFoodDataToChef()
    {
        // 쉐프 쪽 함수 호출
        chef.GetFoodDataFromOrderMemo(foodDatas, tableNum);
    }
}
