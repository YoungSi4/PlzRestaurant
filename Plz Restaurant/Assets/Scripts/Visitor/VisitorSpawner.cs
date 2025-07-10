using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSpawner : Singleton<VisitorSpawner>
{
    public VisitorPool pool;

    [SerializeField]
    private float spawnDelay = 3f;
    private WaitForSeconds delay;

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
            var visitor = pool.GetObj(); // get visitor from pool
            visitor.transform.position = transform.position;
            visitor.Init(pool);
        }
    }
}
