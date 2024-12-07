using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;
    public static T Instance { get { return instance; } }

    // This protected void acts like a hybrid of public and private;
    // it's private for everything except classes that inherit from this one
    protected virtual void Awake()
    {
        // If out instance of 'this' thing already exists in our scene, for 
        // example our player, we want to destroy the duplicate.
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        // If not, we're assigning 'instance' to whatever game object this 
        // script is attached to. 
        else
        {
            instance = (T)this;
        }

        // If this is a new game object, we don't want to destroy on load
        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
