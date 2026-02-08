using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrayControl : MonoBehaviour
{
    // 강화될 트레이의 프리팹 저장
    [SerializeField] 
    private GameObject[] trayPrefabs = new GameObject[4];
    // 현재 레벨에서 사용중인 트레이 오브젝트 저장
    private GameObject currentTray;
    // 트레이에 음식을 올릴 위치 저장
    public List<Transform> foodPositions = new List<Transform>();
    // 현재 트레이에 올라가있는 음식 오브젝트 저장
    // .Add(null)로 크기 늘리기
    private List<GameObject> foodObjs = new List<GameObject>() { null, null };

    // 트레이에 올라간 음식의 주문 정보 저장
    private List<OrderData> trayOrderDatas = new List<OrderData>();

    // 트레이에 음식이 올라간 순서 저장
    // 서빙할 때 음식을 챙겨야할 순서를 관리하기 위한 큐
    // 트레이 인덱스를 저장해서 순차적으로 서빙할 수 있도록 함
    private Queue<int> trayIndexTurns = new Queue<int>();

    private NPC npc;

    // [기현상 관련]
    private bool isListeningForAnomaly = false;
    private int anomalyFirstTableNum = -1;

    void Start()
    {
        SetTray(0);

        npc = FindObjectOfType<NPC>();
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

    // 주문 데이터 가져오기
    // HeadChef.cs 에서 호출
    public void GetOrderInfo(OrderData order)
    {
        SpawnFoodOnTray(order, selectTrayPosition());
    }


    // 트레이에 음식 올리기
    private  void SpawnFoodOnTray(OrderData order, int posNum)
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
        foodObjs[index] = Instantiate(order.foodData.foodPrefab, foodPositions[index].position, foodPositions[index].rotation);
        // 트레이에 음식이 올라간 위치에 주문 정보 저장
        trayOrderDatas[index] = order;
        // 트레이 인덱스 큐에 추가 (서빙 순서 관리)
        trayIndexTurns.Enqueue(posNum);

        // [이상현상] Listen 상태에서 다른 테이블 음식이 올라왔는지 체크
        if (isListeningForAnomaly && order.tableNum != anomalyFirstTableNum)
        {
            // 새로 올라온 이 테이블(order.tableNum)에 이상현상 발동
            npc.SetAnomalyActive(order.tableNum);
            isListeningForAnomaly = false;
            Debug.Log($"[TrayControl] Listen 완료: {order.tableNum}번 테이블(두 번째) 이상현상 발동");
        }
    }

    // 트레이에 음식을 올릴 위치 결정 (앞에서부터 빈자리가 있으면 바로바로 채우기)
    // HeadChef.cs에서 호출
    private int selectTrayPosition()
    {
        for (int i = 0; i < foodObjs.Count; i++)
        {
            if (foodObjs[i] == null)
                return i + 1;
        }
        return 0; // 꽉 찼으면 0 반환
    }

    // 트레이에서 사장님 손에 올리기위해 음식 오브젝트 정보 전달
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
    // 트레이에서 사장님 손에 올리기위해 주문 정보 전달
    // NPC.cs 에서 호출
    public OrderData GetOrderData(int trayIndex)
    {
        int index = trayIndex - 1; // 0부터 시작하는 인덱스
        if (index >= 0 && index < trayOrderDatas.Count)
        {
            return trayOrderDatas[index];
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
            trayOrderDatas[index] = null; // 주문 정보도 삭제
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

    // 트레이의 빈자리 수 반환
    // HeadChef.cs에서 호출
    public int GetTrayEmptyCount()
    {
        int count = 0;
        foreach (var foodObj in foodObjs)
        {
            if (foodObj == null) count++;
        }
        return count;
    }

    // 트레이 인덱스 큐에서 서빙할 음식의 트레이 인덱스(위치)를 가져옴
    // NPC.cs에서 호출
    public int PeekPickIndex() => trayIndexTurns.Count > 0 ? trayIndexTurns.Peek() : 0;

    // 음식을 Pick하는데 성공하면 트레이 인덱스 큐에서 해당 인덱스를 제거
    // NPC.cs에서 호출
    public void ConfirmPickIndex()
    {
        if (trayIndexTurns.Count > 0) trayIndexTurns.Dequeue();
    }

    // --------------------- 기현상 관련 -------------------
    // 기현상 발생 요청받기
    // NPC.cs에서 호출
    public void RequestAnomalyInfo()
    {
        if (trayOrderDatas.Count == 0) return;

        int firstTable = GetFirstPriorityTableNum();
        int secondTable = GetSecondPriorityTableNum(firstTable);

        if (secondTable != -1)
        {
            // 트레이에 이미 다른 테이블 음식이 있음 - 즉시 NPC에게 알림
            npc.SetAnomalyActive(secondTable);
            Debug.Log($"[TrayControl] 즉시 이상현상 발동: {secondTable}번 테이블");
        }
        else
        {
            // 아직 다른 테이블 음식이 없음 - Listen 상태 시작
            isListeningForAnomaly = true;
            anomalyFirstTableNum = firstTable;
            Debug.Log($"[TrayControl] Listen 시작: {firstTable}번 외 다른 테이블 주문 대기중");
        }
    }

    // 트레이에서 최우선 테이블 번호 반환
    private int GetFirstPriorityTableNum()
    {
        // trayIndexTurns의 첫 번째 인덱스에 해당하는 주문의 테이블 번호
        if (trayIndexTurns.Count > 0)
        {
            int firstIndex = trayIndexTurns.Peek() - 1;
            if (trayOrderDatas[firstIndex] != null)
            {
                return trayOrderDatas[firstIndex].tableNum;
            }
        }
        return -1;
    }

    // 첫 번째 테이블과 다른 테이블 번호 반환 (없으면 -1)
    private int GetSecondPriorityTableNum(int excludeTableNum)
    {
        foreach (var orderData in trayOrderDatas)
        {
            if (orderData != null && orderData.tableNum != excludeTableNum)
            {
                return orderData.tableNum;
            }
        }
        return -1;
    }
}