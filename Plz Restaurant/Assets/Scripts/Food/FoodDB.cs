using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class FoodDB : MonoBehaviour
{
    // ����Ƽ �󿡼� Scriptable Object�� �ϳ��ϳ� �־��ָ� ��
    public FoodData[] foodDatas;

    // �� �����͸� �����̳ʿ� ����
    private Dictionary<int, FoodData> foodDB;

    void Awake()
    {
        foodDB = new Dictionary<int, FoodData>();
        foreach (var food in foodDatas)
        {
            foodDB.Add(food.foodNum, food);
        }
    }

    public FoodData GetFoodData(int foodNum)
    {
        FoodData targetFood = foodDB[foodNum];
        return targetFood;
    }

}
