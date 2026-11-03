using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private const string ProgressKey = "LevelsCompleted";

    // 0 = nothing done, 1 = tutorial done, 2 = level 1 done, ...
    private int _levelsCompleted;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _levelsCompleted = PlayerPrefs.GetInt(ProgressKey, 0);
    }

    // levelIndex: 0 = tutorial, 1-3 = levels
    public bool IsUnlocked(int levelIndex) => levelIndex <= _levelsCompleted;

    public void CompleteLevel(int levelIndex)
    {
        _levelsCompleted = Mathf.Max(_levelsCompleted, levelIndex + 1);
        PlayerPrefs.SetInt(ProgressKey, _levelsCompleted);
        PlayerPrefs.Save();
    }

    public void ResetProgress()
    {
        _levelsCompleted = 0;
        PlayerPrefs.DeleteKey(ProgressKey);
    }
}
