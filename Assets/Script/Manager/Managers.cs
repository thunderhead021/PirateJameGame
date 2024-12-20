using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
