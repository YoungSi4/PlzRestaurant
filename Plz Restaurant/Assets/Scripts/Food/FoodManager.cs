using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    private Queue<FoodData> completedFood = new Queue<FoodData>();
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
}
