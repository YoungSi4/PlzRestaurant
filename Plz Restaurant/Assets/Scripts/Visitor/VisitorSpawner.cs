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
    private float tablecCheckDelay = 5f;
    private WaitForSeconds delay;
    private WaitForSeconds emptyTableCheckDelay;

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
        emptyTableCheckDelay = new WaitForSeconds(tablecCheckDelay);

        Start_Spawning(); // start 버튼 누르면 실행됨
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

    /// <summary>
    /// visitor를 생성하는 비동기 함수
    /// 외부 Start_Spawning 함수에 의해 시작
    /// 외부 StopSpawning 함수에 의해 종료된다.
    /// 
    /// 생성 딜레이 delay -> spawnDelay 3f
    /// 생성 조건 1 : 현재 남은 테이블 중 최대 의자 수
    /// 생성 조건 2 : 남은 테이블의 존재 여부
    /// 생성 조건 3 : isTableOccupied의 T/F 여부
    /// </summary>
    /// <returns> IEnumerator </returns>
    private IEnumerator SpawnVisitor()
    {
        
        // 손님 생성 규칙을 정하려면 웨이팅 여부도 정해야 함.
        /* 남은테이블에 맞게 손님을 스폰한다 (개발 및 플레이어 편의성)
         */
        while (true)
        {
            // 손님 생성 딜레이
            yield return delay;

            // 테이블 최대 인원 수를 보고 생성할 손님 수를 조정
            int currentMaxChair = CurrentMaxChair();

            // 테이블이 꽉 찬 상태
            while (currentMaxChair <= 0)
            {
                // 재검사 대기
                yield return emptyTableCheckDelay;
                currentMaxChair = CurrentMaxChair();
            }

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

            var tempTable = ChooseTable(visitors);
            foreach (var visitor in visitors)
            {
                visitor.SetTableNum(tempTable.tableNum);
            }
            ChooseChair(visitors, tempTable);
            

            // 그룹을 담을 배열
            // 테이블 번호에 맞는 인덱스에 넣음

            groupVisitorsList[groupTableNum] = visitors;
        }
    }


    /// <summary>
    /// 현재 빈 테이블 중 최대 의자 수를 검사하는 함수
    /// </summary>
    /// <returns>
    /// int : 0 2 4
    /// 0은 빈 테이블이 없는 경우 반환됨
    /// </returns>
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

    /// <summary>
    /// 앉을 테이블을 고르는 함수
    /// </summary>
    /// <param name="visitors">
    /// 손님 리스트를 받음
    /// </param>
    private Table ChooseTable(List<Visitor> visitors)
    {
         // 손님이 앉을 테이블 정하기 : 현재 테이블 갯수에 맞게 랜덤 생성
        if (visitors.Count == 1) groupTableNum = 0; // 1명인 손님은 2인 테이블 우선 배정
        else groupTableNum = Random.Range(0, tableCount);

        // 테이블 번호를 전달 . 인덱스 -1 처리는 해당 함수 안에서 실행
        var chosenTable = tableManager.GetTable(groupTableNum);

        // 선택한 테이블이 이미 사용 중 or 인원 수보다 테이블 자리 수가 적다면
        while (chosenTable.isTableOccupied || groupVisitorNum > chosenTable.chairNum)
        {
            // 앉을 테이블 번호 + 1 (선형탐색)
            groupTableNum++;
            // 인덱스 넘어서지 않도록 전체 테이블 갯수로 나눠줌
            groupTableNum %= tableCount;

            chosenTable = tableManager.GetTable(groupTableNum);
        }

        return chosenTable;
    }

    // 
    private void ChooseChair(List<Visitor> visitors, Table table)
    {
        int randomSit = Random.Range(0, table.chairNum);

        // 아래 조건문에 대한 상세한 설명
        // 2명이 4인 테이블에 지정됐을 때 맞은 편에 앉도록 함
        // 2명일 때만 대각선으로 앉는 경우가 생겨서 어색한 배치가 된다
        //
        // 의자 번호
        // 1 2
        // 3 4
        //
        // 2명이 앉을 때 문제가 되는 케이스 : 대각선으로 앉는 경우
        // case 1 ) 1, 4
        // case 2 ) 2, 3
        //
        // randomSit == 1 -> 다음 사람 2번에 앉음 -> OK
        // randomSit == 2 -> 다음 사람 randomSit ==3 -> 문제
        // randomSit == 3 -> 다음 사람 randomSit == 4 -> OK
        // randomSit == 4 -> 다음 사람 randomSit == 1 -> 문제
        if (visitors.Count == 2 && table.chairNum == 4 && (randomSit % 2 == 0))
        {
            randomSit++;
            randomSit %= table.chairNum; // 인덱스(randomSit)가 4를 넘으면 안 됨
        }

        // 손님 수만큼 반복
        foreach (var visitor in visitors)
        {
            var target = table.chairPos[randomSit].position;
            visitor.Move(target);
            table.VisitorSitOnChair(randomSit, visitor); // 테이블에 해당 손님 정보 전달

            randomSit++;
            randomSit %= table.chairNum;
        }
    }
}
