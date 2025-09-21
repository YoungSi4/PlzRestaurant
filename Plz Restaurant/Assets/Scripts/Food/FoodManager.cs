using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
/*    [SerializeField] private int foodNum; // 주문이 들어온 음식의 인덱스(임시로 직렬화 사용중)
    private Queue<FoodData> completedFood = new Queue<FoodData>();

    private FoodDB foodDB;
    private HeadChef headChef;
    private void Start()
    {
        headChef = FindObjectOfType<HeadChef>();
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
            headChef.H_startCooking(testFoodNum);
            Debug.Log("음식 조리 시작: " + testFoodNum + "번");
        }
    }

*/
}
