using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int amount { get; private set; }

    private void Awake()
    {
        amount = 0;
    }

    public void Init(int price)
    {
        amount = price;
    }

    /// <summary>
    /// when you interact to money, you get this money
    /// and this money disappear
    /// </summary>
    public void getMoney()
    {
        DisableObj();
    }

    // instantiate, destroy·Î Ã³¸®
    private void DisableObj()
    {
        GameObject.Destroy(gameObject);
    }
}
