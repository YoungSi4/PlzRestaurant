using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/***********************************************************************************
 * 테이블 객체 : 테이블의 상태를 관리하고 타 스크립트에서 이용할 목적의 자료구조?
 * 멤버 변수 : 테이블 번호, 사용 중 여부, 의자 갯수, 각 의자 상태, 각 의자 좌표
 * 멤버 함수 : 생성자 (기본, 매개변수), getter setter, VisitorSit, VisitorStandUp
 ************************************************************************************/

// table 객체는 일종의 노드에 가깝고, 이걸 배열에 넣고 관리할 객체 -> 테이블 매니저
// 트리와 노드의 관계를 연상하면 될 듯?

public class Table : MonoBehaviour
{
    public int tableNum;
    [SerializeField]
    public int chairNum { get; private set; } // 굳이 필요할까? chairPos의 길이로 접근해도 되잖아.
    public bool isTableOccupied { get; private set; }

    // 손님 관련 변수
    public Visitor[] visitorOnChair { get;  private set; } // 각 의자에 앉은 손님 객체를 저장
    [SerializeField]
    public Transform[] chairPos; // 각 의자의 위치
    public int visitorNum { get;  set; } // 손님의 수
    private List<Coroutine> visitorAngry;

    [SerializeField]
    private Collider visitorCheckCollider; // 손님 검사
    private WaitForSeconds inspectionDelay; // 손님이 테이블 근처 도달했는지 확인하는 시간 딜레이
    private float delay = 5f;
    public bool IsWaitingForVisitorArrived { get;  set; } // 배정된 손님을 기다리는 중인지
    private WaitForSeconds orderDelay = new WaitForSeconds(5f);

    // 주문 상호작용 관련 변수
    [SerializeField]
    private GameObject readyToOrderIconPrefab; // 컴포넌트 상에서 연결한 자식 오브젝트
    public bool isReadyToOrder { get; private set; } // true 일 때만 상호작용 가능

    // 음식의 수
    private int orderedFoodNum = 0;

    // 음식 둘 위치 : 각 테이블마다 지정?
    [SerializeField]
    private Transform[] foodPos;
    public Transform[] foodPosPointer => foodPos;

    // 올라간 음식 오브젝트 저장
    private List<GameObject> placedFoodObjects = new List<GameObject>();

    // waitForEating 관련
    public bool isCheckingEating { get; private set; }
    private float eatingCheckTime = 5f;
    private WaitForSeconds eatingCheckDelay;
    private Coroutine eatingCoroutine;

    // 돈 오브젝트 생성 관련
    [SerializeField]
    private GameObject moneyPrefab;
    private int totalPrice = 0;
    private Vector3 moneyGenPos;

    // [기현상 관련]
    private bool hasAnomaly = false; // 테이블에 대한 이상 현상 발생 여부
    public bool HasAnomaly => hasAnomaly;
    private FoodData[] anomalyOrderedFoods;

    private NPC npc;
    private Minigame1 minigame1;

    private void Awake()
    {
        chairNum = chairPos.Length;
        npc = FindObjectOfType<NPC>();
        minigame1 = FindObjectOfType<Minigame1>();
    }

    private void Start()
    {
        visitorOnChair = new Visitor[chairNum];
        inspectionDelay = new(delay);
        eatingCheckDelay = new(eatingCheckTime);
        IsWaitingForVisitorArrived = false;
        visitorNum = 0;
        isReadyToOrder = false;
        isCheckingEating = false;

        SetMoneyGenPos();
    }

    private void SetMoneyGenPos()
    {
        var tempPos = transform.position;
        tempPos.y += 1;
        moneyGenPos = tempPos;
    }

