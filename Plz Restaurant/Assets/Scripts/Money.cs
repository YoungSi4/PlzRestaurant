using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int amount { get; private set; }
    Table table;

    private void Awake()
    {
        amount = 0;
    }

    public void Init(int price, Table table)
    {
        amount = price;
        this.table = table;
    }

    /// <summary>
    /// when you interact to money, you get this money
    /// and this money disappear
    /// </summary>
    public void GetMoney()
    {
        CallTableCleanUp();
        DisableObj();
    }

    // instantiate, destroy·Î Ã³¸®
    private void DisableObj()
    {
        Destroy(gameObject);
    }

    private void CallTableCleanUp()
    {
        table.TableCleanUp();
    }
}
