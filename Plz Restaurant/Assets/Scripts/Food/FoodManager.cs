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

    // ť�� ���� �ֱ�
    public void AddCompletedFood(FoodData food)
    {
        completedFood.Enqueue(food);
    }

    // ť�� ������ �ִ��� Ȯ��
    public bool HasFood() => completedFood.Count > 0;

    public FoodData GetNextFood()
    {
        return HasFood() ? completedFood.Dequeue() : null;
    }
}
