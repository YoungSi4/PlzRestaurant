using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    private VisitorPool pool;
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
    }

    private void OnEnable()
    {
        var targetPos = new Vector3(Random.Range(-23f, 23f), 0f, Random.Range(-23f, 23f));
        Debug.Log("Target Postion: " + targetPos);
        agent.SetDestination(targetPos);
    }

    private void Update()
    {
        // transform.Translate(0f, 0f, 5f * Time.deltaTime);
        // transform.Rotate(new Vector3(1f, 0f, 0f), 50f  * Time.deltaTime);
        // 
    }

    private IEnumerator DisableObj()
    {
        yield return wait;
        transform.position = Vector3.zero;
        pool.SetObj(this);
    }
    // ai navigation

}
