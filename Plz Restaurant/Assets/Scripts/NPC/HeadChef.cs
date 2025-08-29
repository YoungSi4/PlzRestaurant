using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderData
{
    public FoodData foodData { get; private set; } // 음식 데이터
    public float seatNum { get; private set; } // 좌석 번호 (테이블 번호를 포함한 플롯형)

    public OrderData(FoodData foodData, float seatNum)
    {
        this.foodData = foodData;
        this.seatNum = seatNum;
    }
}

public class HeadChef : MonoBehaviour
{
/*    // 현재 조리중인 주문서 목록
    private Queue<FoodData> H_cookingList = new Queue<FoodData>();*/
    // 현재 조리중인 주문서 목록 (OrderData 버전)
    private Queue<OrderData> H_cookingList = new Queue<OrderData>();

    private float cookTime = 1f; // 음식 조리에 걸리는 시간(5초로 수정 예정)
    private bool isCooking = false;

    private FoodDB foodDB;
    private TrayControl trayControl;

    private void Start()
    {
        foodDB = FindObjectOfType<FoodDB>();
        trayControl = FindObjectOfType<TrayControl>();
    }

    private void Update()
    {
        H_startCookingRoutine();
        GetFood();
    }

    private void H_startCookingRoutine()
    {
        if (!isCooking && H_hasFood() && trayControl.isTrayAvailable())
        {
            // 큐 내부에서 맨 앞에 있는 값 반환
            // 수정 1. 하나의 값만 받는 것이 아니라 트레이의 빈 칸만큼 받기(peek가 아니라 dequeue로 빼게 된다)
            // FoodData foodData = H_cookingList.Peek();
            // foodData -> OrderData로 변경
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
    // FoodData -> OrderData로 변경
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
    // FoodData -> OrderData로 변경
    private void H_placeFoodOnTray(List<OrderData> orderDatas)
    {
        foreach(var orderData in orderDatas)
        {
            /*// 올릴 트레이 위치 정하기
            int posNum = trayControl.selectTrayPosition();

            trayControl.SpawnFoodOnTray(orderData, posNum);*/
            // TrayControl로 데이터만 전달하고 TrayControl에서 음식 올리는 동작하도록 수정 (아래)
            trayControl.GetOrderInfo(orderData);

            Debug.Log("조리 완료 " + orderData.foodData.foodName);
        }
    }

    // 큐에 음식이 있는지 확인
    // FoodData -> OrderData로 변경
    private bool H_hasFood() => H_cookingList.Count > 0;

    // 임시 음식 추가용 함수
    // FoodData -> OrderData로 변경
    private void GetFood()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*            for(int i=0;i<3;i++)
                        {
                            FoodData food = foodDB.GetFoodData(1);
                            H_cookingList.Enqueue(new OrderData(food, 1.1f));
                            Debug.Log("조리할 음식 추가");
                        }*/
            FoodData food = foodDB.GetFoodData(1);
            H_cookingList.Enqueue(new OrderData(food, 7.1f));
            Debug.Log("조리할 음식 추가 1");

            food = foodDB.GetFoodData(1);
            H_cookingList.Enqueue(new OrderData(food, 7.2f));
            Debug.Log("조리할 음식 추가 2");
        }
    }

    // 주문 정보 가져오기
    // 흐름대로 OrderMemo.cs에서 호출해야 한다면 tableNum이 아닌 SeatNum을 여기까지 전달해와야 함
    public void H_GetOrderInfo(FoodData foodData, float SeatNum)
    {
        OrderData orderData = new OrderData(foodData, SeatNum);
        H_cookingList.Enqueue(orderData);
        Debug.Log("조리할 음식 추가");
    }
}
