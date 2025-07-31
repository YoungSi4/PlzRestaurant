using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSpawner : Singleton<VisitorSpawner>
{
    public VisitorPool pool;
    [SerializeField]
    private TableManager tableManager;

    [SerializeField]
    private float spawnDelay = 3f;
    private WaitForSeconds delay;
    int groupVisitorNum;
    int groupTableNum; // 그룹 손님이 앉을 테이블
    List<Visitor>[] groupVisitorsList;

    private int visitorID = 1; // 무조건 1씩 증가

    [SerializeField]
    int tableCount; // 맵 상의 테이블 수

    public override void Awake()
    {
        base.Awake();
        delay = new WaitForSeconds(spawnDelay);

        // Start_Spawning(); // start 버튼 누르면 실행됨
    }

    // UI 상의 start 버튼을 누르면 실행된다
    public void Start_Spawning()
    {
        StartCoroutine(SpawnVisitor());
        tableCount = tableManager.tableNum;
        groupVisitorsList = new List<Visitor>[tableCount]; // 테이블 수에 맞게
    }

    public void StopSpawning()
    {
        StopCoroutine(SpawnVisitor());
    }


    private IEnumerator SpawnVisitor()
    {
        
        // 손님 생성 규칙을 정하려면 웨이팅 여부도 정해야 함.
        /* 1. 남은테이블에 맞게 손님을 스폰한다 (개발 및 플레이어 편의성)
         * 2. 손님 생성은 랜덤으로 두고 꽉 차면 웨이팅 시킨다 (현실성)
         */
        while (true)
        {
            // 손님 생성 딜레이
            yield return delay;

            // 테이블 꽉 찼는지 체크하고 생성 중지
            // blank

            // 테이블 최대 인원 수를 보고 생성할 손님 수를 조정
            int currentMaxChair = CurrentMaxChair();

            // 한 번에 생성할 손님 수 (한 그룹에 몇 명인지)
            groupVisitorNum = Random.Range(1, currentMaxChair+1); // 최소 1명, 최대 현재 테이블의 최대 의자 수

            // 같은 그룹 묶어서 소환
            // 한 그룹으로 묶을 리스트
            List<Visitor> visitors = new List<Visitor>();
            for (int i = 0;  i < groupVisitorNum; i++)
            {
                var visitor = pool.GetObj(); // get visitor from pool
                visitor.transform.position = transform.position;
                visitor.Init(pool, visitorID);
                visitorID++;
                visitors.Add(visitor);
            }
            // 그룹을 담아둘 연결 리스트
            // 테이블 번호에 맞게 배열에 넣음

            // 테이블 번호 정하는 함수
            ChooseTable();

            groupVisitorsList[groupTableNum] = visitors;
        }
    }

    // 현재 빈 테이블 중 최대 의자 수를 검사하는 함수
    private int CurrentMaxChair()
    {
        int currentMaxChair = 0;
        foreach (Table t in tableManager.Tables)
        {
            // 테이블이 비어 있고 and 현재 최대 의자 수보다 크면
            if (!t.isTableOccupied && currentMaxChair < t.chairNum)
                currentMaxChair = t.chairNum;
        }

        return currentMaxChair;
    }

    private void ChooseTable(params int[] visitors)
    {
        groupTableNum = Random.Range(1, tableCount+1); // 현재 테이블 갯수에 맞게 랜덤 생성
        
        // 테이블 번호 1번이 배열 0번 인덱스에 있으니까
        var chosenTable = tableManager.GetTable(groupTableNum - 1);

        // 선택한 테이블이 이미 사용 중 or 인원 수보다 테이블 자리 수가 적다면
        // 4인석이 이미 다 찼는데 4명이 들어오면? -> 무한루프
        // 생성 조건에서 해결해야 할 듯
        while (chosenTable.isTableOccupied || groupVisitorNum > chosenTable.chairNum )
        {
            // 앉을 테이블 번호 + 1 (선형탐색)
            groupTableNum++;
            // 인덱스 넘어서지 않도록 전체 테이블 갯수로 나눠줌
            groupTableNum %= tableCount;

            chosenTable = tableManager.GetTable(groupTableNum - 1);
        }

        chosenTable.VisitorSitOnChair(visitors);
    }

    // 
    private void ChooseChair()
    {

    }
}
