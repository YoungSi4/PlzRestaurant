using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class FoodDB : MonoBehaviour
{
    // 유니티 상에서 Scriptable Object로 하나하나 넣어주면 됨
    public FoodData[] foodDatas;

    // 위 데이터를 컨테이너에 저장
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
