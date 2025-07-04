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
        //1) 레이를 만들고 2) if문에서 레이를 쐈을 때 닿았을 때 3) debug.drawray로 레이를 그려줌
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (moveAndToggle.isFirstPerson) {
            if (Physics.Raycast(ray, out RaycastHit hit, distance))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                if (hit.collider.CompareTag("Visitor"))
                {
                    //Debug.Log("Visitor 발견!");
                    if (Input.GetKeyDown(KeyCode.E)) //이건 인풋시스템으로 고쳐야하는데 1인칭일 때 3인칭일 때 아직 어케 할지 몰라서 쉽게 만들어 놓기만함
                    {
                        Debug.Log("visitor상호작용");
                    }
                }
            }
        }
    }
}
