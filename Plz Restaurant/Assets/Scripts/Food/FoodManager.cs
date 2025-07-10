using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    [SerializeField] private int foodNum; // 주문이 들어온 음식의 인덱스(임시로 직렬화 사용중)
    private float cookTime = 1f; // 음식 조리에 걸리는 시간(임시) 
    private Queue<FoodData> completedFood = new Queue<FoodData>();

    private FoodDB foodDB;

    private void Start()
    {
        foodDB = FindObjectOfType<FoodDB>();
    }
    public override void Awake()
    {
        base.Awake();
    }
    // 큐에 음식 넣기
    public void AddCompletedFood(FoodData food)
    {
        completedFood.Enqueue(food);
    }

    // 큐에 음식이 있는지 확인
    public bool HasFood() => completedFood.Count > 0;

    public FoodData GetNextFood()
    {
        return HasFood() ? completedFood.Dequeue() : null;
    }

    private void Update()
    {
        // 테스트용 큐에 음식 넣기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int testFoodNum = 1; // 테스트할 음식 인덱스 번호
            CookFood(testFoodNum);
            Debug.Log("음식 조리 시작: " + testFoodNum + "번");
        }
    }
    // 음식 조리하기
    public void CookFood(int foodNum)
    {
        StartCoroutine(CookFoodCoroutine(foodNum));
    }
    IEnumerator CookFoodCoroutine(int foodNum)
    {
        FoodData food = foodDB.GetFoodData(foodNum);

        if (food == null)
        {
            Debug.Log("음식 정보 Null 오류. foodNum = " + foodNum);
            yield break;
        }
        
        // 조리시간만큼 기다리기
        // 조리시간이 음식마다 다르다면 FoodData에 해당 변수(cookTime) 추가해도 될듯?
        yield return new WaitForSeconds(cookTime);

        AddCompletedFood(food);
        Debug.Log("조리 완료 " + foodNum + "번");
    }

}
