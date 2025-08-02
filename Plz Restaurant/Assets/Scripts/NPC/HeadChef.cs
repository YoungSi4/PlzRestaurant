using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeadChef : MonoBehaviour
{
    // 현재 조리중인 주문서 목록
    private Queue<FoodData> H_cookingList = new Queue<FoodData>();
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
            FoodData foodData = H_cookingList.Peek();
            StartCoroutine(cookingRoutine(foodData));
        }
    }

    private IEnumerator cookingRoutine(FoodData foodData)
    {
        isCooking = true; 

        // 조리 시작
        H_startCooking(foodData);

        // 조리시간만큼 기다리기
        yield return new WaitForSeconds(cookTime);

        // 트레이에 음식 올리기
        H_placeFoodOnTray(foodData);

        // 조리 완료된 음식 큐에서 제거
        H_cookingList.Dequeue();

        // 조리 루틴 끝
        isCooking = false;
    }

    // 음식 조리하기
    // 셰프가 주방에 오더하는 소리나는 기능 추가 필요
    private void H_startCooking(FoodData foodData)
    {
        // 오류검사
        if (foodData == null)
        {
            Debug.Log("FoodData 접근 오류(NULL)");
            return;
        }

        Debug.Log("조리 시작 " + foodData.foodName);
    }

    // 트레이에 음식 올리기
    private void H_placeFoodOnTray(FoodData foodData)
    {
        // 올릴 트레이 위치 정하기
        int posNum = trayControl.selectTrayPosition();

        // 트레이 위치에 생성, 주문정보 전달(OrderData 클래스의 변수로 전달 - 현재 구현x)
        trayControl.SpawnFoodOnTray(foodData, posNum, null);

        Debug.Log("조리 완료 " + foodData.foodName);
    }

    // 큐에 음식이 있는지 확인
    private bool H_hasFood() => H_cookingList.Count > 0;

    // 임시 음식 추가용 함수
    private void GetFood()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FoodData food = foodDB.GetFoodData(1);
            H_cookingList.Enqueue(food);
            Debug.Log("조리할 음식 추가");
        }
    }
}
