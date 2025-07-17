using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly : MonoBehaviour //이 스크립트는 실제 기현상을 다루는 스크립트
{
    int A_id; //고유 id
    string A_description; //기현상 내용
    bool A_isResolved; //처리 여부
    int A_timeLimit; //제한시간
    int A_penaltyCost; //처리 실패시 차감되는 금액

    public AnomalyManager anomalyManger;
    int id; // AnomalyManger에서 넘어온 idx를 담을 id (이거로 A_id랑 비교함)
    bool A_isActive; //기현상이 실행 중인지
    float timer = 0f;

    private void Update()
    {

    }


    public void A_triggerEvent(int idx) //기현상 발생
    {
        anomalyManger.Anomaly(idx);
        id = idx;
        A_isActive = true;
    }

    void A_Resolve()
    {

    }

    void A_calculatePenalty() //처리 실패시 차감되는 금액 계산
    {
        if (id == A_id)
        {
            Debug.Log(A_penaltyCost + "계산처리"); //넘어온 번호에 해당하는 객체만 됨
        }
    }

    void A_applyPenalty() //미처리 시 벌금 적용
    {
        if (id == A_id)
        {
            Debug.Log(A_penaltyCost + "만큼 빼기"); //넘어온 번호에 해당하는 객체만 됨
        }
    }
}
