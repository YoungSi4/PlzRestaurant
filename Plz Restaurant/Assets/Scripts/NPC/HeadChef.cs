using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// FoodData와 정수형 변수를 일괄적으로 관리하는 것이 유리하기 때문에 사용
public class OrderData
{
    public FoodData foodData { get; private set; } // 음식 데이터
    public int tableNum { get; private set; } // 테이블번호

    public OrderData(FoodData foodData, int tableNum)
    {
        this.foodData = foodData;
        this.tableNum = tableNum;
    }
}

public class HeadChef : MonoBehaviour
{
    // 현재 조리중인 주문서 목록 (OrderData 버전)
    private Queue<OrderData> H_cookingList = new Queue<OrderData>();
    // 9.15 로직 수정
    private int tableCount; // 테이블 수
    private List<FoodData>[][] foodDatasOnTable; // 테이블 크기의 배열에 저장된 의자 수 배열에 저장된 음식 데이터 리스트
    private Queue<int> tableNums = new Queue<int>(); // 주문 들어온 순서대로 테이블 번호 저장

    private float cookTime = 1f; // 음식 조리에 걸리는 시간(5초로 수정 예정)
    private bool isCooking = false;

    private FoodDB foodDB;
    private TrayControl trayControl;
    private TableManager tableManager;
    private NPC npc;

    private void Start()
    {
        tableCount = tableManager.tableNum;
        foodDatasOnTable = new List<FoodData>[tableCount][]; // 테이블 수 x 의자 수 (2 or 4 가변)
        // 각 테이블별로 의자 수에 맞게 내부 배열 초기화
        for (int i = 0; i < tableCount; i++)
        {
            var table = tableManager.GetTable(i);
            int chairCount = table.chairNum;
            // Debug.Log($"[HeadChef.Start] table[{i}] 의자 수 = {chairCount}");

            foodDatasOnTable[i] = new List<FoodData>[chairCount]; // 내부 배열 생성

            for (int j = 0; j < chairCount; j++)
            {
                foodDatasOnTable[i][j] = new List<FoodData>();
            }
        }
    }

    private void Awake()
    {
        foodDB = FindObjectOfType<FoodDB>();
        trayControl = FindObjectOfType<TrayControl>();
        tableManager = FindObjectOfType<TableManager>();
        npc = FindObjectOfType<NPC>();
    }

    private void Update()
    {
        H_startCookingRoutine();
        // GetFood();
        ClearTableFoodInfo();
    }

    private void H_startCookingRoutine()
    {
        if (!isCooking && H_hasFood() && trayControl.isTrayAvailable())
        {
            // 큐 내부에서 맨 앞에 있는 값 반환
            // 수정 1. 하나의 값만 받는 것이 아니라 트레이의 빈 칸만큼 받기(peek가 아니라 dequeue로 빼게 된다)
            // FoodData foodData = H_cookingList.Peek();
            List<OrderData> orderDatas = new List<OrderData>();
            int trayEmptyCount = trayControl.GetTrayEmptyCount();
            Debug.Log("현재 트레이 빈 칸 수: " + trayEmptyCount);

            for (int i = 0; i < trayEmptyCount; i++)
            {
                if (H_cookingList.Count > 0)
                {
                    orderDatas.Add(H_cookingList.Dequeue());
                }
                else
                {
                    break;
                }
            }

            StartCoroutine(cookingRoutine(orderDatas));
        }
    }

    // FoodData -> OrderData로 변경
    private IEnumerator cookingRoutine(List<OrderData> orderDatas)
    {
        isCooking = true; 

        // 조리 시작
        H_startCooking(orderDatas);

        // 조리시간만큼 기다리기
        yield return new WaitForSeconds(cookTime);

        // 트레이에 음식 올리기
        // 수정 2. 동시에 조리한 모든 음식이 올라가야 함
        H_placeFoodOnTray(orderDatas);

        // 조리 완료된 음식 큐에서 제거
        // 수정 3. 처음 큐에서 조리할 음식을 모두 꺼내서 필요없는 작업이 됨.
        // H_cookingList.Dequeue();

        // 조리 루틴 끝
        isCooking = false;
    }

    // 음식 조리하기
    // 셰프가 주방에 오더하는 소리나는 기능 추가 필요
    private void H_startCooking(List<OrderData> orderDatas)
    {
        foreach (var orderData in orderDatas)
        {
            Debug.Log("조리 시작 " + orderData.foodData.foodName);

        }
    }

    // 트레이에 음식 올리기
    // 트레이에 주문 정보 전달
    // ** 함수의 필요성 재고 필요 **
    private void H_placeFoodOnTray(List<OrderData> orderDatas)
    {
        foreach(var orderData in orderDatas)
        {
            trayControl.GetOrderInfo(orderData);

            Debug.Log("조리 완료 " + orderData.foodData.foodName);
        }
    }

    // 큐에 음식이 있는지 확인
    // FoodData -> OrderData로 변경
    private bool H_hasFood() => H_cookingList.Count > 0;

    // (임시) 음식 추가용 함수
    private void TestGetFood()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FoodData food = foodDB.GetFoodData(1);
            List<FoodData>[] tmplist = new List<FoodData>[] { new List<FoodData> { foodDB.GetFoodData(2) },  new List<FoodData> { foodDB.GetFoodData(3) }, null, null };
            H_GetOrderInfo(tmplist, 4);
            npc.B_GetTableInfo(tmplist, 4);
        }
    }
    public void GetFoodDataFromOrderMemo(List<FoodData>[] foodDatas, int tableNum)
    {
        H_GetOrderInfo(foodDatas, tableNum);
        npc.B_GetTableInfo(foodDatas, tableNum);
    }
    // (임시) 테이블 위 음식 초기화용 함수
    private void ClearTableFoodInfo()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            H_ClearTableInfo(4);
            npc.B_ClearTableInfo(4);
        }
    }

    // VisitorOrder에서 받아오기
    private void H_GetOrderInfo(List<FoodData>[] foodDatas, int tableNum)
    {
        // foodDatas는 의자 수 크기의 배열
        int chairCount = tableManager.GetTable(tableNum).chairNum;
        int chairIdx = 0;

        foreach (var foodList in foodDatas)
        {
            if (foodList == null)
            {
                chairIdx++;
                if (chairIdx >= chairCount)
                    return;
            }
            else
            {
                // 조리할 음식 리스트에 추가a
                foreach (var foodData in foodList)
                {
                    foodDatasOnTable[tableNum][chairIdx].Add(foodData);
                    H_cookingList.Enqueue(new OrderData(foodData, tableNum));
                    Debug.Log("조리할 음식 추가" + foodData.foodName);
                }
                chairIdx++;
                if (chairIdx >= chairCount)
                    return;
            }
        }
    }

    // 손님이 일어날 때 해당 테이블의 음식 정보 초기화
    // 손님이 일어나는 타이밈을 결정하고 해당 동작을 하는 스크립트에서 호출해서 실행해주면 될 듯
    // NPC.cs에 대해서도 동일한 함수 동시 실행
    public void H_ClearTableInfo(int tableNum)
    {
        int chairCount = tableManager.GetTable(tableNum).chairNum;
        for (int i = 0; i < chairCount; i++)
        {
            foodDatasOnTable[tableNum][i].Clear();
        }
    }
}
