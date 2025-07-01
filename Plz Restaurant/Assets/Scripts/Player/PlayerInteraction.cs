using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    float distance = 3f;
    public MoveAndToggle moveAndToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //1) ���̸� ����� 2) if������ ���̸� ���� �� ����� �� 3) debug.drawray�� ���̸� �׷���
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (moveAndToggle.isFirstPerson) {
            if (Physics.Raycast(ray, out RaycastHit hit, distance))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                if (hit.collider.CompareTag("Visitor"))
                {
                    //Debug.Log("Visitor �߰�!");
                    if (Input.GetKeyDown(KeyCode.E)) //�̰� ��ǲ�ý������� ���ľ��ϴµ� 1��Ī�� �� 3��Ī�� �� ���� ���� ���� ���� ���� ����� ���⸸��
                    {
                        Debug.Log("visitor��ȣ�ۿ�");
                    }
                }
            }
        }
    }
}
