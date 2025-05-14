using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : MonoBehaviour
{
    private VisitorPool pool;
    private WaitForSeconds wait = new WaitForSeconds(5f);

    public void Init(VisitorPool pool)
    {
        this.pool = pool;
        StartCoroutine(DisableObj());
    }

    private void Update()
    {
        transform.Translate(0f, 0f, 5f * Time.deltaTime);
        transform.Rotate(new Vector3(1f, 0f, 0f), 50f  * Time.deltaTime); 
    }

    private IEnumerator DisableObj()
    {
        yield return wait;
        pool.SetObj(this);
    }
    // ai navigation

}
