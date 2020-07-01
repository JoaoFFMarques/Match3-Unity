using UnityEngine;

public class AudioControllerSFX : MonoBehaviour
{
    public static AudioControllerSFX instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