    /// <summary>
    ///  손님이 떠난 후 변수 초기화하는 함수
    ///  
    ///  초기화 하는 변수 목록
    ///  visitorOnChair
    ///  isTableOccupied
    ///  IsWaitingForVisitorArrived
    ///  visitorNum
    ///  isReadyToOrder
    /// </summary>
    private void ResetVars()
    {
        for (int i = 0; i < chairNum; i++)
        {
            visitorOnChair[i] = null;
        }

        isTableOccupied = false;
        IsWaitingForVisitorArrived = false;
        visitorNum = 0;
        isReadyToOrder = false;
        orderedFoodNum = 0;
    }

    public void SetTotalPrice(int price)
    {
        totalPrice = price;
    }

    // 테이블에 놓인 음식 정보도 저장해야하나?

    //// constructor
    //public Table()
    //{
    //    this.tableNum = -1;
    //    this.isTableOccupied = false;
    //    this.chairNum = -1;
    //    this.visitorIDOnChair = new int[chairNum];
    //}

    //public Table(int tableNum, bool isTableOccupied, int chairNum)
    //{
    //    this.tableNum = tableNum;
    //    this.isTableOccupied = isTableOccupied;
    //    this.chairNum = chairNum;
    //    this.visitorIDOnChair = new int[chairNum];
    //}


    /// <summary>
    /// 의자에 손님 정보를 세팅하는 함수
    /// 손님 수만큼 반복해야 한다
    /// </summary>
    /// <param name="index">
    /// 앉는 의자 번호
    /// </param>
    /// <param name="visitor">
    /// 앉는 손님 객체 정보
    /// </param>
    public void VisitorSitOnChair(int index, Visitor visitor)
    {
        isTableOccupied = true;
        visitorOnChair[index] = visitor;
        visitor.SetSeatNum(index);
        IsWaitingForVisitorArrived = true; // 배정된 손님이 도착했는지 기다리는 중
        visitorNum++;
    }

    // 의자에 저장해둔 손님 정보 초기화
    public void VisitorStandUpChair()
    {
        isTableOccupied = false;
        for(int i = 0;  visitorOnChair.Length > i ; i++)
        {
            visitorOnChair[i] = null;
        }
        visitorNum = 0;
        visitorCheckCollider.isTrigger = true; // 다시 손님 받을 준비
    }

    /// <summary>
    /// 손님이 테이블에 도착했는지 확인하는 trigger 함수
    /// </summary>
    /// <returns></returns>
    private void OnTriggerEnter(Collider other)
    {
        // 테이블에 오고 있는 손님이 없다면 return
        if (!isTableOccupied) return;
        
        var obj = other.gameObject.GetComponent<Visitor>();
        if (obj == null) return; // 닿은 물체가 손님이 아니라면 return

        Debug.Log(tableNum + "번 테이블 근처의 손님 ID: " + obj.C_ID);

        // 테이블 근처의 손님이 이 테이블에 지정된 손님인지
        bool isRightVisitor = false;

        // 테이블에 입력된 손님 정보를 순회
        foreach(var visitor in visitorOnChair)
        {
            if (obj == visitor)
            {
                isRightVisitor = true;
                break;
            }
        }

        if (isRightVisitor)
        {
            visitorCheckCollider.isTrigger = false; // 주문 대기부터는 잠시 collider를 꺼둔다
            StartCoroutine(WaitingForOrder());
        }
    } // onTriggerEnter -end-

    // 이 위로는 맞는 손님인지 확인하는 플로우일 뿐

    /// <summary>
    /// 주문 시작하는 함수!!!
    /// 여기서 주문 플로우 시작
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitingForOrder()
    {
        int randInt = Random.Range(5, 11); // 5 ~ 10
        orderDelay = new WaitForSeconds(randInt);

        yield return orderDelay;

        // 손님이 주문할 음식 번호 종합해서 전달하는 함수
        // 고민 : table이 정보를 종합적으로 가지고 있을 것인가
        // 혹은 전달만 할 것인가?
        // 전자 - 음식 놓을 때 활용 가능
        // 후자 - 캡슐화가 잘 됨

        ReadyToOrder();
    }

