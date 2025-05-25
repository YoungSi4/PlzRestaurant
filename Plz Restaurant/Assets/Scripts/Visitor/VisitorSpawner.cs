using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSpawner : Singleton<VisitorSpawner>
{
    public VisitorPool pool;
    private WaitForSeconds delay = new WaitForSeconds(3f); //¿Ã∞≈ πŸ≤ﬁ

    public override void Awake()
    {
        base.Awake();
    }

    public void Start_Spawning()
    {
        StartCoroutine(SpawnVisitor());
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
