using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAndToggle : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam; //1인칭 시점일 때 카메라
    public CinemachineVirtualCamera topDownCam; // 3인칭 시점일 때 카메라


    public bool isFirstPerson = true; //현재 시점이 1인칭인지 아닌지 
    public float moveSpeed = 2f;  //플레이어 이동속도. 아마 나중에 플레이어 스크립트에 넣어야할듯
    
    Rigidbody rb; //리지드바디

    public Transform playerBody; //좌우 회전용 (몸통 전체)
    public Transform cameraHolder; // 상하 회전용 (카메라 부모)

    float mouseSensitivity = 10f; //마우스 감도값
    public float xRotation = 0f; // 카메라 상하 회전 값을 담을 변수
    public float mouseY;


    public Transform cameraTransform;

    Vector2 moveInput; //인풋시스템에서 키보드로 받을 벡터(x,y값으로 키보드 값을 받기 때문에 vector2)
    Vector3 camForward; // 카메라 앞뒤를 다룰 벡터 
    Vector3 camRight; // 카메라 왼쪽 오른쪽을 다룰 벡터 

    public Vector3 direction { get; private set; } //플레이어의 이동방향을 담을 벡터

    public PlayerInput playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked; //마우스를 화면 중앙에 고정
        Cursor.visible = false; //마우스 커서를 안 보이게 함
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //T누르면 시점 변환, 나중에 인풋시스템으로 고쳐야함
        {
            SwitchToView();
        }
        Move();
    }
    public void OnToggleInput(InputAction.CallbackContext ctx)  //On???Input 형태로 항상 작성해야하는듯
    {
        if (ctx.performed) //수행 됬을 때 여기서(인풋시스템)는 button으로 지정해서 눌렸을때?
        {
            SwitchToView();
        }
    }
    void SwitchToView()
    {
        isFirstPerson = !isFirstPerson; //스위치 토글
        firstPersonCam.Priority = isFirstPerson ? 1 : 0; //1인칭이면 1인칭카메라의 우선순위를 1로 설정해 1인칭 카메라의 위치를 보이게 함
        topDownCam.Priority = isFirstPerson ? 0 : 1; //이건 반대
        if (isFirstPerson)
            playerInput.SwitchCurrentActionMap("FirstPerspective"); //액션맵 교체
        else
            playerInput.SwitchCurrentActionMap("ThreePerspective");
    }



    //인풋시스템으로 이동 값 받음.
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); 
    }
    //인풋시스템으로 마우스의 2D 벡터(x:좌우,y:상하)로 받아옴.
    public void OnMouseInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>(); //2D 벡터(x:좌우,y:상하)로 받아옴.

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime; 
        mouseY = mouseInput.y* mouseSensitivity * Time.deltaTime;
        //Time.deltaTime으로 일관성 있게 받아주고

        // 1) 카메라는 Rotation의 x값으로 상하로 움직일 수 있음.카메라 rotation의 x값이 50이면 아래를 보게 됨
        // 2) XRotation(카메라 상하)은 mouseY값으로 정의한다. (X축 회전이기 때문에 y값이 바뀜)
        // 3) mouseY값이 커질수록(위를 향할 수록) 카메라의 Rotation의 x값이 -로 향해야 카메라가 위를 보게 됨

        // => 그래서 카메라 각도를 아래처럼 정의할 수 있음.
        xRotation -= mouseY; 

        xRotation = Mathf.Clamp(xRotation, -50f, 50f); //-50f 50f가 넘어가면 그 값으로 설정됨.
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //카메라의 로컬회전값(쿼터니언)을 바꾸는데 x축만 바꿔서 위아래로 바꾸게함. 

        playerBody.Rotate(Vector3.up * mouseX); //플레이어 몸때기도 같이 회전해야 내가 보는 쪽으로 앞으로 갈 수 있다.
        //Vector3.up = (0,1,0) => y축 회전(y방향)을 의미함, mouseX는 마우스 좌우 움직임, Rotate() : 해당 축을 기준으로 회전시킴 
    }


    public void Move()
    {
        if (!isFirstPerson && moveInput != Vector2.zero) //3인칭 이고 입력받는 값이 존재할때(키보드가 눌릴때?)
        {
            Vector3 direction3 = new Vector3(moveInput.x, 0, moveInput.y); //3인칭일 때는 그냥 인풋값자체를 direction으로 받아야함... 카메라 상관없이 좌표대로 움직여야하기 때문에..
            playerBody.rotation = Quaternion.LookRotation(direction3);
            rb.velocity = direction3.normalized * moveSpeed;
            //플레이어의 rotation을 부드럽게 회전해줌.
            //Quaternion.Slerp(현재회전, 목표회전, 속도)
        }
        else // 1인칭일 때
        {
            camForward = cameraTransform.forward; //카메라가 앞을 보는 방향
            camRight = cameraTransform.right; // 카메라의 오른쪽 방향

            camForward.y = 0f; //각각 카메라의 위/아래 방향 성분은 제거해주고
            camRight.y = 0f;

            camForward.Normalize(); //벡터의 길이를 1로 만듦(방향만 받아준다?)
            camRight.Normalize();

            Vector3 direction = camRight * moveInput.x + camForward * moveInput.y;
            // moveInput.x는 양 옆(A/D)으로 이동하는 값을 받는거고 moveInput.y는 앞뒤(W/S)로 이동하는 값을 받는다.
            // 대각선으로 가기 위해서는 둘다 같이 받아서 정의해줘야 힘이 같이 실린다.

            // w눌렀을 때 현재 보고 있는 카메라를 기준으로 앞으로 가야하기 때문에 카메라의 방향을 받은 것

            rb.velocity = direction * 3f;
            //+ Vector3.up * rb.velocity.y;
            // 리지드바디의 벨로시티(물체 전체 속도)는 위에서 받은 direction에 player의 임의의 속도를 곱해주고
            // 점프가 필요하다면 y축방향으로의 y축 속도를 유지하기 위해 썼지만 점프가 현재는 필요 없어서 뺌

            //float horizontalSpeed = new Vector3(rb.velocity.x,0f,rb.velocity.z).magnitude;
            //magnitude는 벡터의 길이값(노름or제곱?)을 구하는데 이걸로 플레이어의 속도값?을 확인할 수 있음
            //=> 나중에 달릴 때랑 걸을 때를 이 값으로 구분해서 애니메이션 넣을 수 있음.

        }
    }
    // Rotation값은 기본적으로 쿼터니언(4차원 회전값)인데
    // 이 쿼터니언을 우리가 다룰수 없음(?) 그래서 Euler를 붙여서 우리가 알 수 있게 3차원으로
    // 정의를 할 수 있는거임. ex) Quaternion.Euler(xRotation, 0f, 0f);
    // 그래서 회전값 자체를 정의할 때 빼고는 보통 위처럼 quaternion이 사용됨. 
}
