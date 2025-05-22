using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    private VisitorPool pool;
    private VisitorSpawner spawner;
    private WaitForSeconds wait = new WaitForSeconds(10f);
    private NavMeshAgent agent;

    public void Init(VisitorPool pool)
    {
        this.pool = pool;
        StartCoroutine(DisableObj());
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spawner = GameObject.Find("visitorSpawner").GetComponent<VisitorSpawner>();
    }

    private void OnEnable()
    {
        var targetPos = new Vector3(Random.Range(-23f, 23f), 0f, Random.Range(-23f, 23f));
        Debug.Log("Target Postion: " + targetPos);
        agent.SetDestination(targetPos);
    }

    private IEnumerator DisableObj()
    {
        yield return wait;
        pool.SetObj(this);
    }
    // ai navigation

}