    // 아이콘 띄우는 함수
    private void ReadyToOrder()
    {
        isReadyToOrder = true;
        readyToOrderIconPrefab.SetActive(true);

        visitorAngry = new List<Coroutine>();
        foreach (var visitor in visitorOnChair)
        {
            if (visitor == null) continue;
            visitorAngry.Add(StartCoroutine(visitor.Angry()));
        }
    }

    /// <summary>
    /// 주문 접수 받았을 때 테이블 동작 함수
    /// </summary>
    private void OrderAccepted()
    {
        readyToOrderIconPrefab.SetActive(false);
        isReadyToOrder = false;
    }

/// <summary>
/// 해당 테이블에 앉은 손님이 주문한 음식 ID를 넘기는 함수
/// 플레이어의 상호작용 키에 의해 작동된다
/// </summary>
/// <returns>
/// List<int> - 의자 번호 순서대로 저장
/// </returns>
public List<int>[] SendFoodNumToOrderInfo()
{
        if (!isReadyToOrder) return null;  // 주문할 준비가 안 됐다면 리턴

        

        List<int>[] foodIDs = new List<int>[chairNum];

        // 아이콘 끄고 isReadyToOrder false로 변경
        OrderAccepted();

        // 2중 반복이라 좀 거슬리네
        /*
         food ID 규칙
            0) null : 빈자리 - null 처리 따로 안 해주면 예외터짐
            1) -1 : 음식 없음
            2) 0 ~ n : 음식 id
         */

        foreach (var visitor in visitorOnChair)
        {
            // 쓰는 인덱스만 초기화
            if(visitor == null) continue;

            foodIDs[visitor.C_seatChairNumber] = new List<int>();
            foreach(var foodId in visitor.C_foodNumber)
            {
                foodIDs[visitor.C_seatChairNumber].Add(foodId);
                orderedFoodNum++;
            }
        }

        

        // 화내기 코루틴 종료
        // 화내기 전에 이 함수가 실행되더라도 코루틴은 여전히 실행 중 -> 따로 종료 시켜줘야함
        foreach(var co in visitorAngry)
        {
            StopCoroutine(co);
        }
        // 화내는 애니메이션 정지
        foreach (var visitor in visitorOnChair)
        {
            if(visitor == null) continue;
            visitor.CancelAngry();
        }

        return foodIDs;
    }

    /// <summary>
    /// 해당 테이블에서 식사를 시작해도 되는지 확인
    /// NPC가 테이블에 음식을 서빙 후에 검사 실행
    /// 주문한 음식의 수와 테이블에 올려진 음식의 수를 비교한다
    /// </summary>
    public void CanWeStartToEat()
    {
        if (orderedFoodNum == placedFoodObjects.Count)
        {
            foreach (Visitor v in visitorOnChair) {
                if (v == null) continue;
                StartCoroutine(v.EatingFood());
            }
        }
    }

    /// <summary>
    /// 테이블의 모든 손님이 식사를 다 했는지 확인하는 WaitForEatingCount()의 wrapper 함수
    /// 해당 비동기 함수를 Coroutine으로 관리한다
    /// 손님에서 식사를 종료하고 호출
    /// </summary>
    public void WaitingForEating()
    {
        // 식사 확인 함수를 Coroutine으로 등록하고 관리
        // 해당 테이블에서 실행 중인 루틴이 없다면 새로 실행. 이미 있다면 굳이 실행시키지 않음.
        if (eatingCoroutine == null)
        {
            eatingCoroutine = StartCoroutine(WaitForEatingCount());
        }
    }

    /// <summary>
    /// 식사 확인 비동기 루틴을 종료하는 함수
    /// WaitForEatingCount() 내부에서 모든 손님이 다 먹은 걸 확인하면 정지
    /// </summary>
    private void StopWaitngForEating()
    {   // 식사 확인 비동기 루틴을 종료
        if (eatingCoroutine == null) return;
        StopCoroutine(eatingCoroutine);
        eatingCoroutine = null;
    }

