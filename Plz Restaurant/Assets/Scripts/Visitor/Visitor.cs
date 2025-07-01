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
    {   // 1��° agent�� ��� ����� �Ϻ��� ����� && 2��° agent�� ������ġ���� ������������ �Ÿ� - �ڵ����� ���ߴ� �Ÿ� < 
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance - agent.stoppingDistance < 0.5f)
        {
            //Ȥ�� �ڷ�ƾ �Լ��� ���� �� �� ����
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

