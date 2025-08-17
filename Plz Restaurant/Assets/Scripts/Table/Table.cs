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
    public Visitor[] visitorOnChair { get;  private set; } // 각 의자에 앉은 손님의 아이디를 저장
    [SerializeField]
    public Transform[] chairPos; // 각 의자의 위치
    public int visitorNum { get;  set; } // 손님의 수

    private BoxCollider boxCollider; // 손님 검사
    private WaitForSeconds inspectionDelay;
    private float delay = 5f;
    public bool IsWaitingForVisitorArrived { get;  set; } // 배정된 손님을 기다리는 중인지
    private WaitForSeconds orderDelay = new WaitForSeconds(5f);

    // 음식 둘 위치 : 각 테이블마다 지정?
    private Transform[] foodPos { get; set; }

    private void Start()
    {
        chairNum = chairPos.Length;
        visitorIDOnChair = new Visitor[chairNum];
        inspectionDelay = new(delay);
        IsWaitingForVisitorArrived = false;
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


    // 손님을 그룹 단위로 묶어서 관리하는 스크립트 쪽에서 (아마도 Vsitor spawner 혹은 pool)
    // 손님 ID를 보내줘야 함 -> 손님 id가 primary key이므로 생성 규칙에 대해서도 생각해봐야 함

    public void VisitorSitOnChair(int index, Visitor visitor)
    {
        isTableOccupied = true;
        visitorOnChair[index] = visitor;
        IsWaitingForVisitorArrived = true; // 배정된 손님이 도착했는지 기다리는 중
        StartCoroutine(CheckingVisitorHasArrived());
    }

    public void VisitorStandUpChair()
    {
        isTableOccupied = false;
        for(int i = 0;  visitorOnChair.Length > 0; i++)
        {
            visitorOnChair[i] = null;
        }
    }

   /// <summary>
   /// 손님이 테이블에 도착했는지 확인하는 비동기 함수
   /// </summary>
   /// <returns></returns>
    private IEnumerator CheckingVisitorHasArrived()
    {
        while (IsWaitingForVisitorArrived)
        {
            yield return inspectionDelay;

            
        }
        
        

    }

}
