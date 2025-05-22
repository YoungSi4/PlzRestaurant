using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderMemo : MonoBehaviour
{
    public GameObject OrderMemoBlock;
    private TextMeshPro TMP;
    private string order;

    void Start()
    {
        TMP = OrderMemoBlock.GetComponent<TextMeshPro>();
    }

    public void SetText(string text)
    {
        TMP.text = text;
    }
}
