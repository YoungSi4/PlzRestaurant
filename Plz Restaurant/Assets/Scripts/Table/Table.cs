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
    [SerializeField]
    public int tableNum;
    [SerializeField]
    public int chairNum { get; private set; } // 굳이 필요할까? chairPos의 길이로 접근해도 되잖아.
    public bool isTableOccupied { get; private set; }
    public Visitor[] visitorOnChair { get;  private set; } // 각 의자에 앉은 손님 객체를 저장
    [SerializeField]
    public Transform[] chairPos; // 각 의자의 위치
    public int visitorNum { get;  set; } // 손님의 수

    [SerializeField]
    private Collider visitorCheckCollider; // 손님 검사
    private WaitForSeconds inspectionDelay;
    private float delay = 5f;
    public bool IsWaitingForVisitorArrived { get;  set; } // 배정된 손님을 기다리는 중인지
    private WaitForSeconds orderDelay = new WaitForSeconds(5f);

    [SerializeField]
    private GameObject readyToOrderIcon;

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
    }

    // 아이콘 띄우는 함수
    private void ReadyToOrder()
    {

    }

    /// <summary>
    /// 해당 테이블에 앉은 손님이 주문한 음식 ID를 넘기는 함수
    /// </summary>
    /// <returns>
    /// int[] - 의자 번호 순서대로 저장
    /// </returns>
    public int[] SendFoodNumToOrderInfo()
    {
        // 앉은 손님 수만큼 동적 길이 배열 선언
        int[] foodNums = new int[visitorNum];
        int tempIdx = 0;

        foreach(var visitor in visitorOnChair)
        {
            foodNums[tempIdx] = visitor.C_foodNumber;
            tempIdx++;
        }

        return foodNums;
    }
}
