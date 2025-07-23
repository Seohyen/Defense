using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    private static bool _isShuttingDown = false;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_isShuttingDown)
            {
                Debug.LogWarning($"[Singleton<{typeof(T)}>] Instance already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        GameObject singletonObj = new GameObject(typeof(T).Name);
                        _instance = singletonObj.AddComponent<T>();
                        DontDestroyOnLoad(singletonObj);
                    }
                }

                return _instance;
            }
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
            OnAwake(); 
        }
        else if (_instance != this)
        {
            Destroy(gameObject); 
        }
    }

    private void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }

    /// <summary>
    /// 하위 클래스에서 구현할 Awake 추상화 메서드
    /// </summary>
    protected abstract void OnAwake();
}
