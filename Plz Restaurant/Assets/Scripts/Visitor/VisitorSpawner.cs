using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSpawner : MonoBehaviour
{
    public VisitorPool pool;
    private WaitForSeconds delay = new WaitForSeconds(3f); //�̰� �ٲ�

    void Start()
    {
        StartCoroutine(SpawnVisitor());
    }


    private IEnumerator SpawnVisitor()
    {
        while (true)
        {
            yield return delay;
            var visitor = pool.GetObj(); // get visitor from pool
            visitor.transform.position = transform.position; //�߰���
            visitor.Init(pool);
        }
    }
}
