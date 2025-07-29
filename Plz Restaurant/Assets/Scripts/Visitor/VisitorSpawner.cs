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
    List<List<Visitor>> groupVisitorsList;

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
           
            // 테이블 꽉 찼는지 체크하고 생성 멈출 코드 필요

            // 한 번에 생성할 손님 수 (한 그룹에 몇 명인지)
            groupVisitorNum = Random.Range(1, 3); // 1에서 2까지 균등확률. 해당 손님 그룹의 인원

            // 같은 그룹 묶어서 소환
            // 한 그룹으로 묶을 리스트
            List<Visitor> visitors = new List<Visitor>();
            for (int i = 0;  i < groupVisitorNum; i++)
            {
                var visitor = pool.GetObj(); // get visitor from pool
                visitor.transform.position = transform.position;
                visitor.Init(pool);
                visitors.Add(visitor);
            }
            // 그룹을 담아둘 연결 리스트 들어온 순서대로 앞쪽
            groupVisitorsList.Add(visitors);
            ChooseTable();
        }
    }

    private void ChooseTable()
    {
        groupTableNum = Random.Range(1, tableCount+1); // 현재 테이블 갯수에 맞게 랜덤 생성
        
        // 테이블 번호 1번이 0번 인덱스에 있으니까
        var chosenTable = tableManager.GetTable(groupTableNum - 1);

        // 선택한 테이블이 이미 사용 중 or 인원 수보다 테이블 자리 수가 적다면
        while (chosenTable.isTableOccupied || groupVisitorNum > chosenTable.chairNum )
        {
            // 앉을 테이블 번호 + 1
            groupTableNum++;
            // 인덱스 넘어서지 않도록 전체 테이블 갯수로 나눠줌
            groupTableNum %= tableCount;

            chosenTable = tableManager.GetTable(groupTableNum - 1);
        }
    }
}
