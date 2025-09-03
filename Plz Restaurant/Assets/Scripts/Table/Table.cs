using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/***********************************************************************************
 * 테이블 객체 : 테이블의 상태를 관리하고 타 스크립트에서 이용할 목적의 자료구조?
 * 멤버 변수 : 테이블 번호, 사용 중 여부, 의자 갯수, 각 의자 상태, 각 의자 좌표
 * 멤버 함수 : 생성자 (기본, 매개변수), getter setter, VisitorSit, VisitorStandUp
 ************************************************************************************/

// table 객체는 일종의 노드에 가깝고, 이걸 배열에 넣고 관리할 객체가 또 필요함
// 트리와 노드의 관계를 연상하면 될 듯?

public class Table : MonoBehaviour
{
    public int tableNum;
    [SerializeField]
    public int chairNum { get; private set; } // 굳이 필요할까? chairPos의 길이로 접근해도 되잖아.
    public bool isTableOccupied { get; private set; }
    public Visitor[] visitorOnChair { get;  private set; } // 각 의자에 앉은 손님 객체를 저장
    [SerializeField]
    public Transform[] chairPos; // 각 의자의 위치
    public int visitorNum { get;  set; } // 손님의 수
    private List<Coroutine> visitorAngry;

    [SerializeField]
    private Collider visitorCheckCollider; // 손님 검사
    private WaitForSeconds inspectionDelay;
    private float delay = 5f;
    public bool IsWaitingForVisitorArrived { get;  set; } // 배정된 손님을 기다리는 중인지
    private WaitForSeconds orderDelay = new WaitForSeconds(5f);

    [SerializeField]
    private GameObject readyToOrderIconPrefab; // 컴포넌트 상에서 연결한 자식 오브젝트
    public bool isReadyToOrder { get; private set; } // true 일 때만 상호작용 가능

    // 음식 둘 위치 : 각 테이블마다 지정?
    [SerializeField]
    private Transform[] foodPos;



    private void Start()
    {
        chairNum = chairPos.Length;
        visitorOnChair = new Visitor[chairNum];
        inspectionDelay = new(delay);
        IsWaitingForVisitorArrived = false;
        visitorNum = 0;
        isReadyToOrder = false;
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

    public void VisitorStandUpChair()
    {
        isTableOccupied = false;
        for(int i = 0;  visitorOnChair.Length > 0; i++)
        {
            visitorOnChair[i] = null;
        }
        visitorNum = 0;
        visitorCheckCollider.enabled = true; // 다시 손님 받을 준비
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
            visitorCheckCollider.enabled = false; // 주문 대기부터는 잠시 collider를 꺼둔다
            StartCoroutine(WaitingForOrder());
        }
    } // onTriggerEnter -end-

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
            foodIDs[visitor.C_seatChairNumber] = new List<int>();
            foreach(var foodId in visitor.C_foodNumber)
            {
                foodIDs[visitor.C_seatChairNumber].Add(foodId);
            }
        }

        isReadyToOrder = false;

        // 화내기 코루틴 종료
        // 화내기 전에 이 함수가 실행되더라도 코루틴은 여전히 실행 중 -> 따로 종료 시켜줘야함
        foreach(var co in visitorAngry)
        {
            StopCoroutine(co);
        }
        // 화내는 애니메이션 정지
        foreach (var visitor in visitorOnChair)
        {
            visitor.CancelAngry();
        }

        return foodIDs;
    }
}
