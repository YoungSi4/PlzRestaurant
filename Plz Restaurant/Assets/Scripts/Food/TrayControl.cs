using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 수도코드 작성을 위한 임시클래스
public class OrderData
{
    
}

public class TrayControl : MonoBehaviour
{
    // 현재 트레이에 올라가있는 음식 오브젝트 저장
    // .Add(null)로 크기 늘리기
    private List<GameObject> foodObjs = new List<GameObject>() { null, null };
    // 강화될 트레이의 프리팹 저장
    [SerializeField] 
    private GameObject[] trayPrefabs = new GameObject[4];
    // 현재 레벨에서 사용중인 트레이 오브젝트 저장
    private GameObject currentTray;
    // 트레이에 음식을 올릴 위치 저장
    public List<Transform> foodPositions = new List<Transform>();

    // 트레이에 올라간 음식의 주문 정보 저장
    private List<OrderData> trayOrderDatas = new List<OrderData>();

    void Start()
    {
        SetTray(0);
    }

    private void SetTray(int trayLevel)
    {

        if (currentTray != null)
        {
            // 기존 트레이 제거
            Destroy(currentTray); 
        }

        // 트레이 생성 (trayLevel은 0부터 시작)
        currentTray = Instantiate(trayPrefabs[trayLevel], this.transform);


        foodPositions.Clear();
        foodObjs.Clear();
        trayOrderDatas.Clear();

        // 현재 트레이의 자식 오브젝트 중 "FoodPos" 태그를 가진 오브젝트를 찾아서 리스트에 추가
        foreach (Transform child in currentTray.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("FoodPos"))
            {
                foodPositions.Add(child);
                foodObjs.Add(null); // 트레이에 음식이 올라갈 위치를 초기화
                trayOrderDatas.Add(null); // 트레이에 음식이 올라간 위치에 주문 정보 초기화
            }
        }

    }

    public void UpgradeTray(int trayLevel)
    {
        trayLevel++;

        // 트레이 레벨이 유효한지 검사(0보다 작거나 트레이 프리팹 배열의 길이보다 크면 오류)
        if (trayLevel < 0 || trayLevel >= trayPrefabs.Length)
        {
            Debug.LogError("Invalid tray level: " + trayLevel);
            return;
        }

        // 트레이 레벨에 맞는 트레이 세팅
        SetTray(trayLevel);
    }

    // 트레이에 음식 올리기
    // HeadChef.cs 에서 호출
    public void SpawnFoodOnTray(FoodData food, int posNum, OrderData order)
    {
        // 0부터 시작하는 인덱스
        int index = posNum - 1;
        // 유효한 인덱스인지 검사
        if (index < 0 || index >= foodPositions.Count)
        {
            Debug.LogError("Invalid tray position index: " + posNum);
            return;
        }
        // 트레이에 음식 오브젝트 생성
        foodObjs[index] = Instantiate(food.foodPrefab, foodPositions[index].position, foodPositions[index].rotation);

        // 트레이에 음식이 올라간 위치에 주문 정보 저장
        trayOrderDatas[index] = order; 
    }
    
    // 트레이에 음식을 올릴 위치 결정 (앞에서부터 빈자리가 있으면 바로바로 채우기)
    // HeadChef.cs에서 호출
    public int selectTrayPosition()
    {
        for (int i = 0; i < foodObjs.Count; i++)
        {
            if (foodObjs[i] == null)
                return i + 1;
        }
        return 0; // 꽉 찼으면 0
    }

    // 트레이에서 사장님 손에 올리기위해 음식 정보 전달
    // NPC.cs 에서 호출
    public GameObject TakeFoodFromTray(int trayIndex, Transform handPos)
    {
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
        int index = trayIndex - 1; // 0부터 시작하는 인덱스
        if(foodObjs[index] != null)
        {
            Destroy(foodObjs[index]);
            foodObjs[index] = null;
        }
    }
    // 트레이에 빈 자리가 있는지 검사
    // HeadChef.cs에서 호출
    public bool isTrayAvailable() => foodObjs[0] == null || foodObjs[1] == null;

    // 트레이에 올라온 음식이 있는지 검사
    // NPC.cs에서 호출
    public bool isFoodOnTray() => foodObjs[0] != null || foodObjs[1] != null;

    // 트레이 1번 위치가 비어있는지 검사
    // NPC.cs에서 호출
    public bool isTrayFirstSlotEmpty() => foodObjs[0] == null;

    // 트레이 2번 위치가 비어있는지 검사
    // NPC.cs에서 호출
    public bool isTraySecondSlotEmpty() => foodObjs[1] == null;
}