    /// <summary>
    /// visitorNum과 hasEatenCnt가 같으면 종료
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForEatingCount()
    {
        isCheckingEating = true;
        int hasEatenCnt = 0;
        while (true)
        {
            // 식사가 끝난 손님 수 체크
            Debug.Log("식사 완료 체크 : " + tableNum);
            foreach (Visitor v in visitorOnChair)
            {
                if (v == null) continue;
                if (v.hasEaten)
                {
                    hasEatenCnt++;
                }
            }
             
            // 모두 식사를 했다면
            if (hasEatenCnt == visitorNum)
            {
                Debug.Log("식사 완료 : " + tableNum);
                VisitorStandUp();
                break;
            }
            yield return eatingCheckDelay;
            hasEatenCnt = 0;
        }

        isCheckingEating = false;
        // 돈 생성 함수, 손님들 일으켜 세우고 퇴장시키는 함수.
    }

    // 손님 퇴장 프로세스 묶음 함수
    private void VisitorStandUp()
    {
        StopWaitngForEating(); // 식사 확인 코루틴 종료
        GenerateMoney(); // 비용 지불 (테이블 위에 생성)
        VisitorDeparture(); // 손님 이동
    }

    // 돈 생성
    private void GenerateMoney()
    {
        var tempMoney = Instantiate<GameObject>(moneyPrefab, moneyGenPos, Quaternion.Euler(0, 0, 0));
        tempMoney.GetComponent<Money>().Init(totalPrice, this);
    }

    // 손님 퇴장
    private void VisitorDeparture()
    {
        foreach (Visitor visitor in visitorOnChair) {
            if (visitor == null) continue;
            visitor.Departure();
        }
    } 

    public void TableCleanUp()
    {
        ResetVars();
        ClearPlacedFoodObjects();
        VisitorStandUpChair();
    }

    public void AddPlacedFoodObject(GameObject foodObject)
    {
        placedFoodObjects.Add(foodObject);
        Debug.Log("테이블에 올라간 음식 : " + foodObject);
    }

    public void ClearPlacedFoodObjects()
    {
        foreach (var foodObject in placedFoodObjects)
        {
            Destroy(foodObject);
        }
        placedFoodObjects.Clear();
    }

    private void SendVsitorChooseFoodAgain()
    {
        foreach (Visitor visitor in visitorOnChair)
        {
            if (visitor == null) continue;
            visitor.ChooseFood();
        }
    }

    /// <summary>
    /// 주문서에서 거절 선택하면 호출하는 함수
    /// </summary>
    public void OrderRejected()
    {
        float p = Random.Range(1, 10);
        // 재주문
        if (p > 5) // 원래는 3
        {
            Debug.Log("재주문");
            // 재주문을 위해 음식을 다시 정하는 함수
            SendVsitorChooseFoodAgain();
            StartCoroutine(WaitingForOrder()); // 주문 프로세스는 여기서 재시작
        }
        // 퇴장
        else
        {
            Debug.Log("퇴장");
            // 다른 함수 없이 떠나는 것만 있으면 될 것 같음
            VisitorDeparture();
            ResetVars();
            VisitorStandUpChair();
        }
    }


    // [기현상 추가]
    // 기현상 해소
    public void ResolveAnomaly()
    {
        hasAnomaly = false;
        anomalyOrderedFoods = null;

    }
    // 기현상 발생
    public void SetAnomalyServed(FoodData[] orderedFoods)
    {
        hasAnomaly = true;
        anomalyOrderedFoods = orderedFoods;
    }
    public FoodData[] GetAnomalyOrderedFoods()
    {
        return anomalyOrderedFoods;
    }


    public void SetCcorrectFoodObjects()
    {

    }
}
