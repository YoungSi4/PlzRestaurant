using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderMemo : MonoBehaviour
{
    public GameObject OrderMemoBlock;
    private TextMeshPro TMP;
    private string order;

    // FoodInfo
    private int foodNumber;

    void Start()
    {
        TMP = OrderMemoBlock.GetComponent<TextMeshPro>();
    }

    public void GetFoodInfoFromOrder(int foodNum)
    {
        foodNumber = foodNum;
    }

    public void SetText(string text)
    {
        TMP.text = text;
    }
}
