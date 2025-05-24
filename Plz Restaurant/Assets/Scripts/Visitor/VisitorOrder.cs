using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorOrder : MonoBehaviour
{
    private int FoodNum; // FoodDB ���� ���� index. �մԿ��Լ� �����ϰ� ������.
    private OrderMemo orderMemo; // �÷��̾� UI ���� �ֹ� ���

    // Vars related to FoodDB
    // ���� ��ȣ, �̸�, ���� ���

    private void Start()
    {
        // initialize
        orderMemo = GameObject.FindObjectOfType<OrderMemo>();
    }



    // setter, getter
    // ���� ���� ������ �����ϴ� �Լ� - �÷��̾��� ��ȣ�ۿ� E���� ���
    // SetFoodNum -> GetFoodInfo -> SendFoodInfo
    public void SetFoodNum(int foodNum)
    {
        FoodNum = foodNum;
        GetFoodInfoFromDB(FoodNum);
    }

    // VistorOrder -> FoodDB -> VisitorOrder
    private void GetFoodInfoFromDB(int  foodNum)
    {
        // out �̳� ref �� Ȱ���ؼ� ������ ��
        // foodDB.getInfo(foodNum, &name, &price, &img, etc);
    }



    public void SendFoodInfo(int foodNum)
    {
        // OrderMemo �󿡼� ǥ�õ� ������ �� �Լ����� �ʱ�ȭ
        orderMemo.GetFoodInfoFromOrder(foodNum);
    }
}
