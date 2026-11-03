using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _deathScreenPanel;
    [SerializeField] private GameObject _victoryScreenPanel;

    [SerializeField] private GameObject _pauseScreenPanel;
    [SerializeField] private int _levelIndex = 1;

    private void Start()
    {
        // Make sure UI panels are hidden when the game starts
        if (_deathScreenPanel != null) _deathScreenPanel.SetActive(false);
        if (_victoryScreenPanel != null) _victoryScreenPanel.SetActive(false);
        if (_pauseScreenPanel != null) _pauseScreenPanel.SetActive(false);
        
        Time.timeScale = 1f; // Ensure time is running normally
    }


    public void ShowDeathScreen()
    {
        if (_deathScreenPanel != null)
        {
            _deathScreenPanel.SetActive(true);
        }
        //Time.timeScale = 0f; 
    }

    
    public void ShowVictoryScreen()
    {
        if (_victoryScreenPanel != null)
        {
            _victoryScreenPanel.SetActive(true);
        }
        Time.timeScale = 0f; // Freeze game on victory

        //completes the level and unlocks it in the main menu
        //set the index for each level (i.e., Level 1 = 1, Level 2 = 2, etc)
        if  (LevelManager.Instance != null)
        {
            LevelManager.Instance.CompleteLevel(_levelIndex);
        }
        
    }

    public void ShowPauseScreen()
    {
        if(_pauseScreenPanel != null)
        {
            _pauseScreenPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeLevel()
    {
        Time.timeScale = 1f;
        if(_pauseScreenPanel != null)
        {
            _pauseScreenPanel.SetActive(false);
        }

    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName.Trim());
    }

}
