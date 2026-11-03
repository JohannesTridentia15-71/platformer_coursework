using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // already playing, remove the duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // keep this object between scenes
    }
}