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

    public GameObject mark;

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
        var targetPos = new Vector3(Random.Range(-5f, 9f), 0f, Random.Range(-5f, 14f));
        agent.SetDestination(targetPos);
    }

    private void Update()
    {   // 1번째 agent의 경로 계산이 완벽히 됬는지 && 2번째 agent의 현재위치에서 목적지까지의 거리 - 자동으로 멈추는 거리 < 
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance - agent.stoppingDistance < 0.5f)
        {
            //혹은 코루틴 함수로 만들어서 할 수 있음
            StartCoroutine(Mark(true,1));
        }
    }

    IEnumerator Mark(bool what,float t=0)
    {
        yield return new WaitForSeconds(t);
        mark.SetActive(what);
    }

    private IEnumerator DisableObj()
    {
        yield return wait;
        mark.SetActive(false);
        pool.SetObj(this);
    }
    // ai navigation

}

