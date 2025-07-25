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
        

        while (true)
        {
            yield return delay;
            groupVisitorNum = Random.Range(1, 5); // 1에서 4까지 균등확률. 해당 손님 그룹의 인원
            
            
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
