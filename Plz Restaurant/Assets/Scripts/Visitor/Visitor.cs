using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    private VisitorPool pool;
    private WaitForSeconds wait = new WaitForSeconds(20f); // 이거도 바꿈
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
    }

    private void OnEnable()
    {
        var targetPos = new Vector3(Random.Range(-5f, 8f), 0f, Random.Range(-5f, 14f));
        Debug.Log("Target Postion: " + targetPos);
        agent.SetDestination(targetPos);
    }

    private void Update()
    {   // 1번째 agent의 경로 계산이 완벽히 됬는지 && 2번째 agent의 현재위치에서 목적지까지의 거리 - 자동으로 멈추는 거리 < 
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance - agent.stoppingDistance < 0.5f)
        {
            //혹은 코루틴 함수로 만들어서 할 수 있음
            Invoke("MarkOn",1f);
        }
        // transform.Translate(0f, 0f, 5f * Time.deltaTime);
        // transform.Rotate(new Vector3(1f, 0f, 0f), 50f  * Time.deltaTime);
        // 
    }
    private void MarkOn()
    {
        mark.SetActive(true);
    }

    private IEnumerator DisableObj()
    {
        yield return wait;
        transform.position = Vector3.zero;
        pool.SetObj(this);
    }
    // ai navigation

}

