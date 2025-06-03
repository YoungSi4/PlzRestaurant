using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayControl : MonoBehaviour
{
    // 트레이에 음식을 올릴 위치 저장
    public Transform foodPos1;
    public Transform foodPos2;
    // 현재 트레이에 올라가있는 음식 오브젝트 저장
    public GameObject foodObj1;
    public GameObject foodObj2;

    void Update()
    {
        // 테스트용 음식추가 로직
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FoodData food = Resources.Load<FoodData>("GameObject/Food/Ramen");
            FoodData food2 = Resources.Load<FoodData>("GameObject/Food/Spaghetti");

            int ran = Random.Range(0, 2);
            if (food != null && food2 != null)
            {
                if (ran == 0)
                {
                    FoodManager.Instance.AddCompletedFood(food);
                    Debug.Log("음식추가1번");
                }
                if (ran == 1)
                {
                    FoodManager.Instance.AddCompletedFood(food2);
                    Debug.Log("음식추가2번");
                }
            }
            else Debug.Log("Null오류");

        }
        // 큐에 음식이 있으면서 트레이에 빈자리가 있으면 트레이에 음식을 채우기 위해 계속 확인
        TryUpdateTray();
    }
    void TryUpdateTray()
    {
        // 1번 위치에 올라간 음식이 없고 큐에 완성된 음식이 남아있을 때
        if (foodObj1 == null && FoodManager.Instance.HasFood())
        {
            // 큐에서 음식을 꺼냄
            FoodData food1 = FoodManager.Instance.GetNextFood();
            // 음식을 트레이의 지정 위치에 올림
            foodObj1 = SpawnFoodOnTray(food1, foodPos1);
        }
        // 2번 위치에 올라간 음식이 없고 큐에 완성된 음식이 남아있을 때
        if (foodObj2 == null && FoodManager.Instance.HasFood())
        {
            // 큐에서 음식을 꺼냄
            FoodData food2 = FoodManager.Instance.GetNextFood();
            // 음식을 트레이의 지정 위치에 올림
            foodObj2 = SpawnFoodOnTray(food2, foodPos2);
        }
    }

    GameObject SpawnFoodOnTray(FoodData food,  Transform foodPos)
    {
        // foodPos의 위치에 food의 3D오브젝트 생성
        GameObject instantFood = Instantiate(food.foodPrefab, foodPos.position, foodPos.rotation);
        return instantFood;
    }
    
    // 트레이에서 음식 삭제
    // NPC.cs 에서 호출
    public void ClearFood(int foodIndex)
    {
        if(foodIndex == 1)
        {
            if (foodObj1 != null)
            {
                Destroy(foodObj1);
                foodObj1 = null;
            }
        }
        else if(foodIndex == 2)
        {
            if (foodObj2 != null)
            {
                Destroy(foodObj2);
                foodObj2 = null;
            }
        }

    }
}
