using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorPool : MonoBehaviour
{
    public int poolsize;
    public GameObject prefab;
    public GameObject[] prefabs; //visitor1,2,3을 담고 있는 배열
    int randomIndex; //랜덤으로 사용할 숫자


    // stack 활용
    public Stack<Visitor> visitorStack = new ();

    void Awake()
    {
        for (int i = 0; i < poolsize; i++)
        {
            GenerateObj();
        }
    }

    private void GenerateObj()
    {
        randomIndex = Random.Range(0, prefabs.Length);
        // visitor prefab generate
        var tempObj = Instantiate(prefabs[randomIndex]);
        tempObj.transform.SetParent(transform);
        tempObj.SetActive(false); // deactivate obj - activate if needed

        // push not game obj, only component (class) of visitor
        var visitor = tempObj.GetComponent<Visitor>();
        visitorStack.Push(visitor);
    }
    
    public Visitor GetObj()
    {
        if (visitorStack.Count < 1)
        {
            // if there is no obj to pop
            GenerateObj();
        }

        var visitor = visitorStack.Pop();
        visitor.gameObject.SetActive(true);
        return visitor;
    }
       
    public void SetObj(Visitor visitor)
    {
        visitor.gameObject.SetActive(false);
        visitor.transform.position = Vector3.zero;
        visitorStack.Push(visitor);
    }
}
