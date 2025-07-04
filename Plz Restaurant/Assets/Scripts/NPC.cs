using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    public TrayControl trayControl;
    public Transform tablePos; // 서빙할 테이블 위치 저장
    public Transform bossHandPos1; // 음식을 들 사장님의 손 위치 저장.
    public Transform bossHandPos2; // 음식을 들 사장님의 손 위치 저장.

    private Vector3 B_startPos; // 사장님의 기본위치 저장
    private Quaternion B_startRot; // 사장님의 기본위치 방향 저장
    private NavMeshAgent nav; // 네비게이션
    private float B_speed = 5; // 사장님의 이동 속도 (조정 가능)
    private int B_abillity = 2; // 한 번에 들 수 있는 음식 수 (최대4개 예정)
    private Queue<GameObject> foodCopies = new Queue<GameObject>(); // 사장님 손에 들 음식 오브젝트 리스트

    private bool isBusy = false; // 현재 음식을 서빙하는 코루틴이 실행중인지 검사

    // 가장 최근에 챙긴 음식의 트레이 위치 저장
    // 트레이에 올릴 수 있는 음식개수가 늘어나게 되면 하나씩 추가하는 방식으로 가능할 듯
    public enum TrayPickupIndex
    {
        None, First, Second
    }
    // 가장 최근에 챙긴 음식의 트레이 위치 초기화
    private TrayPickupIndex lastPickedIndex = TrayPickupIndex.None;


    void Awake()
    {
        // 사장님 시작위치를 저장해둠
        B_startPos = transform.position;
        // 사장님 시작방향 저장해둠
        B_startRot = transform.rotation;
        nav = gameObject.GetComponent<NavMeshAgent>(); // 초기화

        nav.speed = B_speed;
    }

    void Update()
    {
        // 코루틴이 동작중이지 않으면서 트레이가 한자리라도 채워져 있는 경우 실행
        if (!isBusy && (trayControl.foodObj1 != null || trayControl.foodObj2 != null))
        {
            isBusy = true;
            StartCoroutine(BossRoutine());
        }

    }

    // 사장님의 움직임 총괄
    IEnumerator BossRoutine()
    {
        // 트레이가 채워져 있다면 서빙루틴 반복
        while(trayControl.foodObj1 != null || trayControl.foodObj2 != null)
        {
            // 트레이에서 음식 드는 동작
            while (foodCopies.Count < B_abillity)
            {
                if (trayControl.foodObj1 == null && trayControl.foodObj2 == null)
                    break;
                switch (lastPickedIndex)
                {
                    case TrayPickupIndex.None:
                    case TrayPickupIndex.Second:
                        if (trayControl.foodObj1 != null)
                        {
                            // 트레이 1번 음식위치 앞으로 이동

                            yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                            // 음식 들기
                            PickFood(1);
                            yield return new WaitForSeconds(0.5f);

                            // 최근 트레이에서 음식을 챙긴 위치 저장
                            lastPickedIndex = TrayPickupIndex.First;

                        }
                        break;
                    case TrayPickupIndex.First:
                        if (trayControl.foodObj2 != null)
                        {
                            // 트레이 2번 음식위치 앞으로 이동
                            yield return StartCoroutine(MoveToPos(trayControl.foodPos2.transform.position + new Vector3(0, 0, 1f)));
                            // 음식 들기
                            PickFood(2);
                            yield return new WaitForSeconds(0.5f);

                            // 최근 트레이에서 음식을 챙긴 위치 저장
                            lastPickedIndex = TrayPickupIndex.Second;
                        }
                        else if (trayControl.foodObj2 == null && trayControl.foodObj1 != null)
                        {
                            // 트레이 1번 음식위치 앞으로 이동
                            yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                            // 음식 들기
                            PickFood(1);
                            yield return new WaitForSeconds(0.5f);

                            // 최근 트레이에서 음식을 챙긴 위치 저장
                            lastPickedIndex = TrayPickupIndex.First;
                        }
                        break;
                }
            }
            // 서빙할 테이블로 이동 후 음식을 두는 동작. 들고 있는 음식을 모두 내릴 때 까지 반복
            while (foodCopies.Count > 0)
            {
                // 서빙할 테이블 위치를 얻는 로직이 필요

                // 주문한 테이블 위치로 이동
                yield return StartCoroutine(MoveToPos(tablePos.position));
                // 테이블에 음식 내려놓기
                ServeFood();
            }
        }


        // 음식이 모두 처리되면 원위치로 복귀
        if (trayControl.foodObj1 == null && trayControl.foodObj2 == null)
        {
            yield return StartCoroutine(MoveToPos(B_startPos));
            yield return StartCoroutine(RotateToStart());
        }

        isBusy = false;
    }

    // 사장님이 트레이에서 음식을 챙기는 로직
    void PickFood(int trayIndex)
    {
        // 오류방지 들 수 있는 음시 수 만큼 들고 있으면 
        if (foodCopies.Count >= B_abillity) return;
        GameObject foodCopy = null;
        // 음식을 들고있지 않을 때
        if(foodCopies.Count == 0)
        {
            // 사장님 손위치에 챙긴 음식 생성
            if (trayIndex == 1)
            {
                // 생성할 음식 오브젝트 foodCopy에 저장
                foodCopy = Instantiate(trayControl.foodObj1, bossHandPos1.position, bossHandPos1.rotation);
            }
            else if (trayIndex == 2)
            {
                foodCopy = Instantiate(trayControl.foodObj2, bossHandPos1.position, bossHandPos1.rotation);
            }

            // foodCopy에 저장한 오브젝트 손 위치에 생성
            foodCopy.transform.SetParent(bossHandPos1);
            // 들고있는 음식 큐에 넣기
            foodCopies.Enqueue(foodCopy);
            // 트레이에서 음식 삭제
            trayControl.ClearFood(trayIndex);
        }
        // 음식을 한 개 들고 있을 때
        else if(foodCopies.Count == 1)
        {
            // 사장님 손위치에 챙긴 음식 생성
            if (trayIndex == 1)
            {
                // 생성할 음식 오브젝트 foodCopy에 저장
                foodCopy = Instantiate(trayControl.foodObj1, bossHandPos2.position, bossHandPos2.rotation);
            }
            else if (trayIndex == 2)
            {
                foodCopy = Instantiate(trayControl.foodObj2, bossHandPos2.position, bossHandPos2.rotation);
            }

            // foodCopy에 저장한 오브젝트 손 위치에 생성
            foodCopy.transform.SetParent(bossHandPos2);
            // 들고있는 음식 큐에 넣기
            foodCopies.Enqueue(foodCopy);
            // 트레이에서 음식 삭제
            trayControl.ClearFood(trayIndex);
        }
        // else if(2,3 만들어서 4까지)
    }

    // 테이블에 음식 내려놓기
    void ServeFood()
    {
        while(foodCopies.Count> 0) // 사장님이 들고있는 음식이 있는 경우(foodCopies가 empty가 아닌 경우)
        {
            GameObject foodCopy = foodCopies.Dequeue();
            // 테이블에 음식 생성
            GameObject tableFood = Instantiate(foodCopy, tablePos.position, tablePos.rotation);
            // 테이블에 올릴 음식은 부모 해제 후 독립 개체로
            tableFood.transform.SetParent(null);

            // 손에서 오브젝트 삭제
            Destroy(foodCopy);
        }
    }
    // 목적 위치로 이동
    IEnumerator MoveToPos(Vector3 targetPos)
    {
        // 목적 위치로 이동
        nav.SetDestination(targetPos);

        // 경로계산 중 대기
        while(nav.pathPending)
            yield return null;

        // 이동 중 대기(목적지 도착까지)
        // 남은거리 > 정지거리 || 속도 존재
        while (nav.remainingDistance > nav.stoppingDistance || nav.velocity.sqrMagnitude > 0.01f)
            yield return null;
    }
    // 초기 위치 이동 시 초기 상태로 회전
    IEnumerator RotateToStart()
    {
        // 보간을 위한 시간값
        float t = 0f;

        // 현재 방향 (시작값)
        Quaternion current = transform.rotation; 
        while (t < 1f)
        {
            t += Time.deltaTime * 2f; // 회전 속도 조절
            // 시작(현재) 회전값에서 도착(초기) 회전값으로 보간 값 t에 따라 0 -> 1 회전
            transform.rotation = Quaternion.Slerp(current, B_startRot, t);
            yield return null;
        }
    }


}
