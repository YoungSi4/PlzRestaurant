using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour //기현상을 관리하는 스크립트
{
    //Dictionary<int, string> anomalys = new Dictionary<int, string>();

    int anomalyInterval; //기현상이 나타나는 주기
    public Anomaly anomaly;
    int idx; //기현상이 여러가지 있는데 발생할 그 랜덤순서를 정의할 변수

    private void Start()
    {
        anomalyInterval = 30;
    }

    IEnumerator TriggerAnomalyRoutine() //기현상 발생(아마 게임 시작하면 GameManger가 이 코루틴을 시작하면 될거 같다.)
    {
        while (true)
        {
            yield return new WaitForSeconds(anomalyInterval);
            idx = Random.Range(1, 6);
            anomaly.A_triggerEvent(idx); //anomaly객체로 넘어갔다가 거기서 다시 AnomalyManger로 와서 기현상을 실행시켜줌
        }
    }
    public void Anomaly(int id) //실제 기현상이 수행되는 코드를 적어둘 듯?
    {
        switch (id) 
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
}
