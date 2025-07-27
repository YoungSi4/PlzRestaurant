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
            yield return delay;
            groupVisitorNum = Random.Range(1, 3); // 1에서 2까지 균등확률. 해당 손님 그룹의 인원
            
            
            for (int i = 0;  i < groupVisitorNum; i++)
            {
                var visitor = pool.GetObj(); // get visitor from pool
                visitor.transform.position = transform.position;
                visitor.Init(pool);
            }

        }
    }

    private void ChooseTable()
    {
        groupTableNum = Random.Range(1, tableCount+1); // 일단 1 ~ 4로 범위 정해둠.

    }
}
