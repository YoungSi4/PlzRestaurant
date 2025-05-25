using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null)
                {
                    SetupInstance();
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        RemoveDuplicates();
    }
    private void RemoveDuplicates()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static void SetupInstance()
    {
        instance = (T)FindObjectOfType(typeof(T));
        if( instance == null)
        {
            GameObject gameobj = new GameObject();
            gameobj.name = typeof(T).Name;
            instance = gameobj.AddComponent<T>();
            DontDestroyOnLoad(gameobj);
        }
    }
}
