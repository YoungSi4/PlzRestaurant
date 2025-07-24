using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrayControl : MonoBehaviour
{
    // 현재 트레이에 올라가있는 음식 오브젝트 저장
/*    private GameObject foodObj1;
    private GameObject foodObj2;*/
    private GameObject[] foodObjs = new GameObject[2];
    // 트레이에 음식을 올릴 위치 저장
/*    public Transform foodPos1;
    public Transform foodPos2;*/
    public Transform[] foodPositions = new Transform[2];

    // 트레이에 음식 올리기
    // HeadChef.cs 에서 호출
    public void SpawnFoodOnTray(FoodData food, int posNum)
    {
        // foodPos의 위치에 food의 3D오브젝트 생성
/*        switch (posNum)
        {
            // 1번 위치
            case 1:
                foodObj1 = Instantiate(food.foodPrefab, foodPos1.position, foodPos1.rotation);
                break;
            // 2번 위치
            case 2:
                foodObj2 = Instantiate(food.foodPrefab, foodPos2.position, foodPos2.rotation);
                break;
        }*/

        int index = posNum - 1; // 0부터 시작하는 인덱스
        foodObjs[index] = Instantiate(food.foodPrefab, foodPositions[index].position, foodPositions[index].rotation);   
    }

    // 트레이에 음식을 올릴 위치 결정
    // HeadChef.cs에서 호출
    public int selectTrayPosition()
    {
/*        // 1번 위치가 비었으면 우선적으로 1번 위치에 올림
        if (foodObj1 == null) return 1;
        // 1번위치가 비어있지 않아 첫 번째 조건문을 통과했는데 2번 위치가 비었으면 2번 위치에 올림
        if (foodObj2 == null) return 2;*/
        if(foodObjs[0] == null) return 1; // 1번 위치가 비어있으면 1번 위치에 올림
        if(foodObjs[1] == null) return 2; // 2번 위치가 비어있으면 2번 위치에 올림
        // 빈 트레이 위치가 없다면 0을 반환
        return 0;
    }

    // 트레이에서 사장님 손에 올리기위해 음식 정보 전달
    // NPC.cs 에서 호출
    public GameObject TakeFoodFromTray(int trayIndex, Transform handPos)
    {
/*        GameObject B_handFood = null;

        switch (trayIndex)
        {
            case 1:
                if (foodObj1 != null)
                {
                    B_handFood = Instantiate(foodObj1, handPos.position, handPos.rotation);
                }
                break;
            case 2:
                if (foodObj2 != null)
                {
                    B_handFood = Instantiate(foodObj2, handPos.position, handPos.rotation);
                }
                break;
        }
        return B_handFood;*/

        int index = trayIndex - 1; // 0부터 시작하는 인덱스
        if (foodObjs[index] != null)
        {
            return Instantiate(foodObjs[index], handPos.position, handPos.rotation);
        }
        return null;
    }
    // 트레이에서 음식 삭제
    // NPC.cs 에서 호출
    public void ClearFood(int trayIndex)
    {
/*        switch (trayIndex)
        {
            case 1:
                if (foodObj1 != null)
                {
                    Destroy(foodObj1);
                    foodObj1 = null;
                }
                break;
            case 2:
                if (foodObj2 != null)
                {
                    Destroy(foodObj2);
                    foodObj2 = null;
                }
                break;
        }*/

        int index = trayIndex - 1; // 0부터 시작하는 인덱스
        if(foodObjs[index] != null)
        {
            Destroy(foodObjs[index]);
            foodObjs[index] = null;
        }
    }
    // 트레이에 빈 자리가 있는지 검사
    // HeadChef.cs에서 호출
    public bool isTrayAvailable()
    {
       // return foodObj1 == null || foodObj2 == null;
       return foodObjs[0] == null || foodObjs[1] == null;
    }

    // 트레이에 올라온 음식이 있는지 검사
    // NPC.cs에서 호출
    public bool isFoodOnTray()
    {
       // return foodObj1 != null || foodObj2 != null;
       return foodObjs[0] != null || foodObjs[1] != null;
    }

    // 트레이 1번 위치가 비어있는지 검사
    // NPC.cs에서 호출
    public bool isTrayFirstSlotEmpty()
    {
        //return foodObj1 == null;
        return foodObjs[0] == null;
    }
    // 트레이 2번 위치가 비어있는지 검사
    // NPC.cs에서 호출
    public bool isTraySecondSlotEmpty()
    {
        //return foodObj2 == null;
        return foodObjs[1] == null;
    }
}
