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
    private VisitorOrder order;

    public GameObject readyToOrderMark;

    private int C_ID; // 손님 고유 id : visitor spawner에서 부여
    private int C_seatTableNumber; // 앉을 테이블 번호, 의자 번호
    private int C_seatChairNumber;
    private bool hasOrdered = false;
    private int C_orderID;
    private bool isEating = false;
    private bool hasEaten = false;
    private int C_payment = 0;
    private int C_foodNumber; // 손님이 주문한 음식 번호

    public void Init(VisitorPool pool, int visitorID)
    {
        this.pool = pool;

        /* 랜덤변수로 초기화 할 변수
        C_orderID;
        */

        C_ID = visitorID;
        // C_seatNumber = Random.Range(1.1f, 4.4f);
        C_foodNumber = Random.Range(1, 2);
        //Debug.Log("seatNumber : " +  C_seatNumber);
        //Debug.Log("foodNumber : " + C_foodNumber);



        StartCoroutine(DisableObj());
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spawner = GameObject.Find("visitorSpawner").GetComponent<VisitorSpawner>();
        order = GameObject.Find("VisitorOrder").GetComponent<VisitorOrder>();
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
            StartCoroutine(Mark(true, 1));
        }
    }

    IEnumerator Mark(bool what, float t=0)
    {
        yield return new WaitForSeconds(t);
        readyToOrderMark.SetActive(what);
    }

    private IEnumerator DisableObj()
    {
        yield return wait;
        readyToOrderMark.SetActive(false);
        ResetVars();
        
        pool.SetObj(this);
    }
    
    // 손님을 Disable 할 때 
    private void ResetVars()
    {
        C_ID = 0;
        C_seatTableNumber = 0;
        C_seatChairNumber = 0;
        hasOrdered = false;
        C_orderID = 0;
        isEating = false;
        hasEaten = false;
        C_payment = 0;
        C_foodNumber = 0; // 손님이 주문한 음식 번호
    }

    // player가 상호작용 할 때 이 함수를 실행시키기
    // VisitorOrder 객체를 통해서 데이터가 플레이어의 UI로 전달
    public void SendOrderInfo()
    {
        // C_foodNumber : FoodDB 상의 음식 번호
        // 테이블 번호 : C_seatNumber을 명시적 형변환하여 사용
        order.SetFoodNumFromVisitor(C_foodNumber, C_seatTableNumber);
    }

    // 5초동안 식사
    
    // 자리에서 일어나기 -> 테이블 매니저, 스포너 등에서 리스트 관리

    // 자리에 돈 지불
}

