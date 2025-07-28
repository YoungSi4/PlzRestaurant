using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform tablePos; // 서빙할 테이블 위치 저장
    public Transform bossHandPos1; // 음식을 들 사장님의 손 위치 저장.
    public Transform bossHandPos2; // 음식을 들 사장님의 손 위치 저장.

    private int lastPickedTrayIndex = 0; // 가장 최근에 챙긴 음식의 트레이 위치 저장
    private Vector3 B_startPos; // 사장님의 기본위치 저장
    private Quaternion B_startRot; // 사장님의 기본위치 방향 저장
    private float B_speed = 5; // 사장님의 이동 속도 (조정 가능)
    private int B_abillity = 2; // 한 번에 들 수 있는 음식 수 (최대4개 예정)
    private List<GameObject> B_handFoods = new List<GameObject>(); // 사장님 손에 들고 있는 음식의 리스트

    private bool isBusy = false; // 현재 음식을 서빙하는 코루틴이 실행중인지 검사

    private NavMeshAgent nav; // 네비게이션
    private TrayControl trayControl;

    

    void Awake()
    {
        // 사장님 시작위치를 저장해둠
        B_startPos = transform.position;
        // 사장님 시작방향 저장해둠
        B_startRot = transform.rotation;

        // 초기화
        nav = gameObject.GetComponent<NavMeshAgent>(); 
        trayControl = FindObjectOfType<TrayControl>();

        nav.speed = B_speed;
    }

    void Update()
    {
        // 코루틴이 동작중이지 않으면서 트레이가 한자리라도 채워져 있는 경우 실행
        if (!isBusy && trayControl.isFoodOnTray())
        {
            isBusy = true;
            StartCoroutine(BossRoutine());
        }

    }

    // 사장님의 움직임 총괄
    IEnumerator BossRoutine()
    {
        // 트레이가 채워져 있다면 서빙루틴 반복
        while (trayControl.isFoodOnTray())
        {
            // 트레이 앞으로 이동 (위치 조정필요)
            // 도착할 때 까지 다른 동작 하지 않게 하기 위해 yield return
            yield return StartCoroutine(MoveToPos(trayControl.foodPositions[0].transform.position + new Vector3(0, 0, 1f)));

            // 들 수 있는 만큼 들 때 까지 음식 들기 반복
            while (B_handFoods.Count < B_abillity)
            {
                // 트레이에 음식이 없다면 음식을 드는 동작을 멈추고 서빙으로 넘어가기 위한 예외처리
                if (!trayControl.isFoodOnTray())
                    break;
                // 순서에 따라 트레이에서 음식 들기
                switch (lastPickedTrayIndex)
                {
                    case 0:
                    case 2:
                        if (!trayControl.isTrayFirstSlotEmpty())
                        {
                            yield return new WaitForSeconds(0.5f);
                            // 음식 들기
                            PickFood(1);
                            // 최근 트레이에서 음식을 챙긴 위치 저장
                            lastPickedTrayIndex = 1;

                        }
                        break;
                    case 1:
                        if (!trayControl.isTraySecondSlotEmpty())
                        {
                            yield return new WaitForSeconds(0.5f);
                            // 음식 들기
                            PickFood(2);
                            // 최근 트레이에서 음식을 챙긴 위치 저장
                            lastPickedTrayIndex = 2;
                        }
                        else if (trayControl.isTraySecondSlotEmpty() && !trayControl.isTrayFirstSlotEmpty())
                        {
                            yield return new WaitForSeconds(0.5f);
                            // 음식 들기
                            PickFood(1);
                            // 최근 트레이에서 음식을 챙긴 위치 저장
                            lastPickedTrayIndex = 1;
                        }
                        break;
                }
            }
            // 서빙할 테이블로 이동 후 음식을 두는 동작. 들고 있는 음식을 모두 내릴 때 까지 반복
            while (B_handFoods.Count > 0)
            {
                // 서빙할 테이블 위치를 얻는 로직이 필요

                // 주문한 테이블 위치로 이동
                yield return StartCoroutine(MoveToPos(tablePos.position));
                // 테이블에 음식 내려놓기
                ServeFood();
            }
        }


        // 음식이 모두 처리되면 원위치로 복귀
        if (!trayControl.isFoodOnTray())
        {
            yield return StartCoroutine(MoveToPos(B_startPos));
            yield return StartCoroutine(RotateToStart());
        }

        isBusy = false;
    }

    // 음식을 들 손 위치를 선택
    // case 늘리기로 확장 가능
    Transform selectHandPos()
    {
        switch (B_handFoods.Count)
        {
            case 0:
                return bossHandPos1;
            case 1:
                return bossHandPos2;
            // 음식을 이미 들 수 있는 만큼 들고 있는 경우
            default:
                return null;
        }
    }

    // 사장님이 트레이에서 음식을 챙기는 로직
    void PickFood(int trayIndex)
    {
        // 오류방지 들 수 있는 음식 수 만큼 들고 있으면 
        if (B_handFoods.Count >= B_abillity) return;
        // 어느 위치에 들지 선택
        Transform handPos = selectHandPos();
        // 트레이에서 들 음식 정보 불러 오기 및 트레이에서 삭제
        GameObject B_handFood = trayControl.TakeFoodFromTray(trayIndex, handPos);

        if (B_handFood != null)
        {
            // B_handFood의 부모를 사장님의 손 위치로 설정
            B_handFood.transform.SetParent(handPos);
            // 들고있는 음식 큐에 넣기
            B_handFoods.Add(B_handFood);
            trayControl.ClearFood(trayIndex); // 트레이에서 음식 삭제
        }

    }

    // 테이블에 음식 내려놓기
    // 들고 있는 음식 리스트를 순회하며 도착한 테이블에 서빙할 음식이 더 있는지 확인 후 있으면 추가로 내려놓기(로직추가필요)
    void ServeFood()
    {
        while (B_handFoods.Count > 0) // 사장님이 들고있는 음식이 있는 경우(B_handFoods가 empty가 아닌 경우)
        {
            GameObject B_handFood = B_handFoods[0];
            B_handFoods.RemoveAt(0);
            // 테이블에 음식 생성
            GameObject tableFood = Instantiate(B_handFood, tablePos.position, tablePos.rotation);
            // 테이블에 올릴 음식은 부모 해제 후 독립 개체로
            tableFood.transform.SetParent(null);

            // 손에서 오브젝트 삭제
            Destroy(B_handFood);
        }
    }
    // 목적 위치로 이동
    IEnumerator MoveToPos(Vector3 targetPos)
    {
        // 목적 위치로 이동
        nav.SetDestination(targetPos);

        // 경로계산 중 대기
        while (nav.pathPending)
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
