using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderMemo : MonoBehaviour
{
    // ���ڰ� �������� �Ʒ�
    public GameObject OrderMemoBlock1;
    public GameObject OrderMemoBlock2;
    public GameObject OrderMemoBlock3;
    public GameObject OrderMemoBlock4;
    private TextMeshPro TMP;

    private int tableNum;
    private string foodName;
    // food image?
    private int foodPrice;

    // FoodInfo
    private VisitorOrder order;
    private FoodData foodData;

    void Start()
    {
        TMP = OrderMemoBlock1.GetComponent<TextMeshPro>();
    }

    // FoodDB ��ü���� �� �Լ��� ����...?
    // Food Manager ���� �߰� �Ű�ü�� �����ϴ� �� �����غ��δ�.
    
    // ���̺� ��ȣ�� ��� �޾Ƽ� �ѱ���? -> visitor order ��ü
    public void GetFoodInfo(ref FoodData foodData, int tableNum)
    {
        this.foodData = foodData;
        this.tableNum = tableNum;
        
    }

    private void SetData()
    {
        foodName = foodData.foodName;
        foodPrice = foodData.foodPrice; 
    }

    private void SetText()
    {
        
    }
}
