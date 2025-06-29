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


    private bool isFirstPerson = true; //현재 시점이 1인칭인지 아닌지 
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Cursor.lockState = CursorLockMode.Locked; //마우스를 화면 중앙에 고정
       // Cursor.visible = false; //마우스 커서를 안 보이게 함
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //T누르면 시점 변환, 나중에 인풋시스템으로 고쳐야함
        {
            SwitchToView();
        }
        Move();
    }
    void SwitchToView()
    {
        isFirstPerson = !isFirstPerson; //스위치 토글
        firstPersonCam.Priority = isFirstPerson ? 1 : 0; //1인칭이면 1인칭카메라의 우선순위를 1로 설정해 1인칭 카메라의 위치를 보이게 함
        topDownCam.Priority = isFirstPerson ? 0 : 1; //이건 반대
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); //인풋시스템으로 이동 값 받음
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>(); //2D 벡터(x:좌우,y:상하)로 받아옴.

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime; 
        mouseY = mouseInput.y* mouseSensitivity * Time.deltaTime;
        //Time.deltaTime으로 일관성 있게 받아주고

        xRotation -= mouseY;
        //XRotation(카메라 상하)은 mouseY값으로 정의한다. (X축 회전이기 때문에 y값이 바뀜)

        // 1) 카메라 각도를 아래처럼 정의하고 싶은데 
        // xRotation = Mathf.Clamp(xRotation, -50f, 50f); //-50f 50f가 넘어가면 그 값으로 설정됨.
        // cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 2) 카메라는 Rotation의 x값으로 상하로 움직일 수 있음. 카메라 rotation의 x값이 50이면 아래를 보게 됨
        // 3) 

        

        // 마우스를 위로 올리면 mouseY값은 +가 되는데 XRotation +=

        xRotation = Mathf.Clamp(xRotation, -50f, 50f); //-50f 50f가 넘어가면 그 값으로 설정됨.
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX); //플레이어 몸때기도 같이 회전해야 내가 보는 쪽으로 앞으로 갈 수 있다.
    }

    public void Move()
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction = camRight*moveInput.x + camForward*moveInput.y;

        rb.velocity = direction * 3f + Vector3.up * rb.velocity.y;
        //float horizontalSpeed = new Vector3(rb.velocity.x,0f,rb.velocity.z).magnitude;
        

    }
}
