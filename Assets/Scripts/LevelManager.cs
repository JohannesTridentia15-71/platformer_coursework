using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    // creates the manager if no scene has one yet
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<LevelManager>();
                if (_instance == null)
                {
                    new GameObject("LevelManager").AddComponent<LevelManager>();
                }
            }
            return _instance;
        }
    }

    // old save key: progress used to be stored in PlayerPrefs, it's now cleared on start
    private const string ProgressKey = "LevelsCompleted";

    // 0 = nothing done, 1 = tutorial done, 2 = level 1 done, ...
    // only kept in memory, so progress resets every time the game is closed
    private int _levelsCompleted;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        _levelsCompleted = 0;

        // remove progress saved by older versions of the game
        if (PlayerPrefs.HasKey(ProgressKey))
        {
            PlayerPrefs.DeleteKey(ProgressKey);
            PlayerPrefs.Save();
        }
    }

    // levelIndex: 0 = tutorial, 1-5 = levels
    public bool IsUnlocked(int levelIndex) => levelIndex <= _levelsCompleted;

    // true once the tutorial has been finished at least once
    public bool HasCompletedTutorial => _levelsCompleted > 0;

    public void CompleteLevel(int levelIndex)
    {
        _levelsCompleted = Mathf.Max(_levelsCompleted, levelIndex + 1);
    }

    public void ResetProgress()
    {
        _levelsCompleted = 0;
    }
}
