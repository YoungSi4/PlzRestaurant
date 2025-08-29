using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private Table table; // 서빙할 테이블 정보 저장
    private Transform tablePos; // 서빙할 테이블 위치 저장
    public Transform bossHandPos1; // 음식을 들 사장님의 손 위치 저장.
    public Transform bossHandPos2; // 음식을 들 사장님의 손 위치 저장.

    private Vector3 B_startPos; // 사장님의 기본위치 저장
    private Quaternion B_startRot; // 사장님의 기본위치 방향 저장
    private float B_speed = 5; // 사장님의 이동 속도 (조정 가능)
    private int B_abillity = 2; // 한 번에 들 수 있는 음식 수 (최대4개 예정)
    
    // 실제 음식 오브젝트가 필요할 때 마다 orderDatas에서 꺼내서 사용하기 보다 따로 관리해서 사용하는 것이 단순하고
    // 트레이에서 테이블까지 계속해서 존재하는 오브젝트이기 때문에 따로 관리하는 것이 좋을 것 같음
    // 어짜피 orderDatas와 동일한 Index에 동일한 음식이 관리되기 때문에 괜찮다고 생각
    private List<GameObject> B_handFoods = new List<GameObject>(); // 사장님 손에 들고 있는 음식의 리스트
    // 주문 정보 전달 로직 작성중
    private List<OrderData> B_orderDatas = new List<OrderData>(); // 주문 정보 저장용

    private bool isBusy = false; // 현재 음식을 서빙하는 코루틴이 실행중인지 검사

    private NavMeshAgent nav; // 네비게이션
    private TrayControl trayControl;
    private TableManager tableManager;

    

    void Awake()
    {
        // 사장님 시작위치를 저장해둠
        B_startPos = transform.position;
        // 사장님 시작방향 저장해둠
        B_startRot = transform.rotation;

        // 초기화
        nav = gameObject.GetComponent<NavMeshAgent>(); 
        trayControl = FindObjectOfType<TrayControl>();
        tableManager = FindObjectOfType<TableManager>();

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

    // 주문 정보 전달 받는 함수
    // 전달받은 데이터 사용하는 기능 구현 필요
    // TrayControl.cs에서 호출
    public void GetOrderInfo(OrderData orderData)
    {
        if(B_orderDatas.Count >= B_abillity)
        {
            Debug.LogWarning("사장님이 들 수 있는 음식의 수를 초과했습니다.");
            return; // 사장님이 들 수 있는 음식의 수를 초과하면 무시
        }
        // 주문 정보 저장
        B_orderDatas.Add(orderData);
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

            // 트레이에 도착 후 잠시 대기
            yield return new WaitForSeconds(0.5f); 

            // 들 수 있는 만큼 들 때 까지 음식 들기 반복
            while (B_handFoods.Count < B_abillity)
            {
                // 트레이에 음식이 없다면 음식을 드는 동작을 멈추고 서빙으로 넘어가기 위한 예외처리
                if (!trayControl.isFoodOnTray())
                    break;

                // 트레이의 음식을 하나씩 챙긴다면 여기서 WaitForSeconds 해야함
                // 그 경우 HeadChef의 일괄 조리 로직 수정이 필요해짐
                // yield return new WaitForSeconds(0.5f); // 음식 들기 전 잠시 대기

                // 순서에 따라 트레이에서 음식 들기
                int pickIndex = trayControl.PeekPickIndex();
                PickFood(pickIndex);
                trayControl.ConfirmPickIndex(); // 음식 들기에 성공하면 트레이 인덱스 큐에서 해당 인덱스 제거
            }
            // 서빙할 테이블로 이동 후 음식을 두는 동작. 들고 있는 음식을 모두 내릴 때 까지 반복
            while (B_handFoods.Count > 0)
            {
                // 서빙할 테이블 위치를 얻는 로직이 필요
                // TableManager.GetTable()을 쓰긴 할건데 동일한 테이블에 가야하는 주문처리를 어떻게 할 것인가
                // 여기서 테이블 위치 확인해서 이동시키고
                // 이동 후 서빙로직 전 혹은 중에 해당 테이블에 서빙할 음식이 더 있는지 순회해서 있으면 서빙
                // 그 다음에 어떻게 할까
                // 단순히 다음 인덱스의 테이블 위치를 얻어와도 되는걸까
                // 서빙한 주문에 대해서는 리스트에서 제거하고 null이 아니라면 해당 인덱스의 값에서 테이블 위치를 얻어오도록 해도되나?
                table = tableManager.GetTable((int)B_orderDatas[0].seatNum);
                tablePos = table.transform;
                // 이렇게 되면 tablePos는 private으로 변경 해도됨 - 로직 확정 및 임시로직 삭제 후

                // 주문한 테이블 위치로 이동
                yield return StartCoroutine(MoveToPos(tablePos.position));
                // 테이블에 음식 내려놓기
                ServeFood();
                // ServeFood()에서 서빙할 자리(의자) 위치 찾기 + OrderDatas에서 Remove하기 + 해당 테이블에 추가로 서빙할 음식이 있는지 찾아서 서빙하고 Remove하기 모두 해야함
                // 그럼 도착하면 일단 OrderDatas를 순회해서 해당 테이블에 서빙해야 할 음식의 주문들을 찾고 이걸 하나씩 서빙하면서 Remove해주면 될듯
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
        // 트레이에서 들 음식 정보 불러 오기
        GameObject B_handFood = trayControl.TakeFoodFromTray(trayIndex, handPos);
        OrderData B_orderData = trayControl.GetOrderData(trayIndex);

        if (B_handFood != null && B_orderData != null)
        {
            // B_handFood의 부모를 사장님의 손 위치로 설정
            B_handFood.transform.SetParent(handPos);
            // 들고있는 음식 큐에 넣기
            B_handFoods.Add(B_handFood);
            B_orderDatas.Add(B_orderData); // 주문 정보도 같이 저장
            trayControl.ClearFood(trayIndex); // 트레이에서 음식 및 주묹 정보 삭제
        }

    }

    // 테이블에 음식 내려놓기
    // 들고 있는 음식 리스트를 순회하며 도착한 테이블에 서빙할 음식이 더 있는지 확인 후 있으면 추가로 내려놓기(로직추가필요)
    void ServeFood()
    {
        // B_orderDatas 반영해서 재작성
        int tableNum = (int)B_orderDatas[0].seatNum; // 서빙할 테이블 번호
        for (int i = B_orderDatas.Count - 1; i >= 0; i--)
        {
            if ((int)B_orderDatas[i].seatNum == tableNum)
            {
                GameObject B_handFood = B_handFoods[i];

                // foodPos 결정 어떻게할지 필요. 일단 chairPos를 이용해서
                // 1.1~4.4 형식 가정하고 chairNum 계산
                int chairNum = (int)((B_orderDatas[i].seatNum - tableNum) * 10);

                // 의자 위치를 전달받지 못하는 거면 기존꺼 다시 쓰고 방법 새로 고안해야함
                // 의자 위치를 기반으로 음식을 올릴 위치 찾기
                Transform chairPos = table.chairPos[chairNum - 1];
                Vector3 spawnPos = chairPos.position + chairPos.forward * 1.3f;
                spawnPos.y = tablePos.position.y + 0.4f; // 테이블 높이에 맞춤
                // 테이블에 음식 생성
                GameObject tableFood = Instantiate(B_handFood, spawnPos, chairPos.rotation);

                // 테이블에 음식 생성 (기존코드, 수정 중 임시 주석 처리)
                // GameObject tableFood = Instantiate(B_handFood, table.chairPos[chairNum - 1].position + Vector3.up * 1, table.chairPos[chairNum - 1].rotation);

                // 테이블에 올릴 음식은 부모 해제 후 독립 개체로
                tableFood.transform.SetParent(null);

                // 손에서 오브젝트 삭제
                Destroy(B_handFood);


                B_orderDatas.RemoveAt(i); // 서빙한 주문 정보는 리스트에서 제거
                B_handFoods.RemoveAt(i); // 들고 있는 음식 리스트에서도 제거
            }
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
