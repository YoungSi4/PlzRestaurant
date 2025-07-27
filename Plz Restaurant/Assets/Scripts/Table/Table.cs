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
    public int[] visitorIDOnChair { get; private set; } // 각 의자에 앉은 손님의 아이디를 저장
    [SerializeField]
    public Transform[] chairPos; // 각 의자의 위치

    // 음식 둘 위치는 구현 방식을 못 정했음
    private Transform[] foodPos { get; set; }

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
    
    // 수정 - 손님 묶는 쪽에서 앉을 의자 번호를 알아서 지정하고 위치를 반환 받아야 할 듯?
    // visitorIDsToSitChair : 의자 번호 인덱스에 맞게 전달된 매개변수여야 한다.
    void VisitorSitOnChair(params int[] vistorIDsToSitChair)
    {
        isTableOccupied = true;

        // 아래의 로직을 손님 그룹 단위 묶는 쪽에서 해야함
        // int randPos = Random.Range(1, chairNum); // 아무 위치를 하나만 지정
        
        // 이후 인원 수에 따라 앉는 자리를 배치
        // 2인 : 맞은 편에
        // 3인 : 남는 자리 아무대나
        // 4인 : 마지막 자리

        // 이후 손님 그룹 쪽에서 알아서 chairPos 이용해서 가져와서 자리 세팅하면 됨
    }

    void VisitorStandUpChair()
    {
        isTableOccupied = false;
        for(int i = 0;  visitorIDOnChair.Length > 0; i++)
        {
            visitorIDOnChair[i] = -1;
        }
    }

}
