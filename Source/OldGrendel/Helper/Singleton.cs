using UnityEngine;
using System.Collections;
using System.Reflection;

    /// <summary>
    /// Title: Grendel Engine
    /// Author: Elliot Hudson
    /// Date: Mar 28, 2012
    /// 
    /// Filename: Singleton.cs
    /// 
    /// Summary: Subclass in order to create a singleton.
    /// Ensures that only one instance exists in the scene
    /// 
    /// </summary>

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{ 
    public bool DoNotDestroyOnLoad = false;
    public bool DestroyGameObject = false;
    public bool OverrideInstance = false; //set to true if you would like the new instance to take the place of the old instance, as opposed to carrying the old instance forward
    
    protected static T mInstance;
    
    static public T Instance
    {
        get 
        {
            if (mInstance == null)
            {
                Debug.LogWarning(string.Format("Component of type <{0}> does not exist in the scene", typeof(T).ToString() ));
            }
            
            return mInstance;
        }
    }
        
    virtual protected void Awake()
    {
        //checks if there is already an instance in the game
        //and destroys this object if it is a duplicate
        if (mInstance == null)
        {
            mInstance = this as T;
            if (DoNotDestroyOnLoad){ DontDestroyOnLoad(gameObject); }
        }
        else
        {

            MonoBehaviour targetToDestroy = (OverrideInstance == true ? mInstance as MonoBehaviour : this) ;

            targetToDestroy.enabled = false;

            if (DestroyGameObject)
            {
                GameObject.Destroy( targetToDestroy.gameObject );
            }
            else
            {
                Destroy( targetToDestroy );
            }

            if (!targetToDestroy.Equals(this))
            {
                mInstance = this as T;
            }
        }
    }
}
