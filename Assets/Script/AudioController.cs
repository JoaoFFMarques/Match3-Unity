using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
