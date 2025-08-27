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
    // private VisitorOrder order; // - 더이상 visitor가 가지고 있을 이유가 없음

    public GameObject readyToOrderMark;

    public int C_ID { get; private set; } // 손님 고유 id : visitor spawner에서 부여
    public int C_seatTableNumber { get; private set; } // 앉을 테이블 번호, 의자 번호
    public int C_seatChairNumber { get; private set; }
    public bool hasOrdered = false;
    private int C_orderID; // 이거 테이블에 있어야 하지 않을까? 혹은 주문 객체가 가지고 있던가
    private bool isEating = false;
    private bool hasEaten = false;
    private int C_payment = 0;
    public int[] C_foodNumber { get; private set; } // 손님이 주문한 음식 번호
    public int numOfOrderFood;

    private WaitForSeconds waitToAngry;
    private int angryTime = 15;

    public void Init(VisitorPool pool, int visitorID)
    {
        this.pool = pool;

        /* 랜덤변수로 초기화 할 변수
        C_orderID;
        */

        C_ID = visitorID;
        // C_seatNumber = Random.Range(1.1f, 4.4f);

        numOfOrderFood = Random.Range(1, 3); // 1 ~ 2
        for (int i = 0; i < numOfOrderFood;  i++)
        {
            C_foodNumber[i] = Random.Range(0, 11);
        }

        //Debug.Log("seatNumber : " +  C_seatNumber);
        //Debug.Log("foodNumber : " + C_foodNumber);

        // StartCoroutine(DisableObj());
    }

    public void SetTableNum(int tableNum)
    {
        this.C_seatTableNumber = tableNum;
    }
    public void SetSeatNum(int seatNum)
    {
        this.C_seatChairNumber = seatNum;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spawner = GameObject.Find("visitorSpawner").GetComponent<VisitorSpawner>();
        order = GameObject.Find("VisitorOrder").GetComponent<VisitorOrder>();
    }

    //private void OnEnable()
    //{
    //    var targetPos = new Vector3(Random.Range(-5f, 9f), 0f, Random.Range(-5f, 14f));
    //    agent.SetDestination(targetPos);
    //}

    //private void Update()
    //{   // 1번째 agent의 경로 계산이 완벽히 됬는지 && 2번째 agent의 현재위치에서 목적지까지의 거리 - 자동으로 멈추는 거리 < 
    //    //if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance - agent.stoppingDistance < 0.5f)
    //    //{
    //    //    //혹은 코루틴 함수로 만들어서 할 수 있음
    //    //    StartCoroutine(Mark(true, 1));
    //    //}
    //}

    //IEnumerator Mark(bool what, float t=0)
    //{
    //    yield return new WaitForSeconds(t);
    //    readyToOrderMark.SetActive(what);
    //}

    // player가 상호작용 할 때 이 함수를 실행시키기
    // VisitorOrder 객체를 통해서 데이터가 플레이어의 UI로 전달
    // 필요 없어짐 (사유 : 테이블에 상호작용 하는 걸로 변경)
    //public void SendOrderInfo()
    //{
    //    // C_foodNumber : FoodDB 상의 음식 번호
    //    // 테이블 번호 : C_seatNumber을 명시적 형변환하여 사용
    //    // order.SetFoodNumFromVisitor(C_foodNumber, C_seatTableNumber);
    //}

    // 외부에서 visitior 이동 위치 지정하는 함수
    public void Move(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    // 도착 후 주문 정보 뜨고 15초 뒤에 화내는 모션
    public IEnumerator Angry()
    {
        waitToAngry = new WaitForSeconds(angryTime);
        yield return waitToAngry;
        // 화내는 모션
    }

    // 화내는 모션 취소 함수 - 외부 실행

    public void CancelAngry()
    {
        // 화내는 거 모션 취소, idle 상태로 돌아감
    }

    // 5초동안 식사

    // 자리에서 일어나기 -> 테이블 매니저, 스포너 등에서 리스트 관리

    // 자리에 돈 지불

    // 가게 밖으로 나가기

    // pool에 회수
    private void DisableObj()
    {
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
        C_foodNumber[0] = -1; // 손님이 주문한 음식 번호
        C_foodNumber[1] = -1;
    }
}

