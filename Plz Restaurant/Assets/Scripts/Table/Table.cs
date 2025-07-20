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
    private int tableNum { get; set;  }
    private bool isTableOccupied {  get; set; } 
    private int chairNum { get; set; }
    private int[] chairs; // 각 의자에 앉은 손님의 아이디를 저장
    
    //

    // constructor
    public Table()
    {
        this.tableNum = -1;
        this.isTableOccupied = false;
        this.chairNum = -1;
        this.chairs = new int[chairNum];
    }

    public Table(int tableNum, bool isTableOccupied, int chairNum)
    {
        this.tableNum = tableNum;
        this.isTableOccupied = isTableOccupied;
        this.chairNum = chairNum;
        this.chairs = new int[chairNum];
    }


}
