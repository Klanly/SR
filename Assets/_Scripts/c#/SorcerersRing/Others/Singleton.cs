using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T: class
{
    protected static T _instance = null;
    public static T instance
    {
        get
        {
            if (_instance == null)
                _instance = SingletonManager.gameObject.AddComponent(typeof(T)) as T;
            return _instance;
        }
    }
	
    protected virtual void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }
	
    public static void Instantiate()
    {
        _instance = instance;
    }

    public Singleton()
    {
        _instance = this as T;
    }
	
    public void ExecuteAfterCoroutine(IEnumerator coroutine, System.Action action)
    {
        StartCoroutine(ExecuteAfterCoroutineActual(coroutine, action));
    }
				
    public IEnumerator ExecuteAfterCoroutineActual(IEnumerator coroutine, System.Action action)
    {
        yield return StartCoroutine(coroutine);
        action();
    }

    public static bool HasInstance
    {
        get
        {
            return _instance != null;
        }
    }
}

public class SingletonManager
{
    private static GameObject _gameObject = null;
    public static GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject("-SingletonManager");
				
            }
            Object.DontDestroyOnLoad(_gameObject);
            return _gameObject;
        }
    }
}
